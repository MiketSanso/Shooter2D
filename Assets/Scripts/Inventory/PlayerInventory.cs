using UnityEngine;
using Zenject;

public class PlayerInventory : Inventory
{
    public GameObject[] ammunitionCells;
    private PrefabsManager _prefabsManager;

    [Inject]
    public void Construct(PrefabsManager prefabsManager)
    {
        _prefabsManager = prefabsManager;
    }

    private void Start()
    {
        if (SaveManager.saveMg.activeSave.idInventoryItemsInMainCell.Count == 0)
        {
            SaveManager.saveMg.activeSave.idInventoryItemsInMainCell.Add("7.92×33");
            SaveManager.saveMg.activeSave.xCellIndexItems.Add(0);
            SaveManager.saveMg.activeSave.yCellIndexItems.Add(0);
            SaveManager.saveMg.activeSave.countObjectsInPack.Add(30);

            SaveManager.saveMg.activeSave.idInventoryItemsInMainCell.Add("stg-44");
            SaveManager.saveMg.activeSave.xCellIndexItems.Add(1);
            SaveManager.saveMg.activeSave.yCellIndexItems.Add(0);
            SaveManager.saveMg.activeSave.countObjectsInPack.Add(0);
            SaveManager.saveMg.Save();
        }

        SpawnItemsInInventory();

        gameObject.SetActive(false);
    }

    public void SpawnItemsInInventory()
    {
        if (SaveManager.saveMg.activeSave.idInventoryItemsInMainCell.Count != 0)
        {
            for (int a = 0; a < SaveManager.saveMg.activeSave.idInventoryItemsInMainCell.Count; a++)
            {
                for (int b = 0; b < _prefabsManager.itemsPrefabsInvetory.Length; b++)
                {
                    if (_prefabsManager.itemsPrefabsInvetory[b].GetComponent<Item>() && _prefabsManager.itemsPrefabsInvetory[b].GetComponent<Item>().id == SaveManager.saveMg.activeSave.idInventoryItemsInMainCell[a])
                    {
                        GameObject inventoryObject = Instantiate(_prefabsManager.itemsPrefabsInvetory[b], transform.parent.transform);
                        inventoryObject.GetComponent<InventoryItem>().canvas = transform.parent.GetComponent<Canvas>();

                        if (inventoryObject.GetComponent<ItemConsumables>())
                            inventoryObject.GetComponent<ItemConsumables>()._countObjectsInPack = SaveManager.saveMg.activeSave.countObjectsInPack[a];

                        inventoryObject.GetComponent<InventoryItem>().prevMainCell = cells[SaveManager.saveMg.activeSave.xCellIndexItems[a], SaveManager.saveMg.activeSave.yCellIndexItems[a]];
                        inventoryObject.GetComponent<InventoryItem>().SetPosition(inventoryObject.GetComponent<InventoryItem>(), inventoryObject.GetComponent<InventoryItem>().prevMainCell, null);
                    }
                }
            }
        }

        if (SaveManager.saveMg.activeSave.idInventoryItemsInAmmunitionCell.Count != 0)
        {
            for (int a = 0; a < SaveManager.saveMg.activeSave.idInventoryItemsInAmmunitionCell.Count; a++)
            {
                for (int b = 0; b < _prefabsManager.itemsPrefabsInvetory.Length; b++)
                {
                    if (_prefabsManager.itemsPrefabsInvetory[b].GetComponent<Item>() && _prefabsManager.itemsPrefabsInvetory[b].GetComponent<Item>().id == SaveManager.saveMg.activeSave.idInventoryItemsInAmmunitionCell[a])
                    {
                        GameObject inventoryObject = Instantiate(_prefabsManager.itemsPrefabsInvetory[b], transform.parent.transform);
                        inventoryObject.GetComponent<InventoryItem>().canvas = transform.parent.GetComponent<Canvas>();

                        for (int c = 0; c < ammunitionCells.Length; c++)
                        {
                            if (ammunitionCells[c].GetComponent<AmmunitionCell>().cellAmmunType == inventoryObject.GetComponent<ItemAmmunition>().itemAmmunType)
                            {
                                if (SaveManager.saveMg.activeSave.indexAmmunitionCell.Count > 0 && ammunitionCells[c].GetComponent<AmmunitionCell>().indexGunCell == SaveManager.saveMg.activeSave.indexAmmunitionCell[0])
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
                    }
                }
            }
        }
    }
}
