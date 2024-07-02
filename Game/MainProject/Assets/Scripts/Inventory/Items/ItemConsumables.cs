using UnityEngine.EventSystems;

public class ItemConsumables : InventoryItem
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (prevMainCell != null)
            prevMainCell.CellOccupation(prevMainCell, true, size);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (eventData.pointerEnter.GetComponent<MainCell>() || prevMainCell)
        {
            prevMainCell.CellOccupation(prevMainCell, false, size);
            SetPosition(this, prevMainCell, null);
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        InventoryItem dragItem = eventData.pointerDrag.GetComponent<ItemConsumables>();
        if (dragItem == null)
            dragItem = eventData.pointerDrag.GetComponent<ItemAmmunition>();

        if (eventData.pointerDrag.GetComponent<ItemConsumables>() && dragItem.size == size)
        {
            dragItem.SetPosition(dragItem, prevMainCell, null);
            SetPosition(this, dragItem.prevMainCell, null);

            MainCell bufferCell = dragItem.prevMainCell;

            dragItem.prevMainCell = prevMainCell;
            prevMainCell = bufferCell;

            prevMainCell.CellOccupation(prevMainCell, false, size);
        }

        dragItem.SetPosition(dragItem, dragItem.prevMainCell ? dragItem.prevMainCell : null, dragItem.prevAmmunitionCell ? dragItem.prevAmmunitionCell : null);

        if (dragItem.prevMainCell != null)
            dragItem.prevMainCell.CellOccupation(dragItem.prevMainCell, false, dragItem.size);
        else if (dragItem.prevAmmunitionCell != null)
            dragItem.prevAmmunitionCell.isFree = false;
    }
}
