using UnityEngine;

public class InventoryButtons : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel, mainUIPanel;

    private void Awake()
    {
        PlayerPrefs.SetInt("IsActivePanels", 0);
        PlayerPrefs.Save();
        mainUIPanel.SetActive(true);
    }

    public void OpenCloseInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.active);
        mainUIPanel.SetActive(!mainUIPanel.active);
        PlayerPrefs.SetInt("IsActivePanels", PlayerPrefs.GetInt("IsActivePanels") == 0 ? 1 : 0);
        PlayerPrefs.Save();
    }
}
