using UnityEngine;
using UnityEngine.EventSystems;

public class AmmunitionCell : MonoBehaviour, IDropHandler
{
    [SerializeField] private AmmunType localCellAmmunType;
    [HideInInspector] public AmmunType cellAmmunType { get { return localCellAmmunType; }  }
    public int indexGunCell;

    [HideInInspector] public bool isFree;

    public void Start()
    {
        isFree = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventoryItem dragItem = eventData.pointerDrag.GetComponent<ItemConsumables>();
        if (dragItem == null)
            dragItem = eventData.pointerDrag.GetComponent<ItemAmmunition>();

        if (eventData.pointerDrag.GetComponent<ItemAmmunition>() && cellAmmunType == eventData.pointerDrag.GetComponent<ItemAmmunition>().itemAmmunType && (transform.childCount > 0 && dragItem.prevMainCell && dragItem.prevMainCell.CheckCellFree(dragItem.prevMainCell, transform.GetChild(0).GetComponent<InventoryItem>().size) || isFree || !dragItem.prevMainCell))
        {
            if (isFree)
                isFree = false;
            else
            {
                InventoryItem item = transform.GetChild(0).GetComponent<InventoryItem>();
                item.SetPosition(item, dragItem.prevMainCell ? dragItem.prevMainCell : null, dragItem.prevAmmunitionCell ? dragItem.prevAmmunitionCell : null);

                item.prevMainCell = dragItem.prevMainCell;
                item.prevAmmunitionCell = dragItem.prevAmmunitionCell;

                if (dragItem.prevMainCell)
                {
                    dragItem.prevMainCell.CellOccupation(item.prevMainCell, false, item.size);
                    item.transform.SetParent(item.canvas.transform);
                }
                else if (item.prevAmmunitionCell)
                {
                    dragItem.prevAmmunitionCell.isFree = false;
                    item.transform.SetParent(dragItem.prevAmmunitionCell.transform);
                }
            }
            dragItem.prevAmmunitionCell = this;
            dragItem.prevMainCell = null;
            dragItem.SetPosition(dragItem, null, this);
        }
        else if (eventData.pointerDrag.GetComponent<ItemAmmunition>())
        {
            dragItem.SetPosition(dragItem, dragItem.prevMainCell ? dragItem.prevMainCell : null, dragItem.prevAmmunitionCell ? dragItem.prevAmmunitionCell : null);

            if (dragItem.prevMainCell)
                dragItem.prevMainCell.CellOccupation(dragItem.prevMainCell, false, dragItem.size);
            else
                dragItem.prevAmmunitionCell.isFree = false;
        }
        else
        {
            dragItem.SetPosition(dragItem, dragItem.prevMainCell, null);
            dragItem.prevMainCell.CellOccupation(dragItem.prevMainCell, false, dragItem.size);
        }
    }
}