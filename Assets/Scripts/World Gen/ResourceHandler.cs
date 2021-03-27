using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    [Header("Resources")]
    public GameObject[] trees;
    public GameObject[] ores;

    public GameObject ReturnResource()
    {
        int rand = Random.Range(0, 101);

        if(rand < 50)
        {
            return trees[Random.Range(0, trees.Length)];
        }
        else
        {
            return ores[Random.Range(0, ores.Length)];
        }
    }
}
