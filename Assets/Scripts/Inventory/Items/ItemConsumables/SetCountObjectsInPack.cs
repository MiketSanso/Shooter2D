using UnityEngine;
using TMPro;

public class SetCountObjectsInPack : MonoBehaviour
{
    [SerializeField] private TMP_Text countObjectsInPack;

    private void Update()
    {
        if (countObjectsInPack.text != FindCountObjectsInPack())
            countObjectsInPack.text = FindCountObjectsInPack();
    }

    private string FindCountObjectsInPack()
    {
        for (int i = 0; i < SaveManager.saveMg.activeSave.xCellIndexItems.Count; i++)
        {
            if (SaveManager.saveMg.activeSave.xCellIndexItems[i] == GetComponent<ItemConsumables>().prevMainCell.x && SaveManager.saveMg.activeSave.yCellIndexItems[i] == GetComponent<ItemConsumables>().prevMainCell.y)
                return SaveManager.saveMg.activeSave.countObjectsInPack[i].ToString();
        }
        return null;
    }
}
