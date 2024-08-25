public class Enemy : Entity
{
    protected override void DeathEntity()
    {
        if (health <= 0)
            Destroy(gameObject);
    }
}
