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

    [Header("HP & Stamina")]
    public float currentStamina = 0f;
    public float maxStamina = 100f;
    public Image hpImg = null;
    public Image staminaImg = null;
    public Text hpText = null;
    public Text staminaText = null;
    public HUDButton HUD = null;

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
        currentStamina = maxStamina;
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

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        character.Move(move * stats.moveSpeed * Time.deltaTime);

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded && currentStamina > 20f)
        {
            Debug.Log("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            currentStamina -= 20f;
            
        }

        //temp code about losing hp and call game over
        if (Input.GetKey(KeyCode.Q) && stats.hp > 0)
        {
            stats.HpModify(-1.0f);
            if(stats.hp <= 0)
                HUD.GoGameOver();
        }

        // gravity
        velocity.y += gravity * Time.deltaTime;
        character.Move(velocity * Time.deltaTime);

        // stamina recovery
        if(currentStamina < maxStamina)
        {
            currentStamina += 0.25f ;
        }

        // set hp, stamina gauges
        ManageHP();
        ManageStamina();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    public void ManageHP()
    {
        if (hpImg != null)
        {
            hpImg.fillAmount = stats.hp / stats.maxHp;
            hpText.text = string.Format("{0}", Mathf.Floor(stats.hp));
        }
    }
    public void ManageStamina()
    {
        if (staminaImg != null)
        {
            staminaImg.fillAmount = currentStamina / maxStamina;
            staminaText.text = string.Format("{0}", Mathf.Floor(currentStamina));
        }
    }

    //Pass in a tower prefab to begin placing it
    public void StartBuilding(GameObject tower)
    {
        Time.timeScale = 1.0f;
        builder.towerChoice = tower;
        builder.enabled = true;
    }
}
