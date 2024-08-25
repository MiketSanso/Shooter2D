using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainCell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int x, y;
    public Sprite greenCell, redCell, whiteCell;
    public Image image;

    public bool isFree;
    public Inventory inventory;

    private int sizeDraggenItem;

    public void Start()
    {
        image = GetComponent<Image>();
    }

    public void Update()
    {
        if (PlayerPrefs.GetInt("sizeDraggenItem") == 0)
           image.sprite = whiteCell;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sizeDraggenItem = PlayerPrefs.GetInt("sizeDraggenItem");

        if (sizeDraggenItem != 0)
        {
            Sprite colorCell = CheckCellFree(this, new Vector2Int(sizeDraggenItem/10, sizeDraggenItem-sizeDraggenItem/10*10)) ? greenCell : redCell;

            for (int y1 = y; y1 < y + (sizeDraggenItem - sizeDraggenItem / 10 * 10); y1++)
            {
                for (int x1 = x; x1 < x + sizeDraggenItem / 10; x1++)
                {
                    if (x1 <= inventory.cells.GetLength(0) - 1 && y1 <= inventory.cells.GetLength(1) - 1)
                        inventory.cells[x1, y1].image.sprite = colorCell;
                }
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (sizeDraggenItem != 0)
        {
            for (int y1 = y; y1 < y + (sizeDraggenItem - sizeDraggenItem / 10 * 10); y1++)
            {
                for (int x1 = x; x1 < x + sizeDraggenItem / 10; x1++)
                {
                    if (x1 <= inventory.cells.GetLength(0) - 1 && y1 <= inventory.cells.GetLength(1) - 1)
                        inventory.cells[x1, y1].image.sprite = whiteCell;
                }
            }
        }
    } 

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem dragItem = eventData.pointerDrag.GetComponent<ItemConsumables>();
        if (dragItem == null)
            dragItem = eventData.pointerDrag.GetComponent<ItemAmmunition>();

        if (CheckCellFree(this, dragItem.size))
        {
            dragItem.prevMainCell = this;
            dragItem.prevAmmunitionCell = null;
        }
        else
            dragItem.SetPosition(dragItem, dragItem.prevMainCell ? dragItem.prevMainCell : null, dragItem.prevAmmunitionCell ? dragItem.prevAmmunitionCell : null);
    }


    public bool CheckCellFree(MainCell _cell, Vector2Int size)
    {
        for (int y = _cell.y; y < _cell.y + size.y; y++)
        {
            for (int x = _cell.x; x < _cell.x + size.x; x++)
            {
                if (_cell.x + size.x > _cell.inventory.sizeX || _cell.y + size.y > _cell.inventory.sizeY)
                    return false;
                if (!_cell.inventory.cells[x, y].isFree)
                    return false;
            }
        }
        return true;
    }

    public bool CheckCellCanBeCastled(MainCell _cell, Vector2Int size)
    {
        for (int y = _cell.y; y < _cell.y + size.y; y++)
        {
            for (int x = _cell.x; x < _cell.x + size.x; x++)
            {
                if (_cell.x + size.x > _cell.inventory.sizeX || _cell.y + size.y > _cell.inventory.sizeY)
                    return false;
            }
        }
        return true;
    }

    public void CellOccupation(MainCell _cell, bool _isOrdered, Vector2Int size)
    {

        for (int y = _cell.y; y < _cell.y + size.y; y++)
            for (int x = _cell.x; x < _cell.x + size.x; x++)
                _cell.inventory.cells[x, y].isFree = _isOrdered;
    }
}

