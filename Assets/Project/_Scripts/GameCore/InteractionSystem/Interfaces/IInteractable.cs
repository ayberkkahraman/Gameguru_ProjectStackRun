namespace Project._Scripts.GameCore.InteractionSystem.Interfaces
{
  public interface IInteractable
  {
    public bool IsInteractable { get; set; }
    void StartInteraction();
    void EndInteraction();
  }
}
