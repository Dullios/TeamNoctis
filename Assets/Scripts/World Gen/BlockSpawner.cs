using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Video References:
 * 
 *  Brackeys:
 *   https://www.youtube.com/watch?v=bG0uEXV6aHQ
 * 
 *  Matt MirrorFish:
 *   https://www.youtube.com/watch?v=sUDPfC1nH_E
 */

public class BlockSpawner : MonoBehaviour
{
    // Values from Chunk Manager
    private int width;
    private int height;

    public Vector2 perlinOffset;

    private int perlinStepSizeX;
    private int perlinStepSizeY;

    [Header("Terrain")]
    public GameObject surfaceCube;
    public GameObject fillerCube;

    [Header("Resources")]
    public GameObject resourceBlock1;
    public Item wood;
    public GameObject resourceBlock2;
    public Item steel;

    public int terrainHeightMultiplier;

    [Header("Chunk Values")]
    public Vector2 chunkPos;

    // Start is called before the first frame update
    void Start()
    {
        // Grab values from ChunkManager Instance
        width = ChunkManager.Instance.width;
        height = ChunkManager.Instance.height;
        perlinStepSizeX = ChunkManager.Instance.perlinStepSizeX;
        perlinStepSizeY = ChunkManager.Instance.perlinStepSizeY;

        //tempSpawner.perlinOffset = new Vector2(perlinOffset.x + (x * perlinStepSizeX), perlinOffset.y + (y * perlinStepSizeY));
        Vector2 offset = ChunkManager.Instance.perlinOffset;
        perlinOffset = new Vector2(offset.x + (chunkPos.x * perlinStepSizeX), offset.y + (chunkPos.y * perlinStepSizeY));

        GenerateTerrain();

        //Let chunk manager know that finished spawning block
        ChunkManager.Instance.NotifyFinishedBlockSpanwer();
    }

    private void GenerateTerrain()
    {
        for(int x = 0; x < perlinStepSizeX; x++)
        {
            for(int y = 0; y < perlinStepSizeY; y++)
            {
                GameObject cubeTemp;
                int rand = Random.Range(0, 100);
                if (rand < 2)
                {
                    if (rand < 1)
                    {
                        cubeTemp = Instantiate(resourceBlock1, new Vector3(x, SampleStepped(x, y) * terrainHeightMultiplier + 1, y) + transform.position, Quaternion.identity, transform);
                        cubeTemp.GetComponent<CollectableObject>().item = wood;
                    }
                    else
                    {
                        cubeTemp = Instantiate(resourceBlock2, new Vector3(x, SampleStepped(x, y) * terrainHeightMultiplier + 1, y) + transform.position, Quaternion.identity, transform);
                        cubeTemp.GetComponent<CollectableObject>().item = steel;
                    }
                }
                else
                {
                    cubeTemp = Instantiate(surfaceCube, new Vector3(x, SampleStepped(x, y) * terrainHeightMultiplier, y) + transform.position, Quaternion.identity, transform);
                }

                cubeTemp.transform.position = new Vector3(cubeTemp.transform.position.x, Mathf.CeilToInt(cubeTemp.transform.position.y), cubeTemp.transform.position.z);
                Vector3 cubePos = cubeTemp.transform.position;

                if (cubePos.y > 0)
                {
                    for(int i = (int)cubePos.y - 1; i >= 1; i--)
                    {
                        GameObject filler = Instantiate(fillerCube, new Vector3(cubePos.x, i, cubePos.z), Quaternion.identity, transform);

                        if (i == 1) // Reduce number of collider checks
                            Destroy(filler.GetComponent<BoxCollider>());
                    }
                }
            }
        }
    }

    public float SampleStepped(int x, int y)
    {
        int gridStepSizeX = width / perlinStepSizeX;
        int gridStepSizeY = height / perlinStepSizeY;

        //float sampledFloat = ChunkManager.Instance.perlinTexture.GetPixel((Mathf.FloorToInt((x * gridStepSizeX) + offset.x)),
        //    (Mathf.FloorToInt((y * gridStepSizeY) + offset.y))).grayscale;

        //float sampledFloat = ChunkManager.Instance.perlinTexture.GetPixel((int)((x * gridStepSizeX) + perlinOffset.x),
        //    (int)((y * gridStepSizeY) + perlinOffset.y)).grayscale;
        float sampledFloat = ChunkManager.Instance.perlinTexture.GetPixel((int)(x + perlinOffset.x),
            (int)(y + perlinOffset.y)).grayscale;

        return sampledFloat;
    }
}
