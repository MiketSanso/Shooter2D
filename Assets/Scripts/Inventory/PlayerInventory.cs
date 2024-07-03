using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    public GameObject[] ammunitionCells;

    [SerializeField] private PrefabsManager prefabsManager;
    [SerializeField] private GridLayoutGroup grid;

    private void Start()
    {
        if (SaveManager.saveMg.activeSave.isStayedInMainCell.Count == 0)
        {
            SaveManager.saveMg.activeSave.isStayedInMainCell.Add(true);
            SaveManager.saveMg.activeSave.xCellIndexItems.Add(1);
            SaveManager.saveMg.activeSave.yCellIndexItems.Add(1);
            SaveManager.saveMg.activeSave.idInventoryItems.Add("stg-44");
            SaveManager.saveMg.Save();
        }

        grid.CalculateLayoutInputHorizontal();
        grid.CalculateLayoutInputVertical();
        grid.SetLayoutHorizontal();
        grid.SetLayoutVertical();

        SpawnItemsInInventory();

        transform.parent.transform.parent.gameObject.SetActive(false);
    }

    public void SpawnItemsInInventory()
    {
        if (SaveManager.saveMg.activeSave.idInventoryItems.Count != 0)
        {
            for (int a = 0; a < SaveManager.saveMg.activeSave.idInventoryItems.Count; a++)
            {
                for (int b = 0; b < prefabsManager.gunsPrefabsInvetory.Length; b++)
                {
                    if (prefabsManager.gunsPrefabsInvetory[b].GetComponent<Item>() && prefabsManager.gunsPrefabsInvetory[b].GetComponent<Item>().id == SaveManager.saveMg.activeSave.idInventoryItems[a])
                    {
                        GameObject inventoryObject = Instantiate(prefabsManager.gunsPrefabsInvetory[b], transform.parent.transform);

                        inventoryObject.GetComponent<InventoryItem>().canvas = transform.parent.GetComponent<Canvas>();

                        if (SaveManager.saveMg.activeSave.isStayedInMainCell[a])
                        {
                            inventoryObject.GetComponent<InventoryItem>().prevMainCell = cells[SaveManager.saveMg.activeSave.xCellIndexItems[a], SaveManager.saveMg.activeSave.yCellIndexItems[a]];
                            inventoryObject.GetComponent<InventoryItem>().SetPosition(inventoryObject.GetComponent<InventoryItem>(), inventoryObject.GetComponent<InventoryItem>().prevMainCell, null);
                        }
                        else
                        { 
                            for (int c = 0; c < ammunitionCells.Length; c++)
                            {
                                if (ammunitionCells[c].GetComponent<AmmunitionCell>().cellAmmunType == inventoryObject.GetComponent<ItemAmmunition>().itemAmmunType)
                                {
                                    if (SaveManager.saveMg.activeSave.indexCellGun.Count > 0 && ammunitionCells[c].GetComponent<AmmunitionCell>().indexGunCell == SaveManager.saveMg.activeSave.indexCellGun[0])
                                    {
                                        inventoryObject.GetComponent<ItemAmmunition>().prevAmmunitionCell = ammunitionCells[c].GetComponent<AmmunitionCell>();
                                        ammunitionCells[c].GetComponent<AmmunitionCell>().isFree = false;

                                        inventoryObject.GetComponent<ItemAmmunition>().SetPosition(inventoryObject.GetComponent<ItemAmmunition>(), null, inventoryObject.GetComponent<ItemAmmunition>().prevAmmunitionCell);
                                        return;
                                    }
                                    else if (ammunitionCells[c].GetComponent<AmmunitionCell>().indexGunCell == 0)
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
