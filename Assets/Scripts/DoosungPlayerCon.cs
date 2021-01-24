using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoosungPlayerCon : MonoBehaviour
{
    public float raycastDistance = 100.0f;
    CharacterController characterController;

    public float speed = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //float angle = 
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            characterController.Move(direction * speed * Time.deltaTime);
        }


        if (Input.GetMouseButtonDown(0))
        {
            Debug.DrawLine(transform.position, transform.position + (transform.forward * raycastDistance), Color.green, 1.0f);

            //Simple raycast
            RaycastHit result;
            if(Physics.Raycast(transform.position, transform.forward, out result, raycastDistance))
            {
                Debug.Log(result.collider.gameObject.name);

                //Check if this is collectable object
                CollectableObject collectableObject = result.collider.gameObject.GetComponent<CollectableObject>();
                if(collectableObject != null)
                {
                    collectableObject.Collected();
                }
            }
        }
    }
}
