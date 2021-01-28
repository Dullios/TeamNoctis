using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 1000.0f;
    public Transform playerBody;

    //private float XRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //XRotation -= mouseY;
        //XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        //transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 30.0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
