using UnityEngine;
using Zenject;

public class InputController : MonoBehaviour
{
    public AmmunitionCell[] _cellsInventory;

    private PrefabsManager _prefabsManager;

    public GameObject inventory, menu, mainUI;

    public Animator animator;

    public string lastGunID;

    [Inject]
    private void Construct(PrefabsManager prefabsManager, AmmunitionCell[] cellsInventory)
    {
        _prefabsManager = prefabsManager;
        _cellsInventory = cellsInventory;
    }

    private void Start()
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            menu.SetActive(!menu.active);

        if (!Input.GetKey(KeyCode.LeftControl) && PlayerPrefs.GetInt("recharge") == 0 && PlayerPrefs.GetInt("StopAllAnimations") == 0 && PlayerPrefs.GetInt("IsActivePanels") == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3))
                ActiveGunController();

            if (Input.GetButtonDown("Fire1") && PlayerPrefs.GetString("idActiveGun") != "arm")
            {
                for (int a = 0; a < _prefabsManager.gunsUsable.Length; a++)
                {
                    if (_prefabsManager.gunsUsable[a].GetComponent<Item>().id == PlayerPrefs.GetString("idActiveGun"))
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
                mainUI.SetActive(!mainUI.active);
                PlayerPrefs.SetInt("IsActivePanels", PlayerPrefs.GetInt("IsActivePanels") == 0 ? 1 : 0);
                PlayerPrefs.Save();
            }

            if (Input.GetKeyDown(KeyCode.R) && PlayerPrefs.GetInt("StopAllAnimations") == 0)
                StartCoroutine(ActiveGun().Recharge());
        }
    }

    private Gun ActiveGun()
    {
        for (int a = 0; a < _prefabsManager.gunsUsable.Length; a++)
            if (_prefabsManager.gunsUsable[a].GetComponent<Item>().id == PlayerPrefs.GetString("idActiveGun"))
                return _prefabsManager.gunsUsable[a].GetComponent<Gun>();
        return null;
    }

    private void ActiveGunController()
    {
        if (PlayerPrefs.GetInt("IsActivePanels") == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerPrefs.GetInt("InputedButton") != 1)
                InputGunResult(1, false);
            else if (Input.GetKeyDown(KeyCode.Alpha2) && PlayerPrefs.GetInt("InputedButton") != 2)
                InputGunResult(2, false);
            else if (Input.GetKeyDown(KeyCode.Alpha3) && PlayerPrefs.GetInt("InputedButton") != 3)
                InputGunResult(3, false);
            else
                InputGunResult(0, false);

            if (lastGunID != "arm")
                animator.SetTrigger("downGun");
            else if (PlayerPrefs.GetString("idActiveGun") != "arm" && lastGunID == "arm")
                animator.SetTrigger("downHands");
        }
    }

    public void InputGunResult(int inputedButtonNumber, bool isRaisedArms)
    {
        lastGunID = PlayerPrefs.GetString("idActiveGun");

        PlayerPrefs.SetInt("InputedButton", inputedButtonNumber);

        if (!isRaisedArms)
        {
            for (int i = 0; i < _cellsInventory.Length; i++)
            {
                if (_cellsInventory[i].cellAmmunType == AmmunType.Gun && _cellsInventory[i].GetComponent<AmmunitionCell>().indexGunCell == inputedButtonNumber * -1)
                    _cellsInventory[i].GetComponent<GivingItemsInformations>().GiveGunInformation();
            }

            PlayerPrefs.SetInt("StopAllAnimations", 1);
        }
        else
        {
            PlayerPrefs.SetString("idActiveGun", "arm");
            PlayerPrefs.SetInt("constantAmmo", 0);
            PlayerPrefs.SetInt("variableAmmo", 0);

            if (lastGunID != "arm")
                PlayerPrefs.SetInt("StopAllAnimations", 1);
        }
        PlayerPrefs.Save();
    }
}
