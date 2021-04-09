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
    public int cubeCount;

    public int terrainHeightMultiplier;

    public GameObject surfaceCube;
    public GameObject fillerCube;

    public GameObject[,] topLayer;

    [Header("Chunk Values")]
    public Vector2 chunkPos;

    [Header("Resource Values")]
    public int maxResources;

    [Header("Materials")]
    public Mesh pieceMesh;
    public Material[] blockMat;

    // Components
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private ResourceHandler resourceHandler;
    private BlockPool blockPool;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        resourceHandler = GetComponent<ResourceHandler>();
        blockPool = GetComponent<BlockPool>();

        perlinStepSizeX = ChunkManager.instance.perlinStepSizeX;
        perlinStepSizeY = ChunkManager.instance.perlinStepSizeY;

        topLayer = new GameObject[perlinStepSizeX, perlinStepSizeY];

        for(int x = 0; x < perlinStepSizeX; x++)
        {
            for(int y = 0; y < perlinStepSizeY; y++)
            {
                topLayer[x, y] = Instantiate(surfaceCube, new Vector3(x, 0, y), Quaternion.identity, transform);
            }
        }    
    }

    public void RepositionChunk(int _width, int _height, int perlinStepX, int perlinStepY)
    {
        width = _width;
        height = _height;
        //perlinStepSizeX = perlinStepX;
        //perlinStepSizeY = perlinStepY;

        //topLayer = new GameObject[perlinStepSizeX, perlinStepSizeY];

        Vector2 offset = ChunkManager.instance.perlinOffset;
        perlinOffset = new Vector2(offset.x + (chunkPos.x * perlinStepSizeX), offset.y + (chunkPos.y * perlinStepSizeY));

        GenerateTerrain();
        StartCoroutine(CreateCombinedMesh());
        SpawnResources();
        StartCoroutine(GenerateNavMesh());
    }

    private void GenerateTerrain()
    {
        for(int x = 0; x < perlinStepSizeX; x++)
        {
            for(int y = 0; y < perlinStepSizeY; y++)
            {
                cubeCount++;

                //GameObject cubeTemp = Instantiate(surfaceCube, new Vector3(x, SampleStepped(x, y) * terrainHeightMultiplier, y) + transform.position, Quaternion.identity, transform);
                //topLayer[x, y] = cubeTemp;
                
                //cubeTemp.transform.position = new Vector3(cubeTemp.transform.position.x, Mathf.CeilToInt(cubeTemp.transform.position.y), cubeTemp.transform.position.z);
                //Vector3 cubePos = cubeTemp.transform.position;

                Vector3 pos = topLayer[x, y].transform.position;
                pos.y = Mathf.CeilToInt(SampleStepped(x, y) * terrainHeightMultiplier);
                topLayer[x, y].transform.position = pos;

                if (pos.y > 0)
                {
                    //for(int i = (int)cubePos.y - 1; i >= 1; i--)
                    for(int i = 1; i <= 2; i++)
                    {
                        if (pos.y - i < 1)
                            continue;

                        cubeCount++;

                        //GameObject filler = Instantiate(fillerCube, new Vector3(pos.x, pos.y - i, pos.z), Quaternion.identity, transform);
                        GameObject filler = blockPool.TakeBlock();
                        filler.transform.position = new Vector3(pos.x, pos.y - 1, pos.z);

                        //if (i == 1) // Reduce number of collider checks
                        //    Destroy(filler.GetComponent<BoxCollider>());
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
        float sampledFloat = ChunkManager.instance.perlinTexture.GetPixel((int)(x + perlinOffset.x),
            (int)(y + perlinOffset.y)).grayscale;

        return sampledFloat;
    }

    private void SpawnResources()
    {
        for (int i = 0; i < maxResources; i++)
        {
            int randX = Random.Range(0, perlinStepSizeX);
            int randY = Random.Range(0, perlinStepSizeY);

            Vector3 pos = topLayer[randX, randY].transform.position;
            pos.y += 1;

            Instantiate(resourceHandler.ReturnResource(), pos, Quaternion.identity, gameObject.transform);
        }
    }

    private IEnumerator CreateCombinedMesh()
    {
        Vector3 oldPos = transform.position;
        transform.position = Vector3.zero;

        meshRenderer.materials = blockMat;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(false);
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(false);
        List<Material> materialList = new List<Material>();

        foreach(MeshRenderer renderer in meshRenderers)
        {
            if (renderer == meshRenderer)
                continue;

            Material[] localMats = renderer.sharedMaterials;
            foreach(Material mat in localMats)
            {
                if (!materialList.Contains(mat))
                    materialList.Add(mat);
            }
        }

        yield return new WaitForEndOfFrame();

        List<Mesh> submeshes = new List<Mesh>();
        foreach(Material mat in materialList)
        {
            List<CombineInstance> combiner = new List<CombineInstance>();

            for (int i = 0; i < meshFilters.Length; i++)
            {
                Material[] localMat = meshRenderers[i].sharedMaterials;
                for(int matIndex = 0; matIndex < localMat.Length; matIndex++)
                {
                    if (localMat[matIndex] != mat)
                        continue;

                    CombineInstance ci = new CombineInstance();
                    //ci.mesh = meshFilters[i].sharedMesh;
                    ci.mesh = pieceMesh;
                    ci.subMeshIndex = matIndex;
                    ci.transform = meshFilters[i].transform.localToWorldMatrix;
                    combiner.Add(ci);
                }

                if (meshRenderers[i] == meshRenderer)
                    continue;
                meshRenderers[i].enabled = false;
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combiner.ToArray(), true);
            submeshes.Add(mesh);
        }

        List<CombineInstance> finalCombiners = new List<CombineInstance>();
        foreach(Mesh mesh in submeshes)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = 0;
            ci.transform = Matrix4x4.identity;
            finalCombiners.Add(ci);
        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
        meshFilter.sharedMesh = finalMesh;

        transform.position = oldPos;
    }

    private IEnumerator GenerateNavMesh()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
        
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            int range = ChunkManager.instance.loadDistance + 1;

            for(int x = (int)chunkPos.x - range; x <= chunkPos.x + range; x++)
            {
                for(int y = (int)chunkPos.y -range; y <= chunkPos.y + range; y++)
                {
                    Vector2 index = new Vector2(x, y);

                    if (Vector2.Distance(chunkPos, index) >= range)
                    {
                        if (ChunkManager.instance.chunkDict.ContainsKey(index))
                            ChunkManager.instance.chunkDict[index].gameObject.SetActive(false);
                    }
                    else
                    {
                        if (ChunkManager.instance.chunkDict.ContainsKey(index))
                            ChunkManager.instance.chunkDict[index].gameObject.SetActive(true);
                        else
                            ChunkManager.instance.InstantiateChunk((int)index.x, (int)index.y);
                    }
                }
            }

            //ChunkManager.Instance.GetComponent<NavMeshSurface>().BuildNavMesh();
            StartCoroutine(BuildSkyNavMesh());
        }
    }

    private IEnumerator BuildSkyNavMesh()
    {
        ChunkManager.instance.GetComponent<NavMeshSurface>().BuildNavMesh();

        yield return null;
    }
}
