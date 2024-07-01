using System.Collections;
using DG.Tweening;
using Project._Scripts.GameCore.CharacterController.ScriptableObjects;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Project._Scripts.GameCore.CharacterController.Core
{
  [DefaultExecutionOrder(800)]
  public class CharacterController : MonoBehaviour
  {
    #region Dependency Injection
    [Inject]
    public void Construct(PlatformController platformController, CameraManager cameraManager)
    {
      _platformController = platformController;
      _cameraManager = cameraManager;
    }
    
    private PlatformController _platformController;
    private CameraManager _cameraManager;
    #endregion

    #region Components
    public CharacterLocomotionData CharacterLocomotionData;
    private Rigidbody _rigidbody;
    #endregion

    #region Fields
    private float _movementSpeed;
    public bool CanMove { get; set; }
    #endregion

    #region Unity Functions
    private void Awake() => Initialize();
    private void OnEnable() => InitializeDelegates();
    private void OnDisable() => DeInitializeDelegates();
    
    private void Update(){if(CanMove) MoveCharacter();}
    #endregion

    #region Initialize / DeInitialization
    private void Initialize()
    {
      InitializeComponents();
      InitializeLocomotion();
    }

    private void InitializeDelegates()
    {
      PlatformController.OnPlatformKilledHandler += (platform) => TranslateCharacter(platform.transform);
      GameManagerData.OnLevelSuccessHandler += () => CanMove = false;
      GameManagerData.OnGameStartedHandler += () => CanMove = true;
      GameManagerData.OnGameStartedHandler += () => _cameraManager.UpdateFollowTarget(transform);
    }
    private void DeInitializeDelegates()
    {
      PlatformController.OnPlatformKilledHandler -= (platform) => TranslateCharacter(platform.transform);
      GameManagerData.OnLevelSuccessHandler -= () => CanMove = false;
      GameManagerData.OnGameStartedHandler -= () => CanMove = true;
      GameManagerData.OnGameStartedHandler -= () => _cameraManager.UpdateFollowTarget(transform);
    }

    private void InitializeLocomotion() => _movementSpeed = CharacterLocomotionData.MovementSpeed;
    private void InitializeComponents() => _rigidbody = GetComponent<Rigidbody>();
    #endregion
    
    #region Character Movement Behaviour
    /// <summary>
    /// Moves the character
    /// </summary>
    private void MoveCharacter() => _rigidbody.MovePosition(_rigidbody.position + transform.forward * (_movementSpeed * Time.deltaTime));
    private void TranslateCharacter(Transform platform) => StartCoroutine(TranslateCharacterCoroutine(platform));
    
    IEnumerator TranslateCharacterCoroutine(Transform platform)
    {
      yield return new WaitForSeconds(_platformController.PlatformControllerData.ScaleDuration);
      
      transform.DOMoveX(platform.position.x, CharacterLocomotionData.TransitionSpeed).SetEase(Ease.InOutSine);
    }
    #endregion
  }
}
