using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float gravity = -30.0f;
    public float jumpHeight = 10.0f;
    private Vector3 velocity;

    public Joystick joystickMove;
    public Joystick joystickLook;

    public Transform groundCheck;
    public float groundRadius = 0.5f;
    public LayerMask groundMask;

    private bool isGrounded;
    private CharacterController character;
    private BuildingComponent builder;
    private Stats stats;
    public HUDButton HUD = null;

    [Header("Sounds")]
    public AudioSource walkingSFX;
    public AudioSource jumpSFX;

    InventorySlot inventorySlot;

    // Start is called before the first frame update
    void Start()
    {
        HUD = FindObjectOfType<HUDButton>();
        character = GetComponent<CharacterController>();
        //Builder should only be enabled when in use
        builder = GetComponent<BuildingComponent>();
        builder.enabled = false;
        //Set Stats
        stats = GetComponent<Stats>();
        inventorySlot = FindObjectOfType<InventorySlot>();

        StartCoroutine(Walking());
        // save & load
        //SaveLoad.OnSave.AddListener(OnSaveBack);
        //SaveLoad.OnLoad.AddListener(OnLoadBack);
    }

    // Update is called once per frame
    void Update()
    {
        //float x = joystick.Horizontal;
        //float z = joystick.Vertical;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        float x = 0;
        float z = 0;

        if (GlobalData.HasInstance)
        {
            if (Input.GetKey(GlobalData.instance.keys["UP"]))
            {
                z = 1;
            }
            if (Input.GetKey(GlobalData.instance.keys["DOWN"]))
            {
                z = -1;
            }
            if (Input.GetKey(GlobalData.instance.keys["LEFT"]))
            {
                x = -1;
            }
            if (Input.GetKey(GlobalData.instance.keys["RIGHT"]))
            {
                x = 1;
            }

        }

        Vector3 move = transform.right * x + transform.forward * z;
        character.Move(move * stats.moveSpeed * Time.deltaTime);

        // jump
        if (GlobalData.HasInstance)
        {
            if (Input.GetKey(GlobalData.instance.keys["JUMP"]) && isGrounded && stats.currentStamina > 20f)
            {
                jumpSFX.Play();

                Debug.Log("Jump");
                velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
                stats.currentStamina -= 20f;
            }
        }
       
        //temp code about losing hp and call game over
        if (Input.GetKey(KeyCode.Q) && stats.currnetHP > 0)
        {
            stats.HpModify(-1.0f);
            if(stats.currnetHP <= 0)
                HUD.GoGameOver();
        }

        // gravity
        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);

        // stamina recovery
        if(stats.currentStamina < stats.maxStamina)
        {
            stats.currentStamina += 0.25f ;
        }

        // set hp, stamina gauges
        stats.ManageHP();
        stats.ManageStamina();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    IEnumerator Walking()
    {
        yield return new WaitForSeconds(.7f);
        if (isGrounded && character.velocity.magnitude > 0)
            walkingSFX.Play();
    }

    //Pass in a tower prefab to begin placing it
    public void StartBuilding(GameObject tower)
    {
        Time.timeScale = 1.0f;
        builder.towerChoice = tower;
        builder.enabled = true;
    }



    //save load 
    void OnSaveBack()
    {
        // playet position and rotation
        string posData = "";
        string rotData = "";
        posData = transform.position.x + "," + transform.position.y + "," + transform.position.z;
        rotData = transform.eulerAngles.x + "," + transform.eulerAngles.y + "," + transform.eulerAngles.z;

        PlayerPrefs.SetString("PlayerPos", posData);
        PlayerPrefs.SetString("PlayerRot", rotData);

        // inventory
    }

    //public void OnLoadBack()
    //{
    //    char[] delimeters = new char[] { ',' };
    //    // playet position and rotation
    //    if (PlayerPrefs.HasKey("PlayerPos") && PlayerPrefs.HasKey("PlayerRot"))
    //    {
    //        string savedPos = PlayerPrefs.GetString("PlayerPos");
    //        string savedRot = PlayerPrefs.GetString("PlayerRot");

    //        string[] splitePos = savedPos.Split(delimeters);
    //        string[] spliteRot = savedRot.Split(delimeters);

    //        float posX = float.Parse(splitePos[0]);
    //        float posY = float.Parse(splitePos[1]);
    //        float posZ = float.Parse(splitePos[2]);

    //        float rotX = float.Parse(spliteRot[0]);
    //        float rotY = float.Parse(spliteRot[1]);
    //        float rotZ = float.Parse(spliteRot[2]);


    //        transform.position = new Vector3(posX, posY, posZ);
    //        transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
    //    }
    //}
}
