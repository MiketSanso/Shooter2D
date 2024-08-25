using UnityEngine;
using Zenject;

public class AmmunitionCellsInstaller : MonoInstaller
{
    [SerializeField] private AmmunitionCell[] inventoryCells;

    public override void InstallBindings()
    {
        Container.Bind<AmmunitionCell[]>().FromInstance(inventoryCells);
    }
}
