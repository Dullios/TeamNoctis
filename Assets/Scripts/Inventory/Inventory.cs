using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    //Comp
    RectTransform rectTransform;
    Canvas inventoryCanvas;

    InventorySlot[,] inventory; //2d grid for inventory

    [Header("Inventory settings")]
    [SerializeField] int numOfSlotCol = 5; //number of colume of inventory
    [SerializeField] int numOfSlotRow = 5; //number of row of inventory
    [SerializeField] int colSpacing = 1; //space between col tile
    [SerializeField] int rowSpacing = 1; //space between row tile
    [SerializeField] int topPadding = 10; //top padding
    [SerializeField] int leftPadding = 10; //left padding
    [SerializeField] int rightPadding = 10; //right padding
    [SerializeField] int bottomPadding = 10; //bottom padding

    [Header("SLot settings")]
    [SerializeField] GameObject slotPrefab; //slot gameobject

    protected override void Awake()
    {
        base.Awake();

        rectTransform = GetComponent<RectTransform>(); //rect transform of inventory background
        inventoryCanvas = GetComponentInParent<Canvas>();

        GenerateInventory();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryCanvas.enabled == false)
            {
                inventoryCanvas.enabled = true;
            }
            else
            {
                inventoryCanvas.enabled = false;
            }
        }
    }

    void GenerateInventory()
    {
        Debug.Log(rectTransform.rect.width); //actual width of rect
        Debug.Log(rectTransform.rect.height); //actual height of rect

        int numOfColSpace = numOfSlotCol - 1; //get number of col space between col slot
        int numOfRowSpace = numOfSlotRow - 1; //get number of row space between row slot

        float calculatedInventoryWidth = rectTransform.rect.width - leftPadding - rightPadding; //calculate inventory's width based on padding
        float calculatedInventoryHeight = rectTransform.rect.height - topPadding - bottomPadding; //calculate inventory's height based on padding


        float slotWidth = (calculatedInventoryWidth - numOfColSpace * colSpacing) / (float)numOfSlotCol; //Get slot's width based on space and with of inventory
        float slotHeight = (calculatedInventoryHeight - numOfRowSpace * rowSpacing) / (float)numOfSlotRow; //Get slot's height based on space and with of inventory

        //create new inventory
        inventory = new InventorySlot[numOfSlotRow, numOfSlotCol];

        //nested for loop to generate slots to fill inventory
        for (int row = 0; row < numOfSlotRow; ++row)
        {
            for (int col = 0; col < numOfSlotCol; ++col)
            {
                //create one slot
                GameObject slotGameObject = Instantiate(slotPrefab, transform);

                //Save it to inventory as slot
                inventory[row, col] = slotGameObject.GetComponent<InventorySlot>();
                if (inventory[row, col] == null)
                {
                    Debug.LogWarning("Inventory: SlotPrefab doesn't have InventorySlot component");
                    return;
                }

                //Set slot idx
                inventory[row, col].inventoryRowIdx = row;
                inventory[row, col].inventoryColIdx = col;

                //Set the position of slot
                inventory[row, col].GetComponent<RectTransform>().anchoredPosition = new Vector2((col * slotWidth) + (col * rowSpacing) + (leftPadding),
                                                                                                -((row * slotHeight) + (row * colSpacing) + (topPadding)));

                //Set size of slot
                inventory[row, col].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotHeight);
                inventory[row, col].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotWidth);

                //Adjust item image size
                inventory[row, col].AdjustItemImageAndCountSize();
                inventory[row, col].SetItemCount(0);
            }
        }
    }

    public bool AddItem(Item newItem, int newItemCount)
    {
        int emptyRow = -1; //save empty row
        int emptyCol = -1; //save empty Col

        for (int row = 0; row < numOfSlotRow; ++row)
        {
            for (int col = 0; col < numOfSlotCol; ++col)
            {
                //If slot's item is empty
                if(inventory[row, col].item == null)
                {
                    //Save empty slot's row and col
                    if (emptyRow == -1 && emptyCol == -1)
                    {
                        emptyRow = row;
                        emptyCol = col;
                    }
                }
                else
                {
                    //If it's the same item,
                    if(inventory[row, col].item.itemID == newItem.itemID)
                    {
                        //Stack up item
                        inventory[row, col].SetItemCount(inventory[row, col].itemCount + newItemCount);
                        return true;
                    }
                }
            }
        }

        //If we wasn't able to find the same item in the inventory and has empty slot, add new item
        if(emptyRow != -1 && emptyCol != -1)
        {
            inventory[emptyRow, emptyCol].SetItem(newItem, newItemCount);
            return true;
        }

        return false;
    }
}
