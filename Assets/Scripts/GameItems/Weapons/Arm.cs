using UnityEngine;

public class Arm : Weapon
{
    [SerializeField] private CircleCollider2D circleCollider;
    public override void Hit() {}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))//&&)
            damageable.Damage(damage*3);
    }
}
