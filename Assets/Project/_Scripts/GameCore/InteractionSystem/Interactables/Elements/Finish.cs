using Project._Scripts.GameCore.InteractionSystem.Interactables.Core;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.GameCore.Primitives;
using Project._Scripts.Global.Manager.Core;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Elements
{
  public class Finish : InteractableBase
  {
    public Rotator CharacterPositionReference { get; set; }
    private CameraManager _cameraManager;

    private void Awake()
    {
      CharacterPositionReference = GetComponentInChildren<Rotator>();
      _cameraManager = ManagerCore.Instance.GetInstance<CameraManager>();
    }
    public override void StartInteraction()
    {
      base.StartInteraction();
      
      GameManagerData.OnLevelSuccessHandler();

      // _rotator.transform.position = new Vector3(_cameraManager.CharacterCamera.Follow.position.x, transform.position.y, transform.position.z);
      CharacterPositionReference.transform.parent = _cameraManager.CharacterCamera.Follow;
      CharacterPositionReference.transform.localPosition = Vector3.zero;
      
      _cameraManager.LevelEndCamera.Follow = (CharacterPositionReference.transform);
      _cameraManager.LevelEndCamera.LookAt = (CharacterPositionReference.transform);
      _cameraManager.ChangeActiveCamera(_cameraManager.LevelEndCamera);
      
      CharacterPositionReference.Rotate();
    }
  }
}
