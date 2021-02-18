using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Bake navmesh
        //GetComponent<NavMeshSurface>().BuildNavMesh();
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

        for(int x = 0; x < range; x++)
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
}
