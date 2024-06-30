namespace Project._Scripts.GameCore.InteractionSystem.Interfaces
{
  public interface IInteractor
  {
    public IInteractable Interactable { get; set; }
    public void Subscribe(IInteractable interactable) => Interactable = interactable;
  }
}
