using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    RectTransform rectTransform; //slot's rectTransform

    [Header("ItemImage settings")]
    public RectTransform itemImageRectTransform; //Item image from children
    public float itemImageWidthPercent = 0.8f; //percent of slot's width
    public float itemImageHeightPercent = 0.8f; //percent of slot's width

    [Header("ItemCount settings")]
    public RectTransform itemCountRectTransform; //Item count from children
    public float itemCountWidthPercent = 0.2f; //percent of slot's width
    public float itemCountHeightPercent = 0.2f; //percent of slot's width


    public int inventoryColIdx = 0; //col index for inventory
    public int inventoryRowIdx = 0; //row index for inventory

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustItemImageAndCountSize()
    {
        float slotWidth = rectTransform.rect.width;
        float slotHeight = rectTransform.rect.height;

        //Calculate item image width and height
        float itemImageWidth = slotWidth * itemImageWidthPercent;
        float itemImageHeight = slotHeight * itemImageHeightPercent;
        itemImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemImageWidth);
        itemImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemImageHeight);

        //Calculate item count width and height
        float itemCountWidth = slotWidth * itemCountWidthPercent;
        float itemCountHeight = slotHeight * itemCountHeightPercent;
        itemCountRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemCountWidth);
        itemCountRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, itemCountHeight);
    }
}
