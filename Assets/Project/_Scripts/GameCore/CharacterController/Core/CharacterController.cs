using System.Collections;
using DG.Tweening;
using Project._Scripts.GameCore.CharacterController.ScriptableObjects;
using Project._Scripts.GameCore.PlatformSystem.System;
using UnityEngine;

namespace Project._Scripts.GameCore.CharacterController.Core
{
  [DefaultExecutionOrder(800)]
  public class CharacterController : MonoBehaviour
  {
    public CharacterLocomotionData CharacterLocomotionData;
    private Animator _animator;
    private UnityEngine.CharacterController _characterController;
    
    private float _movementSpeed;

    public bool Run;
    private void Awake()
    {
      InitializeComponents();
      InitializeLocomotion();
    }

    private void OnEnable()
    {
      PlatformController.OnPlatformSpawnedHandler += (_) => TranslateCharacter();
    }

    private void OnDisable()
    {
      PlatformController.OnPlatformSpawnedHandler -= (_) => TranslateCharacter();
    }

    private void Update()
    {
      if(!Run) return;
      
      MoveCharacter();
    }

    private void InitializeLocomotion()
    {
      _movementSpeed = CharacterLocomotionData.MovementSpeed;
    }

    private void InitializeComponents()
    {
      _animator = GetComponent<Animator>();
      _characterController = GetComponent<UnityEngine.CharacterController>();
    }

    private void MoveCharacter()
    {
      transform.position += transform.forward * (_movementSpeed * Time.deltaTime);
    }

    void TranslateCharacter() => StartCoroutine(TranslateCharacterCoroutine());

    IEnumerator TranslateCharacterCoroutine()
    {
      yield return new WaitForSeconds(PlatformController.SPlatformControllerData.ScaleDuration);
      
      transform.DOMoveX(PlatformController.PreviousPlatform.position.x, PlatformController.SPlatformControllerData.ScaleDuration).SetEase(Ease.InOutSine);
    }
  }
}
