using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rabbit : Enemy
{
    //Comp
    float jumpTime = 0.0f;

    float distanceToTarget = 0.0f;

    [Header("Attribute")]
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] float jumpCoolTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            FaceTarget();
        }


    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Move();

            Jump();
        }

        float speedPercent = rb.velocity.magnitude / stats.moveSpeed;
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
    }

    void Move()
    {
        //Distacne from to target
        distanceToTarget = Vector3.Distance(transform.position, target.position);
        //Take account of position volume offset radius into distacne, see description of PositionVolumeOffset class
        PositionVolumeOffset pvoComp = target.gameObject.GetComponent<PositionVolumeOffset>();
        if (pvoComp != null)
        {
            distanceToTarget -= pvoComp.offsetRadius;
        }

        //If target is closer than attack radius... attack
        if (distanceToTarget <= stats.attackRadius)
        {
            //Stop
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);

            Attack();
        }
        //move
        else
        {
            Vector3 rbVel = rb.velocity;
            Vector3 moveDir = (target.position - transform.position).normalized;

            rb.velocity = new Vector3(moveDir.x * stats.moveSpeed, rbVel.y, moveDir.z * stats.moveSpeed);
        }
        
    }

    void Jump()
    {
        //If distance if far than attack radius
        if (distanceToTarget > stats.attackRadius)
        {
            if (jumpTime <= 0.0f)
            {
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                jumpTime = jumpCoolTime;
            }
            jumpTime -= Time.deltaTime;
        }
    }
}
