//The problem is when character moving to position or target character(agent.SetDestination),
//the volume of target doesn't matter when goes to poisiton but it matters when goes to target(which has volume and position is at center of volume)
//Character collide with volume(collider) and can't reach destination(position which center of volume), or need to keep distance with attack range
//This class will define offset from center position to edge of volume
//When character wants to chase volumed object, use volumed object's position(center of volume) + PositionVolumeOffset

//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PositionVolumeOffset : MonoBehaviour
{
    [SerializeField]
    public float offsetRadius = 0;

    [SerializeField]
    float sphereDrawPositionYOffset = 2f;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 temp = transform.position;
        temp.y += sphereDrawPositionYOffset;

        Gizmos.DrawWireSphere(temp, offsetRadius);
    }
}
