using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InventoryItem : Item, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    public Canvas canvas;

    public MainCell prevMainCell;
    [HideInInspector] public AmmunitionCell prevAmmunitionCell;

    [HideInInspector] public float postIndex = 31.25f;

    public ItemSize itemSize;
    [HideInInspector] public Vector2Int size;

    public PlayerInventory plInv;


    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        GetSize();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
        PlayerPrefs.SetInt("sizeDraggenItem", size.x * 10 + size.y); //Dont work, if (x or y) > 9
        DeleteObject();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 difference = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        rectTransform.position = difference;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        PlayerPrefs.SetInt("sizeDraggenItem", 0);
    }
    public virtual void OnDrop(PointerEventData eventData)
    {
        SaveObject();
    }

    public Vector2Int GetSize()
    {
        string strItemSize = Convert.ToString(itemSize);
        char[] chars = strItemSize.ToCharArray();
        string strX = chars[1].ToString();
        string strY = chars[2].ToString();
        int x = Convert.ToInt32(strX);
        int y = Convert.ToInt32(strY);
        return size = new Vector2Int(x, y);
    }

    public virtual void SetPosition(InventoryItem _item, MainCell _mainCell, AmmunitionCell _ammunitionCell)
    {
        if (_mainCell)
        {
            _item.transform.SetParent(_mainCell.transform);
            var newPos = Vector3.zero;

            if (size.y > 1 || size.x > 1)
                for (int y = 1; y <= 6; y++)
                    for (int x = 1; x <= 6; x++)
                        if (x == size.x && y == size.y)
                        {
                            newPos.y -= postIndex * (y - 1);
                            newPos.x += postIndex * (x - 1);
                        }

            _item.transform.localPosition = newPos;

            if (_item.GetComponent<ItemAmmunition>() && Convert.ToString(_item.GetComponent<ItemAmmunition>().itemAmmunType) == "Gun")
                _item.transform.rotation = new Quaternion(0, 0, 0, 0);

            _mainCell.CellOccupation(_mainCell, false, size);
            _item.transform.SetParent(_item.prevMainCell.inventory.transform);
        }
    }

    public virtual void SaveObject()
    {
        SaveManager.saveMg.activeSave.idInventoryGuns.Add(id);

        if (prevMainCell != null)
        {
            SaveManager.saveMg.activeSave.xCellIndexGuns.Add(prevMainCell.x);
            SaveManager.saveMg.activeSave.yCellIndexGuns.Add(prevMainCell.y);
            SaveManager.saveMg.activeSave.isStayedInMainCell.Add(true);
            SaveManager.saveMg.Save();
        }
    }

    public virtual void DeleteObject()
    {
        if (prevMainCell != null)
        {
            int stepInLists = 0; //�� -1
            do
            {
                stepInLists++;
            }
            while (SaveManager.saveMg.activeSave.xCellIndexGuns[stepInLists] == prevMainCell.x && SaveManager.saveMg.activeSave.yCellIndexGuns[stepInLists] == prevMainCell.y);

            SaveManager.saveMg.activeSave.xCellIndexGuns.RemoveAt(stepInLists);
            SaveManager.saveMg.activeSave.yCellIndexGuns.RemoveAt(stepInLists);
            SaveManager.saveMg.activeSave.isStayedInMainCell.RemoveAt(stepInLists);
            SaveManager.saveMg.Save();
        }
    } 
}