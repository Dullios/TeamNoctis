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

    public int terrainHeightMultiplier;

    [Header("Resources")]
    public GameObject resourceBlock1;
    public Item wood;
    public GameObject resourceBlock2;
    public Item steel;


    [Header("Chunk Values")]
    public Vector2 chunkPos;

    [Header("Materials")]
    public Material[] blockMat;

    // Components
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        // Grab values from ChunkManager Instance
        width = ChunkManager.Instance.width;
        height = ChunkManager.Instance.height;
        perlinStepSizeX = ChunkManager.Instance.perlinStepSizeX;
        perlinStepSizeY = ChunkManager.Instance.perlinStepSizeY;

        //tempSpawner.perlinOffset = new Vector2(perlinOffset.x + (x * perlinStepSizeX), perlinOffset.y + (y * perlinStepSizeY));
        Vector2 offset = ChunkManager.Instance.perlinOffset;
        perlinOffset = new Vector2(offset.x + (chunkPos.x * perlinStepSizeX), offset.y + (chunkPos.y * perlinStepSizeY));

        GenerateTerrain();
        CreateCombinedMesh();
        GenerateNavMesh();
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

    //private void CreateCombinedMesh()
    //{
    //    Vector3 oldPos = transform.position;
    //    transform.position = Vector3.zero;

    //    meshRenderer.materials = blockMat;

    //    MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
    //    MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
    //    CombineInstance[] combine = new CombineInstance[meshFilters.Length];

    //    for (int i = 0; i < meshFilters.Length; i++)
    //    {
    //        if (meshFilters[i] == meshFilter)
    //            continue;

    //        combine[i].mesh = meshFilters[i].sharedMesh;
    //        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    //        combine[i].subMeshIndex = 0;

    //        meshFilters[i].GetComponent<MeshRenderer>().enabled = false;
    //    }

    //    meshFilter.mesh = new Mesh();
    //    meshFilter.mesh.CombineMeshes(combine, true);

    //    transform.position = oldPos;
    //}

    private void CreateCombinedMesh()
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
                    ci.mesh = meshFilters[i].sharedMesh;
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

    private void GenerateNavMesh()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            for(int x = (int)chunkPos.x - 2; x <= chunkPos.x + 2; x++)
            {
                for(int y = (int)chunkPos.y -2; y <= chunkPos.y + 2; y++)
                {
                    Vector2 index = new Vector2(x, y);

                    if (Vector2.Distance(chunkPos, index) >= 2)
                    {
                        if (ChunkManager.Instance.chunkDict.ContainsKey(index))
                            ChunkManager.Instance.chunkDict[index].gameObject.SetActive(false);
                    }
                    else
                    {
                        if (ChunkManager.Instance.chunkDict.ContainsKey(index))
                            ChunkManager.Instance.chunkDict[index].gameObject.SetActive(true);
                        else
                            ChunkManager.Instance.InstantiateChunk((int)index.x, (int)index.y);
                    }
                }
            }

            ChunkManager.Instance.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}
