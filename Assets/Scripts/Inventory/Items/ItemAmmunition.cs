using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemAmmunition : InventoryItem
{
    public AmmunType itemAmmunType { get { return localItemAmmunType; } }
    [SerializeField] private AmmunType localItemAmmunType;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        if (prevMainCell != null)
            prevMainCell.CellOccupation(prevMainCell, true, size);
        else if (prevAmmunitionCell != null)
            prevAmmunitionCell.isFree = true;
        if (GetComponent<ItemAmmunition>() && Convert.ToString(itemAmmunType) == "Gun")
            transform.rotation = new Quaternion(0, 0, 0, 0);
    }  

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (eventData.pointerEnter.GetComponent<MainCell>() && prevMainCell || !eventData.pointerEnter.GetComponent<AmmunitionCell>() && prevMainCell || eventData.pointerEnter.GetComponent<AmmunitionCell>() && !eventData.pointerEnter.GetComponent<AmmunitionCell>().isFree && prevMainCell)
        {
            SetPosition(this, prevMainCell, null);
            prevMainCell.CellOccupation(prevMainCell, false, size);
        }
        else if (prevAmmunitionCell)
        {
            SetPosition(this, null, prevAmmunitionCell);
            prevAmmunitionCell.isFree = false;
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        InventoryItem dragItem = eventData.pointerDrag.GetComponent<ItemConsumables>();
        if (dragItem == null)
            dragItem = eventData.pointerDrag.GetComponent<ItemAmmunition>();

        if (eventData.pointerDrag.GetComponent<ItemAmmunition>() && itemAmmunType == dragItem.GetComponent<ItemAmmunition>().itemAmmunType && (dragItem.prevMainCell && (prevMainCell && dragItem.prevMainCell.CheckCellCanBeCastled(dragItem.prevMainCell, size) && prevMainCell.CheckCellCanBeCastled(prevMainCell, dragItem.size) || !prevMainCell && dragItem.prevMainCell.CheckCellCanBeCastled(dragItem.prevMainCell, size)) || !dragItem.prevMainCell && prevMainCell && prevMainCell.CheckCellCanBeCastled(prevMainCell, dragItem.size) || !prevMainCell && !dragItem.prevMainCell)) 
        {
            transform.SetParent(canvas.transform);

            SetPosition(this, dragItem.prevMainCell ? dragItem.prevMainCell : null, dragItem.prevAmmunitionCell ? dragItem.prevAmmunitionCell : null);
            dragItem.SetPosition(dragItem, prevMainCell ? prevMainCell : null, prevAmmunitionCell ? prevAmmunitionCell : null);
            

            var bufferMainCell = dragItem.prevMainCell ? dragItem.prevMainCell : null;
            var bufferAmmunCell = dragItem.prevAmmunitionCell ? dragItem.prevAmmunitionCell : null;

            dragItem.prevMainCell = prevMainCell;
            dragItem.prevAmmunitionCell = prevAmmunitionCell;

            prevMainCell = bufferMainCell;
            prevAmmunitionCell = bufferAmmunCell;

            if (prevMainCell && dragItem.prevMainCell)
            {
                prevMainCell.CellOccupation(prevMainCell, false, size);
                dragItem.prevMainCell.CellOccupation(dragItem.prevMainCell, true, size);
                dragItem.prevMainCell.CellOccupation(dragItem.prevMainCell, false, dragItem.size);
            }
            else if (prevMainCell)
            {
                prevMainCell.CellOccupation(prevMainCell, true, dragItem.size);
                prevMainCell.CellOccupation(prevMainCell, false, size);
            }
            else if (dragItem.prevMainCell)
            {
                dragItem.prevMainCell.CellOccupation(dragItem.prevMainCell, true, size);
                dragItem.prevMainCell.CellOccupation(dragItem.prevMainCell, false, dragItem.size);
                prevAmmunitionCell.isFree = false;
            }
            else if (prevAmmunitionCell)
                prevAmmunitionCell.isFree = false;
        }


        if (eventData.pointerDrag.GetComponent<ItemConsumables>())
        {
            dragItem.SetPosition(dragItem, dragItem.prevMainCell, null);
            dragItem.prevMainCell.CellOccupation(dragItem.prevMainCell, false, dragItem.size);
        }
    }

    public override void SetPosition(InventoryItem _item, MainCell _mainCell, AmmunitionCell _ammunitionCell)
    {
        base.SetPosition(_item, _mainCell, _ammunitionCell);

        if (_ammunitionCell)
        {
            _item.transform.localPosition = Vector3.zero;
            _item.transform.position = _ammunitionCell.transform.position;
            _item.transform.SetParent(_ammunitionCell.transform);

            _ammunitionCell.isFree = false;

            if (Convert.ToString(itemAmmunType) == "Gun")
                _item.transform.rotation = new Quaternion(0, 0, 90, 90);
        }
    }

    public override void SaveObject()
    {
        base.SaveObject();

        if (prevAmmunitionCell != null)
        {
            SaveManager.saveMg.activeSave.isStayedInMainCell.Add(false);

            if (itemAmmunType == AmmunType.Gun)
                SaveManager.saveMg.activeSave.indexCellGun.Add(prevAmmunitionCell.indexGunCell);
            else
                SaveManager.saveMg.activeSave.indexCellGun.Add(Array.IndexOf(Enum.GetValues(prevAmmunitionCell.cellAmmunType.GetType()), prevAmmunitionCell.cellAmmunType));
        }
        SaveManager.saveMg.Save();
    } 

    public override void DeleteObject()
    {
        base.DeleteObject();

        if (prevAmmunitionCell != null)
        {
            int stepInLists = 0;
         /*   do
            {
                stepInLists++;
            }
            while ((SaveManager.saveMg.activeSave.indexCellGun[stepInLists] != Array.IndexOf(Enum.GetValues(prevAmmunitionCell.cellAmmunType.GetType()), prevAmmunitionCell.cellAmmunType) && SaveManager.saveMg.activeSave.indexCellGun[stepInLists] > 0) ||  SaveManager.saveMg.activeSave.indexCellGun[stepInLists] != prevAmmunitionCell.indexGunCell); */

            SaveManager.saveMg.activeSave.isStayedInMainCell.RemoveAt(stepInLists); 
            SaveManager.saveMg.activeSave.idInventoryItems.RemoveAt(stepInLists);
            SaveManager.saveMg.activeSave.indexCellGun.RemoveAt(stepInLists);
            SaveManager.saveMg.Save();
       } 
    } 
}
