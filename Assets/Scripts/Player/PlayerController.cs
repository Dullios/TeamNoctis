using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 10.0f;
    public float gravity = -30.0f;
    public float jumpHeight = 10.0f;

    public Joystick joystickMove;
    public Joystick joystickLook;

    public Transform groundCheck;
    public float groundRadius = 0.5f;
    public LayerMask groundMask;

    private CharacterController character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //float x = joystick.Horizontal;
        //float z = joystick.Vertical;
        Vector3 x = transform.forward * Input.GetAxisRaw("Vertical") * maxSpeed * Time.deltaTime;
        Vector3 z = transform.right * Input.GetAxisRaw("Horizontal") * maxSpeed * Time.deltaTime;
        Vector3 move = x + z;

        character.Move(move);
    }
}
