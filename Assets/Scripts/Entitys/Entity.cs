using System.Collections;
using UnityEngine;
using static DamageSettings;

public abstract class Entity : MonoBehaviour, IDamageable
{
    public float _health, _toxic, _explosionResistance, _bleeding, _bulletproof, _rupture, _radiation;

    public virtual void Damage(float radiationDamage, float ruptureDamage, float toxixDamage, float bleedingDamage, float bulletProofDamage, float damage, bool isBullet, bool isMelee, bool isGrenade, bool isToxic, bool isRadiation)
    {

        _health -= damage;

        if (isBullet && Random.Range(1, 101) >= _bulletproof)
        {
            _health -= bulletProofDamage;
            StartCoroutine(DealingDamage(bleedingDamage / _bleeding, 60));

            if (Random.Range(1, 101) >= _rupture)
                StartCoroutine(DealingDamage(ruptureDamage / _rupture, 140));
        }

        if (isMelee && Random.Range(1, 101) >= _bleeding)
        {
            StartCoroutine(DealingDamage(bleedingDamage / _bleeding, 60));

            if (Random.Range(1, 101) >= _rupture)
                StartCoroutine(DealingDamage(ruptureDamage / _rupture, 140));
        }

        if (isToxic && Random.Range(1, 101) >= _toxic)
        {
            StartCoroutine(DealingDamage(toxixDamage / _toxic, 95));
        }

        if (isRadiation && Random.Range(1, 101) >= _toxic)
        {
            StartCoroutine(DealingDamage(radiationDamage / _radiation, 360));
        }
    }

    public virtual void DestroyObject()
    {
        if (_health <= 0)
            Destroy(gameObject);
    }

    public virtual IEnumerator DealingDamage(float _damage, int timeDamage)
    {
        for (int i = 0; i < timeDamage; i++)
        {
            _health -= _damage;
            if(_health > 0)
                yield return new WaitForSeconds(2f);
        }
        yield return null;
    }

    public void Update()
    {
        DestroyObject();
    }
}