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

    public void RepositionChunk()
    {
        //perlinStepSizeX = perlinStepX;
        //perlinStepSizeY = perlinStepY;

        //topLayer = new GameObject[perlinStepSizeX, perlinStepSizeY];

        Vector2 offset = ChunkManager.instance.perlinOffset;
        perlinOffset = new Vector2(offset.x + (chunkPos.x * perlinStepSizeX), offset.y + (chunkPos.y * perlinStepSizeY));

        GenerateTerrain();
        if (ChunkManager.instance.chunkDict.ContainsKey(chunkPos))
            meshFilter.sharedMesh = ChunkManager.instance.chunkDict[chunkPos];
        else
        {
            SpawnResources();
            StartCoroutine(CreateCombinedMesh());
        }
        StartCoroutine(GenerateNavMesh());
    }

    public void RecycleDirt()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).CompareTag("Block"))
                continue;

            blockPool.ReturnBlock(transform.GetChild(i).gameObject);
        }
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
                        filler.transform.position = new Vector3(pos.x, pos.y - i, pos.z);
                    }
                }
            }
        }
    }

    public float SampleStepped(int x, int y)
    {
        int gridStepSizeX = ChunkManager.instance.width / perlinStepSizeX;
        int gridStepSizeY = ChunkManager.instance.height / perlinStepSizeY;

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

            GameObject resource = Instantiate(resourceHandler.ReturnResource(), pos, Quaternion.identity, transform);
            resource.transform.SetParent(ChunkManager.instance.transform);
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

                //if (meshRenderers[i] == meshRenderer)
                //    continue;
                //meshRenderers[i].enabled = false;
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

        ChunkManager.instance.chunkDict.Add(chunkPos, finalMesh);

        transform.position = oldPos;
    }

    private IEnumerator GenerateNavMesh()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
        
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int range = ChunkManager.instance.loadDistance + 1;

            //for(int x = (int)chunkPos.x - range; x <= chunkPos.x + range; x++)
            //{
            //    for(int y = (int)chunkPos.y -range; y <= chunkPos.y + range; y++)
            //    {
            //        Vector2 index = new Vector2(x, y);

            //        if (Vector2.Distance(chunkPos, index) >= range)
            //        {
            //            if (ChunkManager.instance.chunkDict.ContainsKey(index))
            //                ChunkManager.instance.chunkDict[index].gameObject.SetActive(false);
            //        }
            //        else
            //        {
            //            if (ChunkManager.instance.chunkDict.ContainsKey(index))
            //                ChunkManager.instance.chunkDict[index].gameObject.SetActive(true);
            //            else
            //                ChunkManager.instance.InstantiateChunk((int)index.x, (int)index.y);
            //        }
            //    }
            //}

            Vector2 difference = new Vector2(Mathf.Abs(chunkPos.x - other.GetComponent<PlayerController>().gridLocation.x),
                Mathf.Abs(chunkPos.y - other.GetComponent<PlayerController>().gridLocation.y));

            foreach (GameObject chunk in ChunkManager.instance.chunkList)
            {
                BlockSpawner bs = chunk.GetComponent<BlockSpawner>();

                if (difference.x != 0)
                {
                    if(bs.chunkPos.x == chunkPos.x - range)
                    {
                        chunk.transform.position = new Vector3((chunkPos.x + (range - 1)) * perlinStepSizeX, 0, chunk.transform.position.y);
                        bs.chunkPos.x = difference.x + range - 1;
                    }
                    else if(bs.chunkPos.x == chunkPos.x + range)
                    {
                        chunk.transform.position = new Vector3((chunkPos.x - (range - 1)) * perlinStepSizeX, 0, chunk.transform.position.y);
                        bs.chunkPos.x = difference.x - range - 1;
                    }

                    bs.RecycleDirt();
                    bs.RepositionChunk();
                }
                else if (difference.y != 0)
                {
                    if (bs.chunkPos.y == chunkPos.y - range)
                    {
                        chunk.transform.position = new Vector3(chunk.transform.position.x, 0, (chunkPos.y + (range - 1)) * perlinStepSizeY);
                        bs.chunkPos.y = difference.y + range - 1;
                    }
                    else if (bs.chunkPos.y == chunkPos.y + range)
                    {
                        chunk.transform.position = new Vector3(chunk.transform.position.x, 0, (chunkPos.y - (range - 1)) * perlinStepSizeY);
                        bs.chunkPos.y = difference.y - range - 1;
                    }

                    bs.RecycleDirt();
                    bs.RepositionChunk();
                }
            }

            StartCoroutine(BuildSkyNavMesh());
        }
    }

    private IEnumerator BuildSkyNavMesh()
    {
        ChunkManager.instance.GetComponent<NavMeshSurface>().BuildNavMesh();

        yield return null;
    }
}
