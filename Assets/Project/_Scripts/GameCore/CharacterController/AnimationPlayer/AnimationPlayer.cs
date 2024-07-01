using System;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.GameCore.CharacterController.AnimationPlayer
{
  public class AnimationPlayer : MonoBehaviour
  {
    #region Components
    private Animator _animator;
    #endregion

    #region Animation Parameters
    private static readonly int Dance = Animator.StringToHash("Dance");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    #endregion

    #region Unity Functions
    private void Awake() => InitializeComponents();
    private void OnEnable() => InitializeDelegates();
    private void OnDisable() => DeInitializeDelegates();
    #endregion

    #region Initialization /DeInitialization
    private void InitializeComponents() => _animator = GetComponent<Animator>();
    private void InitializeDelegates()
    {
      GameManagerData.OnGameStartedHandler += () => PlayMoveAnimation(true);
      GameManagerData.OnLevelSuccessHandler += PlayDanceAnimation;
    }
    private void DeInitializeDelegates()
    {
      GameManagerData.OnGameStartedHandler -= () => PlayMoveAnimation(true);
      GameManagerData.OnLevelSuccessHandler -= PlayDanceAnimation;
    }
    #endregion

    #region Animation Behaviours
    public void PlayDanceAnimation()
    {
      PlayMoveAnimation(false);
      _animator.SetTrigger(Dance);
    }
    public void PlayMoveAnimation(bool condition) => _animator.SetBool(IsMoving, condition);
    #endregion
    
  }
}
