using UnityEngine;

public class QuitManager : MonoBehaviour
{
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("ClearMassivesInventory", 0);
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        PlayerPrefs.SetInt("ClearMassivesInventory", 0);
        PlayerPrefs.Save();
    }
}
