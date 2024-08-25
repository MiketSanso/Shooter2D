using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class ItemConsumables : InventoryItem
{
    [HideInInspector] public int _countObjectsInPack;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (prevMainCell != null)
            prevMainCell.CellOccupation(prevMainCell, true, size);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<MainCell>() || prevMainCell)
        {
            prevMainCell.CellOccupation(prevMainCell, false, size);
            SetPosition(this, prevMainCell, null);
        }
        SaveObject();
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

    private void OnEnable()
    {
        if (SaveManager.saveMg.activeSave.xCellIndexItems.Count != 0 && prevMainCell != null)
        {
            int index = 0;

            while (SaveManager.saveMg.activeSave.xCellIndexItems[index] != prevMainCell.x && SaveManager.saveMg.activeSave.yCellIndexItems[index] != prevMainCell.y)
            {
                index++;
                if (SaveManager.saveMg.activeSave.xCellIndexItems[index]++ == SaveManager.saveMg.activeSave.xCellIndexItems.Count)
                {
                    index = -1;
                    break;
                }
                Debug.Log(index);
            }

            if (index != -1)
                _countObjectsInPack = SaveManager.saveMg.activeSave.countObjectsInPack[index];
        }
    }

    public override void SaveObject()
    {
        base.SaveObject();
        SaveManager.saveMg.activeSave.countObjectsInPack.Add(_countObjectsInPack);
        SaveManager.saveMg.Save();
    }
}
