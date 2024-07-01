using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Global.Manager.ManagerClasses;
using Zenject;

namespace Project._Scripts.GameCore.DependencyInjection.Zenject
{
  public class ZenjectInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<CameraManager>().FromComponentInHierarchy().AsSingle();
      Container.Bind<PlatformController>().FromComponentInHierarchy().AsSingle();
      Container.Bind<PoolManager>().FromComponentInHierarchy().AsSingle();
    }
  }
}
