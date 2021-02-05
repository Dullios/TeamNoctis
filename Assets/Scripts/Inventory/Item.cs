using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class Item : ScriptableObject
{
    public int itemID = 0;
    public string itemName = "Item";
    public Sprite itemImage = null;
    public Material collectableObjectMaterial;
}
