using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.GameCore.CharacterController.Initializer
{
  public class CharacterInitializer : MonoBehaviour
  {
    private Animator _animator;
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private void OnEnable() => InitializeController();
    private void InitializeController()
    {
      _animator = GetComponent<Animator>();
      
      GameManagerData.OnGameStartedHandler += () => SetController(true);
      GameManagerData.OnLevelCompletedHandler += () => SetController(false);
      GameManagerData.OnLevelFailedHandler += () => SetController(false);
      GameManagerData.OnCharacterRestrictedHandler += () => SetController(false);
    }

    private void SetController(bool condition)
    {
      SetToIdle();
    }

    private void SetToIdle()
    {
      _animator.SetBool(IsGrounded, true);
      _animator.SetBool(IsMoving, false);
    }
  }
}
