using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public Transform rayStart;
    public float raycastDistance = 100.0f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.DrawLine(rayStart.position, rayStart.position + (rayStart.forward * raycastDistance), Color.green, 1.0f);

            //Simple raycast
            RaycastHit result;
            if (Physics.Raycast(rayStart.position, rayStart.forward, out result, raycastDistance))
            {
                Debug.Log(result.collider.gameObject.name);

                //Check if this is collectable object
                CollectableObject collectableObject = result.collider.gameObject.GetComponent<CollectableObject>();
                if (collectableObject != null)
                {
                    //collectableObject.Collected();
                    collectableObject.collecting = true;
                }
            }
        }
    }
}
