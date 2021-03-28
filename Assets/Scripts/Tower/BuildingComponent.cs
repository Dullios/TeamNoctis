using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

//The component that allows a gameobject to build towers
//Should be disabled until in use
public class BuildingComponent : MonoBehaviour
{
    public GameObject towerChoice;
    public float placeDistance = 10.0f;
    public float placeAngle = 20.0f;
    public LayerMask groundLayer;

    private bool canPlace;
    private GameObject tower;

    // for quest
    [HideInInspector]
    public UnityEvent OnPlaceTower;

    //Instantiate the tower choice 
    private void OnEnable()
    {
        tower = Instantiate(towerChoice, transform);
        tower.GetComponent<TowerController>().isActivated = false;
        _Selected(tower);
        tower.SetActive(false);
        Debug.Log("Building Beginning");
    }

    //Destroy and reset the tower
    private void OnDisable()
    {
        if (tower !=null)
        {
            Destroy(tower);
            tower = null;
        }
        Debug.Log("Building Stopping");
    }

    // Update is called once per frame
    void Update()
    {
        _SearchGround();
        if (Input.GetMouseButtonDown(1))
        {
            PlaceTower();
        }
    }

    //searches for a suitable spot to place a tower
    private void _SearchGround()
    {
        Vector3 rayStart = Quaternion.AngleAxis(placeAngle, transform.right) * transform.forward;
        if (Physics.Raycast(transform.position, rayStart, out RaycastHit hit, placeDistance, groundLayer))
        {//hit ground
            if (!canPlace)
            {//enable the tower
                Assert.IsNotNull(towerChoice, "Oops, tower not found in Builder");
                Debug.Log("Found Ground");
                tower.SetActive(true);
                canPlace = true;
            }
            if (tower != null)
            {//update tower's position to blocks top center
                Vector3 blockPos = hit.collider.gameObject.transform.position;
                Vector3 newPos = new Vector3(blockPos.x, blockPos.y + 1, blockPos.z);
                tower.transform.position = newPos;
            }
        }
        else
        {//delete existing tower and allow another to be placed later
            canPlace = false;
            tower.SetActive(false);
        }

        Debug.DrawRay(transform.position, rayStart * placeDistance);
    }

    //Adds transparency and tint to selection
    private void _Selected(GameObject selection)
    {
        Color tint = new Color(0, 0, 1, 0.5f);

        foreach (MeshRenderer renderer in selection.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material.color = tint;
        }
    }
    //Removes transparency and tint from selection
    private void _Deselected(GameObject selection)
    {
        Color detint = new Color(1, 1, 1, 1);

        foreach (Renderer renderer in selection.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = detint;
        }
    }

    //places the tower prefab and sleeps this component
    public void PlaceTower()
    {
        if (canPlace)
        {//remove from builder transform and builder
            Assert.IsNotNull(tower, "Oops, no tower available");
            _Deselected(tower);
            tower.GetComponent<TowerController>().isActivated = true;
            tower.transform.parent = null;
            tower = null;
            canPlace = false;

            // start event for quest
            if(OnPlaceTower!= null)
            {
                OnPlaceTower.Invoke();
            }

            StopBuilding();
        }
    }

    public void StopBuilding()
    {
        this.enabled = false;
    }
}
