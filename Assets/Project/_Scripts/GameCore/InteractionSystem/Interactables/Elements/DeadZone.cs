using Project._Scripts.GameCore.InteractionSystem.Interactables.Core;
using Project._Scripts.Global.Manager.Core;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Elements
{
  public class DeadZone : InteractableBase
  {
    public override void StartInteraction()
    {
      base.StartInteraction();
      GameManagerData.OnLevelFailHandler();
      ManagerCore.Instance.GetInstance<CameraManager>().UpdateFollowTarget(null);
    }
  }
}
