using UnityEngine;
using TMPro;

public class AmmoText : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoActiveGun, bulletsInInventory, typeBullet;

    void Update()
    {
        for (int i = 0; i < SaveManager.saveMg.activeSave.idInventoryItemsInMainCell.Count; i++)
        {
            if (SaveManager.saveMg.activeSave.idInventoryItemsInMainCell[i] == PlayerPrefs.GetString("activeGunTypeBullet"))
            {
                bulletsInInventory.text = SaveManager.saveMg.activeSave.countObjectsInPack[i].ToString();
                break;
            }
        }

        ammoActiveGun.text = PlayerPrefs.GetInt("activeGunVariableAmmo").ToString() + "/" + PlayerPrefs.GetInt("activeGunConstantAmmo").ToString();
        typeBullet.text = PlayerPrefs.GetString("activeGunTypeBullet");
    }
}
