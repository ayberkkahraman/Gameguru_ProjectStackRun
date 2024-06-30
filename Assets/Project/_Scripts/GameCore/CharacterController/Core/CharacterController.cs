using System.Collections;
using DG.Tweening;
using Project._Scripts.GameCore.CharacterController.ScriptableObjects;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Global.Manager.Core;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.GameCore.CharacterController.Core
{
  [DefaultExecutionOrder(800)]
  public class CharacterController : MonoBehaviour
  {
    #region Components
    private PlatformController _platformController;
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
      PlatformController.OnPlatformSpawnedHandler += (_) => TranslateCharacter();
      GameManagerData.OnLevelSuccessHandler += () => CanMove = false;
      GameManager.OnGameStartedHandler += () => CanMove = true;
    }
    private void DeInitializeDelegates()
    {
      PlatformController.OnPlatformSpawnedHandler -= (_) => TranslateCharacter();
      GameManagerData.OnLevelSuccessHandler -= () => CanMove = false;
      GameManager.OnGameStartedHandler -= () => CanMove = true;
    }

    private void InitializeLocomotion() => _movementSpeed = CharacterLocomotionData.MovementSpeed;

    private void InitializeComponents()
    {
      _rigidbody = GetComponent<Rigidbody>();
      _platformController = ManagerCore.Instance.GetInstance<PlatformController>();
    }
    #endregion
    
    #region Character Movement Behaviour
    private void MoveCharacter() => _rigidbody.MovePosition(_rigidbody.position + transform.forward * (_movementSpeed * Time.deltaTime));
    private void TranslateCharacter() => StartCoroutine(TranslateCharacterCoroutine());
    
    IEnumerator TranslateCharacterCoroutine()
    {
      yield return new WaitForSeconds(_platformController.PlatformControllerData.ScaleDuration);
      
      transform.DOMoveX(PlatformController.PreviousPlatform.position.x, _platformController.PlatformControllerData.ScaleDuration).SetEase(Ease.InOutSine);
    }
    #endregion
  }
}
