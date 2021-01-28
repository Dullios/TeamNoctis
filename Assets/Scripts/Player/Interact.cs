using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public float raycastDistance = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.DrawLine(transform.position, transform.position + (transform.forward * raycastDistance), Color.green, 1.0f);

            //Simple raycast
            RaycastHit result;
            if (Physics.Raycast(transform.position, transform.forward, out result, raycastDistance))
            {
                Debug.Log(result.collider.gameObject.name);

                //Check if this is collectable object
                CollectableObject collectableObject = result.collider.gameObject.GetComponent<CollectableObject>();
                if (collectableObject != null)
                {
                    collectableObject.Collected();
                }
            }
        }
    }
}
