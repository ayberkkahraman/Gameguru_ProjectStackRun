using Project._Scripts.GameCore.InteractionSystem.Interactables.Core;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.GameCore.Primitives;
using Project._Scripts.Global.Manager.Core;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Elements
{
  public class Finish : InteractableBase
  {
    private Rotator _rotator;
    private CameraManager _cameraManager;

    private void Awake()
    {
      _rotator = GetComponentInChildren<Rotator>();
      _cameraManager = ManagerCore.Instance.GetInstance<CameraManager>();
    }
    public override void StartInteraction()
    {
      base.StartInteraction();
      
      GameManagerData.OnLevelSuccessHandler();

      // _rotator.transform.position = new Vector3(_cameraManager.CharacterCamera.Follow.position.x, transform.position.y, transform.position.z);
      _rotator.transform.parent = _cameraManager.CharacterCamera.Follow;
      _rotator.transform.localPosition = Vector3.zero;
      
      _cameraManager.LevelEndCamera.Follow = (_rotator.transform);
      _cameraManager.LevelEndCamera.LookAt = (_rotator.transform);
      _cameraManager.ChangeActiveCamera(_cameraManager.LevelEndCamera);
      
      _rotator.Rotate();
    }
  }
}
