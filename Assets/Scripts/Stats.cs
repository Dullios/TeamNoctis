using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    public float hp = 100.0f;
    public float maxHp = 100.0f;
    public float damage = 10.0f;
    public float moveSpeed = 3.5f;
    public float attackRadius = 1.0f;
    public float attackSpeed = 0.5f;

    [Header("Debug")]
    [SerializeField] float debugSphereYOffset = 0f; //Gizmo spherer drawing y offset

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HpModify(float hpModifier)
    {
        hp += hpModifier;
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
}
