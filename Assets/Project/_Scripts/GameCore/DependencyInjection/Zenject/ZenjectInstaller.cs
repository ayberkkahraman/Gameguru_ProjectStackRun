using Project._Scripts.Global.Manager.ManagerClasses;
using UnityEngine;
using Zenject;

namespace Project._Scripts.GameCore.DependencyInjection.Zenject
{
  public class ZenjectInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<CameraManager>().FromComponentInHierarchy().AsSingle();
    }
  }
}
