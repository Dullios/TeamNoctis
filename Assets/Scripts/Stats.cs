using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    public float currnetHP = 0f;
    public float maxHp = 100.0f;
    public float currentStamina = 0f;
    public float maxStamina = 100f;

    public float damage = 10.0f;
    public float moveSpeed = 3.5f;
    public float attackRadius = 1.0f;
    public float attackSpeed = 0.5f;

    public Image hpImg = null;
    public Image staminaImg = null;
    public Text hpText = null;
    public Text staminaText = null;

    [Header("Debug")]
    [SerializeField] float debugSphereYOffset = 0f; //Gizmo spherer drawing y offset

    [Header("Sounds")]
    public AudioSource hitSFX;

    HUDButton hUDButton;

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        currnetHP = maxHp;

        hUDButton = FindObjectOfType<HUDButton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HpModify(float hpModifier)
    {
        currnetHP += hpModifier;
        hitSFX.Play();
        if(currnetHP <= 0)
        {
            hUDButton.GoGameOver();
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.white;

        Vector3 temp = transform.position;
        temp.y += debugSphereYOffset;

        //Gizmos.DrawWireSphere(temp, inSightRadius);

        //Draw attack radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(temp, attackRadius);
    }

    public void ManageHP()
    {
        if (hpImg != null)
        {
            hpImg.fillAmount = currnetHP / maxHp;
            hpText.text = string.Format("{0}", Mathf.Floor(currnetHP));
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
}
