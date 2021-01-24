using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public Item item = null; //item in collectable object
    public int itemCount = 1; //how much item?

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collected()
    {
        if (Inventory.HasInstance)
        {
            bool result = Inventory.instance.AddItem(item, itemCount);

            if(result == true)
            {
                Debug.Log("Added " + item.itemName);
            }
            else
            {
                Debug.Log("Couldn't added " + item.itemName);
            }
         
        }
        else
        {
            Debug.LogWarning("CollectableObject: Inventory singleton instance not exist!");
        }
        Destroy(gameObject);
    }
}
