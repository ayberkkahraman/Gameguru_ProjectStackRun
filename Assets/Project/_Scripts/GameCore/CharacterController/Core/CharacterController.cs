using System.Collections;
using DG.Tweening;
using Project._Scripts.GameCore.CharacterController.ScriptableObjects;
using Project._Scripts.GameCore.PlatformSystem.System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project._Scripts.GameCore.CharacterController.Core
{
  [DefaultExecutionOrder(800)]
  public class CharacterController : MonoBehaviour
  {
    #region Components
    public CharacterLocomotionData CharacterLocomotionData;
    private Animator _animator;
    private UnityEngine.CharacterController _characterController;
    #endregion

    #region Fields
    private float _movementSpeed;
    public bool CanMove;
    #endregion

    #region Unity Functions
    private void Awake()
    {
      InitializeComponents();
      InitializeLocomotion();
    }
    private void OnEnable() => Initialize();
    private void OnDisable() => DeInitialize();
    private void Update(){if(CanMove) MoveCharacter();}
    #endregion

    #region Initialize / DeInitialization
    private void Initialize()
    {
      InitializeDelegates();
      InitializeComponents();
      InitializeLocomotion();
    }
    private void DeInitialize() => DeInitializeDelegates();
    
    private void InitializeDelegates() => PlatformController.OnPlatformSpawnedHandler += (_) => TranslateCharacter();
    private void DeInitializeDelegates() => PlatformController.OnPlatformSpawnedHandler -= (_) => TranslateCharacter();

    private void InitializeLocomotion() => _movementSpeed = CharacterLocomotionData.MovementSpeed;

    private void InitializeComponents()
    {
      _animator = GetComponent<Animator>();
      _characterController = GetComponent<UnityEngine.CharacterController>();
    }
    #endregion
    
    #region Character Movement Behaviour
    private void MoveCharacter() => transform.position += transform.forward * (_movementSpeed * Time.deltaTime);
    internal void TranslateCharacter() => StartCoroutine(TranslateCharacterCoroutine());
    
    IEnumerator TranslateCharacterCoroutine()
    {
      yield return new WaitForSeconds(PlatformController.SPlatformControllerData.ScaleDuration);
      
      transform.DOMoveX(PlatformController.PreviousPlatform.position.x, PlatformController.SPlatformControllerData.ScaleDuration).SetEase(Ease.InOutSine);
    }
    #endregion
  }
}
