using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : Singleton<ChunkPool>
{
    public GameObject chunkPrefab;
    public int poolSize;

    private Queue<GameObject> chunkPool = new Queue<GameObject>();

    public void PopulatePool(int loadDistance)
    {
        loadDistance = loadDistance * 2 + 1;
        poolSize = loadDistance * loadDistance;

        for(int i = 0; i < poolSize; i++)
        {
            GameObject tempChunk = Instantiate(chunkPrefab, transform);
            tempChunk.SetActive(false);
            chunkPool.Enqueue(tempChunk);
        }
    }

    public GameObject TakeChunk()
    {
        GameObject tempChunk = chunkPool.Dequeue();
        tempChunk.SetActive(true);
        return tempChunk;
    }

    public void ReturnChunk(GameObject chunk)
    {
        // TODO: tell chunk to return all their blocks to blockPool

        chunk.SetActive(false);
        chunkPool.Enqueue(chunk);
    }
}
