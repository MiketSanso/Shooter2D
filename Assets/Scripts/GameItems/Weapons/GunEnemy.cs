using System;
using System.Collections;
using UnityEngine;

public class GunEnemy : Gun
{
    public void Updatr()
    {
        StartCoroutine(Shoot());
    }

    public override IEnumerator Shoot()
    {
        do
        {
            if (variableAmmo > 0)
            {
                variableAmmo -= 1;

                audioSource.PlayOneShot(audioClipShoot);

                RaycastHit2D hit = Physics2D.Raycast(ponintShoot.position, ponintShoot.right);
                Debug.Log(2);
                if (hit)
                {
                    Instantiate(bulletObject, hit.point, Quaternion.identity);

                    if (hit.transform.gameObject.TryGetComponent<IDamageable>(out var damageable))
                        damageable.Damage(damage);
                }
            }
            else
                StartCoroutine(Recharge());

            yield return new WaitForSeconds(1.0f / timeShootOneBulletInSecond);
        } while (Input.GetButton("Fire1") && !Input.GetKey(KeyCode.LeftControl));

        Debug.Log(1);

        yield return new WaitForSeconds(0.5f);
        yield return null;
    }

    public override IEnumerator Recharge()
    {
        yield return new WaitForSeconds(timeRecharge);
        variableAmmo = constantAmmo;
    }
}
