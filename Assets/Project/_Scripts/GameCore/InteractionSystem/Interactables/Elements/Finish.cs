using Project._Scripts.GameCore.InteractionSystem.Interactables.Core;
using Project._Scripts.GameCore.Primitives;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Elements
{
  [DefaultExecutionOrder(800)]
  public class Finish : InteractableBase
  {
    public CameraManager CameraManager { get; set; }
    public Rotator CharacterPositionReference { get; set; }
    private void Awake() => CharacterPositionReference = GetComponentInChildren<Rotator>();
    private void Start() => IsInteractable = false;
    public override void StartInteraction()
    {
      base.StartInteraction();
      
      GameManagerData.OnLevelSuccessHandler();

      CharacterPositionReference.transform.parent = CameraManager.CharacterCamera.Follow;
      CharacterPositionReference.transform.localPosition = Vector3.zero;
      
      CameraManager.LevelEndCamera.Follow = (CharacterPositionReference.transform);
      CameraManager.LevelEndCamera.LookAt = (CharacterPositionReference.transform);
      CameraManager.ChangeActiveCamera(CameraManager.LevelEndCamera);
      
      CharacterPositionReference.Rotate();
    }
  }
}
