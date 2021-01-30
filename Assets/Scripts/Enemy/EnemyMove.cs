using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    //Comp
    NavMeshAgent navMeshAgent;

    [SerializeField] Transform target;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        //Find player with tag
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if(target == null)
        {
            Debug.LogWarning("EnemyMove : Can't find player with tag(Player)");
        }
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(target.position);
    }
}
