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
    #endregion

    #region Unity Functions
    private void Awake() => InitializeComponents();
    private void OnEnable() => InitializeDelegates();
    private void OnDisable() => DeInitializeDelegates();
    #endregion

    #region Initialization /DeInitialization
    private void InitializeComponents() => _animator = GetComponent<Animator>();
    private void InitializeDelegates() => GameManagerData.OnLevelCompletedHandler += PlayDanceAnimation;
    private void DeInitializeDelegates() => GameManagerData.OnLevelCompletedHandler -= PlayDanceAnimation;
    #endregion

    #region Animation Behaviours
    public void PlayDanceAnimation() => _animator.SetTrigger(Dance);
    #endregion
    
  }
}
