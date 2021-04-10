using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Transform rayStart;
    public float raycastDistance = 1.0f;

    public GameObject AlchemyTable;

    private bool _tableSummoned = false;
    
    // Update is called once per frame
    void Update()
    {
        //Mouse
        if (Input.GetMouseButton(0) && Application.platform != RuntimePlatform.Android)
        {
            Extract();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTable();
        }
    }

    public void Extract()
    {
        Debug.DrawLine(rayStart.position, rayStart.position + (rayStart.forward * raycastDistance), Color.green, 1.0f);

        //Simple raycast
        RaycastHit result;
        if (Physics.Raycast(rayStart.position, rayStart.forward, out result, raycastDistance))
        {
            //Debug.Log(result.collider.gameObject.name);

            //Check if this is collectable object
            CollectableObject collectableObject = result.collider.gameObject.GetComponent<CollectableObject>();
            if (collectableObject != null)
            {
                //collectableObject.Collected();
                collectableObject.collecting = true;
            }

            AlchemyTable alchemyTable = result.collider.gameObject.GetComponent<AlchemyTable>();
            if (alchemyTable)
            {
                alchemyTable.AddGoldToInventory();
            }
        }
    }

    public void ToggleTable()
    {
        RaycastHit result;
        if (Physics.Raycast(rayStart.position, rayStart.forward, out result, raycastDistance))
        {
            AlchemyTable Table = result.collider.gameObject.GetComponent<AlchemyTable>();
            if (Table)
            {
                Destroy(Table.gameObject);
                _tableSummoned = false; 
            }
            else
            {
                if (_tableSummoned) return;
                Instantiate(AlchemyTable, result.point, Quaternion.identity);
                _tableSummoned = true;
            }
        }
    }
}
