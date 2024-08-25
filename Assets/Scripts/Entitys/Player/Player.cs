using UnityEngine;

public class Player : Entity
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject body;

    protected override void DeathEntity()
    {
        if (health <= 0)
            body.SetActive(false);
    }
}
