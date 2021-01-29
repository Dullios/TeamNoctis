using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Header("Positioning")]
    [Tooltip("Blocks parallel to the x-axis")]
    public int length;
    [Tooltip("Blocks parallel to the z-axis")]
    public int width;
    [Tooltip("Blocks parallel to the y-axis")]
    public int height;
    [Tooltip("Bottom layer y value")]
    public float depth;

    [Header("Block Details")]
    public GameObject block;
    public float blockOffset;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < length; x++)
        {
            for (int z = 0; z < width; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = new Vector3(blockOffset * x, depth + blockOffset * y, blockOffset * z);
                    GameObject.Instantiate(block, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
