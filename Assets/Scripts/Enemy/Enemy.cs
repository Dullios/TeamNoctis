using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected const float locomotionAnimationSmoothTime = 0.1f; //interpolation time between two value

    //Comp
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rb;
    protected Stats stats;

    [SerializeField] protected Transform target;

    bool hasAttacked = false;

    [Header("Sounds")]
    public AudioSource attackSFX;

    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<Stats>();

        //Find player with tag
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
        {
            Debug.LogWarning("EnemyMove : Can't find player with tag(Player)");
        }

        //Set speed
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = stats.moveSpeed;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        //If we have target
        if (target != null)
        {
            //Distacne from to target
            Vector3 targetPosition = new Vector3(target.position.x, 0.0f, target.position.z);
            Vector3 myPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
            float distance = Vector3.Distance(myPosition, targetPosition);
            

            //Take account of position volume offset radius into distacne, see description of PositionVolumeOffset class
            PositionVolumeOffset pvoComp = target.gameObject.GetComponent<PositionVolumeOffset>();
            if (pvoComp != null)
            {
                distance -= pvoComp.offsetRadius;
            }

            //If target is closer than attack radius...
            if (distance <= stats.attackRadius)
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

    protected void FaceTarget()
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
            attackSFX.Play();

            //Get stats and damage
            Stats targetStats = target.GetComponent<Stats>();
            if(targetStats != null)
            {
                targetStats.HpModify(-stats.damage);
            }

            //Reset attack cool down
            Invoke(nameof(ResetHasAttacked), 1f / stats.attackSpeed);
        }
    }

    void ResetHasAttacked()
    {
        hasAttacked = false;
    }
}
