using UnityEngine;
using System.Collections;
using System;
using static DamageSettings;

public class Gun : Weapon
{
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private Transform ponintShoot, pointSpawnPatron;

    [SerializeField] private TMPro.TMP_Text rechargeText;

    [SerializeField] private int constantAmmo, variableAmmo, timeRecharge;
    [SerializeField] private int timeShootOneBulletInSecond;

    [SerializeField] private string typeBullet;

    void OnEnable()
    {
        PlayerPrefs.SetInt("activeGunConstantAmmo", constantAmmo);
        PlayerPrefs.SetInt("activeGunVariableAmmo", variableAmmo);
        PlayerPrefs.Save();
    }

    public override void Hit()
    {
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
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
                        damageable.Damage(radiation, rupture, toxic, bleeding, bulletproof, damage, true, false, false, isToxic, isRadiation);
                }
            }
            else
                yield return null;

            yield return new WaitForSeconds(1.0f/timeShootOneBulletInSecond);
        } while (Input.GetButton("Fire1") && !Input.GetKey(KeyCode.LeftControl));
        yield return null;
    }

    public IEnumerator Recharge()
    {
        PlayerPrefs.SetInt("StopAllAnimations", 1);
#warning анимация перезарядки!
        for (float i = 0f; i <= 1f; i += 0.05f)
        {
            yield return new WaitForSeconds(0.05f);
            rechargeText.GetComponent<TMPro.TMP_Text>().color = new Color(0, 0, 0, (float)Math.Round(Convert.ToDouble(i), 1));
        }
#warning -патроны из инвентаря (и присваивание лишь нужного количества патронов оружию)

        yield return new WaitForSeconds(timeRecharge - 1);

        variableAmmo = constantAmmo;
        PlayerPrefs.SetInt("activeGunVariableAmmo", variableAmmo);
        PlayerPrefs.Save();

        for (float i = 1f; i >= -0.1; i -= 0.1f)
        {
            yield return new WaitForSeconds(0.04f);
            rechargeText.GetComponent<TMPro.TMP_Text>().color = new Color(0, 0, 0, (float)Math.Round(Convert.ToDouble(i), 1));
        }

        PlayerPrefs.SetInt("StopAllAnimations", 0);
        yield return null;
    }
}
