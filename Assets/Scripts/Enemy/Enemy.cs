using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    const float locomotionAnimationSmoothTime = 0.1f; //interpolation time between two value

    //Comp
    Animator animator;
    NavMeshAgent navMeshAgent;

    [SerializeField] Transform target;
    [SerializeField] float attackRadius = 10;

    [SerializeField] float sphereDrawPositionYOffset = 2f; //Gizmo spherer drawing y offset

    [SerializeField] float moveSpeed = 3.5f;
    [SerializeField] float attackSpeed = 0.5f; //attack per second

    bool hasAttacked = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        //Find player with tag
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
        {
            Debug.LogWarning("EnemyMove : Can't find player with tag(Player)");
        }

        //Set speed
        navMeshAgent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //If we have target
        if (target != null)
        {
            //Distacne from to target
            float distance = Vector3.Distance(transform.position, target.position);
            //Take account of position volume offset radius into distacne, see description of PositionVolumeOffset class
            PositionVolumeOffset pvoComp = target.gameObject.GetComponent<PositionVolumeOffset>();
            if (pvoComp != null)
            {
                distance -= pvoComp.offsetRadius;
            }

            //If target is closer than attack radius...
            if(distance <= attackRadius)
            {
                //Stop moving
                navMeshAgent.velocity = Vector3.zero;

                //Face target
                FaceTarget();

                //Attack
                Attack();
            }
            //GO to target
            else
            {
                navMeshAgent.SetDestination(target.position);
            }
        }

        SetAnimationSpeedPercent();
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.white;

        Vector3 temp = transform.position;
        temp.y += sphereDrawPositionYOffset;

        //Gizmos.DrawWireSphere(temp, inSightRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(temp, attackRadius);
    }

    void FaceTarget()
    {
        //DIrection from enemy to player
        Vector3 direction = (target.position - transform.position).normalized;

        //Calculate Quaternion rotation based on look direction
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        //Slerp between current rotation and lookrotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void SetAnimationSpeedPercent()
    {
        float speedPercent = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
    }

    public virtual void Attack()
    {
        if (hasAttacked == false)
        {
            hasAttacked = true;

            animator.SetTrigger("attackTrigger");

            //Reset attack cool down
            Invoke(nameof(ResetHasAttacked), 1f / attackSpeed);
        }
    }

    void ResetHasAttacked()
    {
        hasAttacked = false;
    }
}
