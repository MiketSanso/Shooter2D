using UnityEngine;
using System.Collections;

public abstract class Gun : Weapon
{
    [SerializeField] protected GameObject bulletObject;

    [SerializeField] protected Transform ponintShoot, pointSpawnPatron;

    [SerializeField] protected int constantAmmo, variableAmmo;
    [SerializeField] protected float timeShootOneBulletInSecond, timeRecharge;

    public override void Hit()
    {
        StartCoroutine(Shoot());
    }

    public abstract IEnumerator Shoot();

    public abstract IEnumerator Recharge();
}
