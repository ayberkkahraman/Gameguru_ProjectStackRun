using Project._Scripts.GameCore.InteractionSystem.Interactables.Core;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;
using Zenject;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Elements
{
  public class DeadZone : InteractableBase
  {
    private CameraManager _cameraManager;
    [Inject]
    public void Construct(CameraManager cameraManager) => _cameraManager = cameraManager;
    private void OnEnable() => GameManagerData.OnGameStartedHandler += () => IsInteractable = false;
    private void OnDisable() => GameManagerData.OnGameStartedHandler -= () => IsInteractable = false;
    public override void StartInteraction()
    {
      base.StartInteraction();
      GameManagerData.OnLevelFailHandler();
      _cameraManager.UpdateFollowTarget(null);
    }
  }
}
