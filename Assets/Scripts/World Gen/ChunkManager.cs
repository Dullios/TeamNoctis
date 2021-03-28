using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChunkManager : Singleton<ChunkManager>
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
    public Dictionary<Vector2, BlockSpawner> chunkDict;
    public int loadDistance = 1;

    private void Start()
    {
        m_renderer = GetComponent<Renderer>();

        if (randomizeOffset)
        {
            perlinOffset = new Vector2(Random.Range(0.0f, 99999.0f), Random.Range(0.0f, 99999.0f));
        }

        perlinTexture = GenerateTexture();
        m_renderer.material.mainTexture = perlinTexture;
        chunkDict = new Dictionary<Vector2, BlockSpawner>();
        
        
        int range = loadDistance * 2 + 1;

        int inverse = loadDistance * -1;
        for (int x = inverse; x < range + inverse; x++)
        {
            for (int y = inverse; y < range + inverse; y++)
            {
                InstantiateChunk(x, y);
            }
        }
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

    public void InstantiateChunk(int x, int y)
    {
        int xPos = x * perlinStepSizeX;
        int yPos = y * perlinStepSizeY;

        GameObject tempChunk = Instantiate(chunkPrefab, new Vector3(xPos, 0, yPos), Quaternion.identity, transform);
        BlockSpawner tempSpawner = tempChunk.GetComponent<BlockSpawner>();
        tempSpawner.chunkPos = new Vector2(x, y);
        chunkDict.Add(tempSpawner.chunkPos, tempSpawner);
    }
}
