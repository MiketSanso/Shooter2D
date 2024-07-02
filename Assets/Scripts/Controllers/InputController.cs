using UnityEngine;

public class InputController : MonoBehaviour
{
    public GivingItemsInformations inventoryGunOne, inventoryGunTwo, inventoryGunThree;

    [SerializeField] private PrefabsManager prefabsManager;

    public GameObject inventory, menu;

    public Animator animator;

    public string lastGunID;

    public void Start()
    {
        animator.SetTrigger("upHands");

        PlayerPrefs.SetInt("recharge", 0);
        PlayerPrefs.SetInt("InputedButton", 0);
        PlayerPrefs.SetInt("constantAmmo", 0);
        PlayerPrefs.SetInt("variableAmmo", 0);
        PlayerPrefs.SetInt("StopAllAnimations", 0);

        PlayerPrefs.SetString("idActiveGun", "arm");
        lastGunID = "arm";
        PlayerPrefs.Save();
    }

    private void ActiveGunController()
    {
        if (PlayerPrefs.GetInt("IsActivePanels") == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerPrefs.GetInt("InputedButton") != 1)
            {
                lastGunID = PlayerPrefs.GetString("idActiveGun");

                inventoryGunOne.GiveGunInformation();
                PlayerPrefs.SetInt("InputedButton", 1);
                PlayerPrefs.SetInt("StopAllAnimations", 1);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && PlayerPrefs.GetInt("InputedButton") != 2)
            {
                lastGunID = PlayerPrefs.GetString("idActiveGun");

                inventoryGunTwo.GiveGunInformation();
                PlayerPrefs.SetInt("InputedButton", 2);
                PlayerPrefs.SetInt("StopAllAnimations", 1);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && PlayerPrefs.GetInt("InputedButton") != 3)
            {
                lastGunID = PlayerPrefs.GetString("idActiveGun");

                inventoryGunThree.GiveGunInformation();
                PlayerPrefs.SetInt("InputedButton", 3);
                PlayerPrefs.SetInt("StopAllAnimations", 1);

            }
            else
            {
                lastGunID = PlayerPrefs.GetString("idActiveGun");

                if(lastGunID != "arm")
                    PlayerPrefs.SetInt("StopAllAnimations", 1);

                PlayerPrefs.SetInt("InputedButton", 0);
                PlayerPrefs.SetInt("constantAmmo", 0);
                PlayerPrefs.SetInt("variableAmmo", 0);
                PlayerPrefs.SetString("idActiveGun", "arm");
            }
            PlayerPrefs.Save();

            if (lastGunID != "arm")
                animator.SetTrigger("downGun");
            else if (PlayerPrefs.GetString("idActiveGun") != "arm" && lastGunID == "arm")
                animator.SetTrigger("downHands");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            menu.SetActive(!menu.active);

        if (!Input.GetKey(KeyCode.LeftControl) && PlayerPrefs.GetInt("recharge") == 0 && PlayerPrefs.GetInt("StopAllAnimations") == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
                ActiveGunController();

            if (Input.GetButtonDown("Fire1") && PlayerPrefs.GetString("idActiveGun") != "arm")
            {
                for (int a = 0; a < prefabsManager.gunsUsable.Length; a++)
                {
                    if (prefabsManager.gunsUsable[a].GetComponent<Item>().id == PlayerPrefs.GetString("idActiveGun"))
                    {
                        ActiveGun().Hit();
                        return;
                    }
                }
            }
            else if (Input.GetButtonDown("Fire1") && PlayerPrefs.GetString("idActiveGun") == "arm")
            {
                PlayerPrefs.SetInt("StopAllAnimations", 1);
                animator.SetTrigger("twoLightBlow");
                PlayerPrefs.Save();
            }
            else if (Input.GetButtonDown("Fire2") && PlayerPrefs.GetString("idActiveGun") == "arm")
            {
                PlayerPrefs.SetInt("StopAllAnimations", 2);
                animator.SetTrigger("twoHeavyBlow");
                PlayerPrefs.Save();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                inventory.SetActive(!inventory.active);
                PlayerPrefs.SetInt("IsActivePanels", PlayerPrefs.GetInt("IsActivePanels") == 0 ? 1 : 0);
                PlayerPrefs.Save();
            }

            if (Input.GetKeyDown(KeyCode.R) && PlayerPrefs.GetInt("StopAllAnimations") == 0)
                StartCoroutine(ActiveGun().Recharge());
        }
    }

    private Gun ActiveGun()
    {
        for (int a = 0; a < prefabsManager.gunsUsable.Length; a++)
            if (prefabsManager.gunsUsable[a].GetComponent<Item>().id == PlayerPrefs.GetString("idActiveGun"))
                return prefabsManager.gunsUsable[a].GetComponent<Gun>();
        return null;
    }
}
