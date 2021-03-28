using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 1000.0f;
    public Transform playerBody;
    public Joystick joystickLook;

    public float XRotation = -90.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
            return; //Don't lock cursor on phone

        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX;
        float mouseY;

        if (Application.platform == RuntimePlatform.Android)
        {
            mouseX = joystickLook.Horizontal * sensitivity * Time.deltaTime;
            mouseY = joystickLook.Vertical * sensitivity * Time.deltaTime;
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        }

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
