using Project._Scripts.GameCore.InteractionSystem.Interactables.Core;
using Project._Scripts.Global.ScriptableObjects;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Elements
{
  public class DeadZone : InteractableBase
  {
    private void OnEnable() => GameManagerData.OnGameStartedHandler += () => IsInteractable = false;
    private void OnDisable() => GameManagerData.OnGameStartedHandler -= () => IsInteractable = false;
    public override void StartInteraction()
    {
      base.StartInteraction();
      GameManagerData.OnLevelFailHandler();
    }
  }
}
