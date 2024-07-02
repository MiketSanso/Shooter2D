using UnityEngine;

public class GivingItemsInformations : MonoBehaviour
{
    public void GiveGunInformation()
    {
        if (transform.childCount > 0)
            PlayerPrefs.SetString("idActiveGun", transform.GetChild(0).GetComponent<Item>().id);
        else
            PlayerPrefs.SetString("idActiveGun", "arm");
    }
}