using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChunkManager : MonoBehaviour
{
    private Renderer m_renderer;
    public Texture2D perlinTexture;

    [Header("Perlin Texture")]
    public int width = 256;
    public int height = 256;

    public float scale = 5.0f;

    public bool randomizeOffset = false;
    public Vector2 perlinOffset;
    
    [Header("Chunk Properties")]
    public int perlinStepSizeX;
    public int perlinStepSizeY;

    [Header("Chunk Loading Properties")]
    public GameObject chunkPrefab;
    private GameObject[,] chunkGrid;
    public int loadDistance = 2;

    public static ChunkManager Instance;

    int numberOfBlockSpawner = 0; //total number of block spawner
    int generatedBlockSpawner = 0; //number of block spawner which finished generating blocks

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        m_renderer = GetComponent<Renderer>();

        if (randomizeOffset)
        {
            perlinOffset = new Vector2(Random.Range(0.0f, 99999.0f), Random.Range(0.0f, 99999.0f));
        }

        perlinTexture = GenerateTexture();
        m_renderer.material.mainTexture = perlinTexture;
    }

    private Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        return texture;
    }

    private Color CalculateColor(int x, int y)
    {
        // convert pixel to perlin coords
        float xCoord = (float)x / width * scale + perlinOffset.x;
        float yCoord = (float)y / height * scale + perlinOffset.y;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }


    private void Start()
    {
        int range = loadDistance * 2 + 1;

        chunkGrid = new GameObject[range, range];

        numberOfBlockSpawner = range * range; //save total number of block spawner

        for (int x = 0; x < range; x++)
        {
            int xPos = x * perlinStepSizeX;

            for(int y = 0; y < range; y++)
            {
                int yPos = y * perlinStepSizeY;

                GameObject tempChunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity, transform);
                BlockSpawner tempSpawner = tempChunk.GetComponent<BlockSpawner>();
                tempSpawner.chunkPos = new Vector2(x, y);
                //tempSpawner.perlinOffset = new Vector2(perlinOffset.x + (x * perlinStepSizeX), perlinOffset.y + (y * perlinStepSizeY));

                chunkGrid[x, y] = tempChunk;
            }
        }
    }

    //Will be called by block spawner that finished generating block
    public void NotifyFinishedBlockSpanwer()
    {
        ++generatedBlockSpawner; //increase count of finished block spawner

        //If all block spawner finished, generate nav mesh
        if(generatedBlockSpawner == numberOfBlockSpawner)
        {
            //Bake navmesh
            NavMeshSurface[] navMeshSurfaces = GetComponents<NavMeshSurface>();
            for (int i = 0; i < navMeshSurfaces.Length; ++i)
            {
                Debug.Log("Build Navmesh");
                navMeshSurfaces[i].BuildNavMesh();
            }
        }
    }
}