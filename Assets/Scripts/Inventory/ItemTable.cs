using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item table - have all item info
/// </summary>

[CreateAssetMenu(fileName = "ItemTable", menuName = "ScriptableObjects/ItemTable", order = 2)]
public class ItemTable : ScriptableObject
{
    [SerializeField] Item[] itemArr;


    public Item GetItem(int ID)
    {
        return itemArr[ID];
    }

    public void AssignItemIDs()
    {
        for (int i = 0; i < itemArr.Length; ++i)
        {
            itemArr[i].itemID = i;
        }
    }
}
