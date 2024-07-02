using UnityEngine;
using TMPro;

public class AmmoText : MonoBehaviour
{
    [SerializeField] private TMP_Text ammo;

    void Update()
    {
        ammo.text = PlayerPrefs.GetInt("activeGunVariableAmmo").ToString() + "/" + PlayerPrefs.GetInt("activeGunConstantAmmo").ToString();
    }
}
