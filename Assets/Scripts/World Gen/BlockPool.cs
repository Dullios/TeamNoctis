using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    public GameObject fillerPrefab;
    public int queueCount;
    private Queue<GameObject> blockQueue = new Queue<GameObject>();

    private void Awake()
    {
        for(int i = 0; i < queueCount; i++)
        {
            ReturnBlock(Instantiate(fillerPrefab, transform));
        }
    }

    public GameObject TakeBlock()
    {
        GameObject block = blockQueue.Dequeue();
        block.SetActive(true);
        return block;
    }
    
    public void ReturnBlock(GameObject block)
    {
        block.SetActive(false);
        blockQueue.Enqueue(block);
    }
}
