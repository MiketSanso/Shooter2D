using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    public GameObject[] ammunitionCells;

    [SerializeField] private PrefabsManager prefabsManager;
    [SerializeField] private GridLayoutGroup grid;

    private void Start()
    {
        if (SaveManager.saveMg.activeSave.xCellIndexGuns.Count == 0)
        {
            SaveManager.saveMg.activeSave.idInventoryGuns.Add("stg-44");
            SaveManager.saveMg.activeSave.xCellIndexGuns.Add(1);
            SaveManager.saveMg.activeSave.yCellIndexGuns.Add(1);
            SaveManager.saveMg.activeSave.isStayedInMainCell.Add(true);
        }

        grid.CalculateLayoutInputHorizontal();
        grid.CalculateLayoutInputVertical();
        grid.SetLayoutHorizontal();
        grid.SetLayoutVertical();

        SpawnItemsInInventory();

        SaveManager.saveMg.activeSave.idInventoryGuns.Clear();
        SaveManager.saveMg.activeSave.xCellIndexGuns.Clear();
        SaveManager.saveMg.activeSave.yCellIndexGuns.Clear();
        SaveManager.saveMg.activeSave.isStayedInMainCell.Clear();
        SaveManager.saveMg.activeSave.indexCellGun.Clear();

        transform.parent.transform.parent.gameObject.SetActive(false);
    }

    public void SpawnItemsInInventory()
    {
        if (SaveManager.saveMg.activeSave.idInventoryGuns.Count != 0)
        {
            for (int a = 0; a < SaveManager.saveMg.activeSave.idInventoryGuns.Count; a++)
            {
                for (int b = 0; b < prefabsManager.gunsPrefabsInvetory.Length; b++)
                {
                    if (prefabsManager.gunsPrefabsInvetory[b].GetComponent<Item>() && prefabsManager.gunsPrefabsInvetory[b].GetComponent<Item>().id == SaveManager.saveMg.activeSave.idInventoryGuns[a])
                    {
                        GameObject inventoryObject = Instantiate(prefabsManager.gunsPrefabsInvetory[b], cells[SaveManager.saveMg.activeSave.xCellIndexGuns[a], SaveManager.saveMg.activeSave.yCellIndexGuns[a]].transform);

                        inventoryObject.GetComponent<InventoryItem>().canvas = transform.parent.GetComponent<Canvas>();

                        if (SaveManager.saveMg.activeSave.isStayedInMainCell[a])
                        {
                            inventoryObject.GetComponent<InventoryItem>().prevMainCell = cells[SaveManager.saveMg.activeSave.xCellIndexGuns[a], SaveManager.saveMg.activeSave.yCellIndexGuns[a]];
                            inventoryObject.GetComponent<InventoryItem>().SetPosition(inventoryObject.GetComponent<InventoryItem>(), inventoryObject.GetComponent<InventoryItem>().prevMainCell, null);
                        }
                        else
                        { 
                            for (int c = 0; c < ammunitionCells.Length; c++)
                            {
                                if (ammunitionCells[c].GetComponent<AmmunitionCell>().cellAmmunType == inventoryObject.GetComponent<ItemAmmunition>().itemAmmunType)
                                {
                                    if (SaveManager.saveMg.activeSave.indexCellGun.Count > 0 && ammunitionCells[c].GetComponent<AmmunitionCell>().indexGun == SaveManager.saveMg.activeSave.indexCellGun[0])
                                    {
                                        SaveManager.saveMg.activeSave.indexCellGun.RemoveAt(0);
                                        SaveManager.saveMg.Save();
                                        inventoryObject.GetComponent<ItemAmmunition>().prevAmmunitionCell = ammunitionCells[c].GetComponent<AmmunitionCell>();
                                        ammunitionCells[c].GetComponent<AmmunitionCell>().isFree = false;

                                        inventoryObject.GetComponent<ItemAmmunition>().SetPosition(inventoryObject.GetComponent<ItemAmmunition>(), null, inventoryObject.GetComponent<ItemAmmunition>().prevAmmunitionCell);
                                        return;
                                    }
                                    else if (ammunitionCells[c].GetComponent<AmmunitionCell>().indexGun == 0)
                                    {
                                        ammunitionCells[c].GetComponent<AmmunitionCell>().isFree = false;

                                        inventoryObject.GetComponent<ItemAmmunition>().SetPosition(inventoryObject.GetComponent<ItemAmmunition>(), null, inventoryObject.GetComponent<ItemAmmunition>().prevAmmunitionCell);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                    }
                }
            }
        }
    }
}
