using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyTable : MonoBehaviour
{

    public Item requiredItem;
    public Item rewardItem;
    
    public void AddGoldToInventory()
    {
        if(!Inventory.instance.CheckItem(requiredItem, 1)) return;
        Inventory.instance.RemoveItem(requiredItem, 1);
        Inventory.instance.AddItem(rewardItem, 1);
    }
}
