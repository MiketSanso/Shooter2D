using UnityEngine;
using Zenject;

namespace InstallerPrefabsManager
{
    public class PrefabsManagerInstaller : MonoInstaller
    {
        [SerializeField] private PrefabsManager prefabsManger;

        public override void InstallBindings()
        {
            Container.Bind<PrefabsManager>().FromInstance(prefabsManger).AsSingle();
        }
    }
}
