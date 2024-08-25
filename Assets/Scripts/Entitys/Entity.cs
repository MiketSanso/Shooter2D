using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health, _armor;
    public float health
    {
        get { return _health; }
        protected set { _health = value; }
    }

    public float armor
    {
        get { return _armor; }
        protected set { _armor = value; }
    }

    public virtual void Damage(float damage)
    {
        if (damage - _armor > 0)
            _health -= damage - (damage / 100 * _armor);
    }

    protected abstract void DeathEntity();

    protected void Update()
    {
        DeathEntity();
    }
}