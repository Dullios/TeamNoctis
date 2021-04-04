using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : TowerController
{
    new void Start()
    {
        base.Start();
        //Override Tower Behaviours... this is a wall
    }

    new void Update()
    {
        //Override Tower Behaviours... this is a wall
    }
    protected new void OnTriggerEnter(Collider other)
    {
        //Override Tower Behaviours... this is a wall
    }

    //Remove dead or out of range enemies
    protected new void OnTriggerExit(Collider other)
    {
        //Override Tower Behaviours... this is a wall
    }

    protected new void OnDestroy()
    {
        //Override Tower Behaviours... this is a wall
    }

}
