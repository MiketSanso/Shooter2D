using System;
using System.Collections;
using UnityEngine;

public class GunPlayer : Gun
{
    [SerializeField] protected TMPro.TMP_Text rechargeText;

    [SerializeField] protected string typeBullet;

    void OnEnable()
    {
        PlayerPrefs.SetString("activeGunTypeBullet", typeBullet);
        PlayerPrefs.SetInt("activeGunConstantAmmo", constantAmmo);
        PlayerPrefs.SetInt("activeGunVariableAmmo", variableAmmo);
        PlayerPrefs.Save();
    }

    public override IEnumerator Shoot()
    {
        do
        {
            if (variableAmmo > 0)
            {
                variableAmmo -= 1;

                audioSource.PlayOneShot(audioClipShoot);

                PlayerPrefs.SetInt("activeGunVariableAmmo", variableAmmo);
                PlayerPrefs.Save();

                RaycastHit2D hit = Physics2D.Raycast(ponintShoot.position, ponintShoot.right);

                if (hit)
                {
                    Instantiate(bulletObject, hit.point, Quaternion.identity);

                    if (hit.transform.gameObject.TryGetComponent<IDamageable>(out var damageable))
                        damageable.Damage(damage);
                }
            }
            else
                yield return null;

            yield return new WaitForSeconds(1.0f / timeShootOneBulletInSecond);
        } while (Input.GetButton("Fire1") && !Input.GetKey(KeyCode.LeftControl));
        yield return null;
    }

    public override IEnumerator Recharge()
    {
        int numberGunBulletsInMassive = 0;

        for (int i = 0; i < SaveManager.saveMg.activeSave.idInventoryItemsInMainCell.Count; i++)
        {
            if (SaveManager.saveMg.activeSave.idInventoryItemsInMainCell[i] == typeBullet)
            {
                if (SaveManager.saveMg.activeSave.countObjectsInPack[i] == 0)
                    yield break;

                numberGunBulletsInMassive = i;
                break;
            }
        }
        PlayerPrefs.SetInt("StopAllAnimations", 1);

        for (float i = 0f; i <= 1f; i += 0.05f)
        {
            yield return new WaitForSeconds(0.05f);
            rechargeText.GetComponent<TMPro.TMP_Text>().color = new Color(0, 0, 0, (float)Math.Round(Convert.ToDouble(i), 1));
        }

        if (SaveManager.saveMg.activeSave.countObjectsInPack[numberGunBulletsInMassive] > constantAmmo - variableAmmo)
        {
            SaveManager.saveMg.activeSave.countObjectsInPack[numberGunBulletsInMassive] = SaveManager.saveMg.activeSave.countObjectsInPack[numberGunBulletsInMassive] - (constantAmmo - variableAmmo);
            variableAmmo = constantAmmo;
        }
        else
        {
            variableAmmo += SaveManager.saveMg.activeSave.countObjectsInPack[numberGunBulletsInMassive];
            SaveManager.saveMg.activeSave.countObjectsInPack[numberGunBulletsInMassive] = 0;
        }
        SaveManager.saveMg.Save();
        PlayerPrefs.SetInt("activeGunVariableAmmo", variableAmmo);


        yield return new WaitForSeconds(timeRecharge - 1);

        for (float i = 1f; i >= -0.1; i -= 0.1f)
        {
            yield return new WaitForSeconds(0.04f);
            rechargeText.GetComponent<TMPro.TMP_Text>().color = new Color(0, 0, 0, (float)Math.Round(Convert.ToDouble(i), 1));
        }

        PlayerPrefs.SetInt("StopAllAnimations", 0);
        PlayerPrefs.Save();
        yield return null;
    }
}
