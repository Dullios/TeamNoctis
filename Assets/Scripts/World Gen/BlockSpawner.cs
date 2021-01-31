using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private Renderer m_renderer;
    private Texture2D perlinTexture;

    [Header("Perlin Texture")]
    public int width = 256;
    public int height = 256;

    public float scale = 20.0f;

    public bool randomizeOffset = false;
    public Vector2 perlinOffset;

    [Header("Terrain")]
    public GameObject surfaceCube;
    public GameObject fillerCube;

    public int perlinStepSizeX;
    public int perlinStepSizeY;

    public int terrainHeightMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();

        if(randomizeOffset)
        {
            perlinOffset = new Vector2(Random.Range(0.0f, 99999.0f), Random.Range(0.0f, 99999.0f));
        }

        perlinTexture = GenerateTexture();
        m_renderer.material.mainTexture = perlinTexture;

        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
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

    private void GenerateTerrain()
    {
        for(int x = 0; x < perlinStepSizeX; x++)
        {
            for(int y = 0; y < perlinStepSizeY; y++)
            {
                GameObject cubeTemp = Instantiate(surfaceCube, new Vector3(x, SampleStepped(x, y) * terrainHeightMultiplier, y) + transform.position, Quaternion.identity, transform);

                cubeTemp.transform.position = new Vector3(cubeTemp.transform.position.x, Mathf.CeilToInt(cubeTemp.transform.position.y), cubeTemp.transform.position.z);
                Vector3 cubePos = cubeTemp.transform.position;

                if (cubePos.y > 0)
                {
                    for(int i = (int)cubePos.y - 1; i >= 0; i--)
                    {
                        Instantiate(fillerCube, new Vector3(cubePos.x, i, cubePos.z), Quaternion.identity, cubeTemp.transform);
                    }
                }
            }
        }
    }

    public float SampleStepped(int x, int y)
    {
        int gridStepSizeX = width / perlinStepSizeX;
        int gridStepSizeY = height / perlinStepSizeY;

        float sampledFloat = perlinTexture.GetPixel((Mathf.FloorToInt(x * gridStepSizeX)), (Mathf.FloorToInt(y * gridStepSizeY))).grayscale;

        return sampledFloat;
    }
}
