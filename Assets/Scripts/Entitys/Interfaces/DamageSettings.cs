using UnityEngine;

public class DamageSettings : MonoBehaviour
{
    public interface IDamageable
    {
        void Damage(float radiationDamage, float ruptureDamage, float toxixDamage, float bleedingDamage, float bulletProofDamage, float damage, bool isBullet, bool isMelee, bool isGrenade, bool isToxic, bool isRadiation);
    }
}
