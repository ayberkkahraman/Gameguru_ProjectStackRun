using Project._Scripts.GameCore.CharacterController.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.GameCore.CharacterController.Core
{
  public class CharacterController : MonoBehaviour
  {
    public CharacterLocomotionData CharacterLocomotionData;
    private Animator _animator;
    private UnityEngine.CharacterController _characterController;
    
    private float _movementSpeed;
    private float _transitionSpeed;

    public bool Run;
    private void Awake()
    {
      InitializeComponents();
      InitializeLocomotion();
    }

    private void Update()
    {
      if(!Run) return;
      
      MoveCharacter();
    }

    private void InitializeLocomotion()
    {
      _movementSpeed = CharacterLocomotionData.MovementSpeed;
      _transitionSpeed = CharacterLocomotionData.TransitionSpeed;
    }

    private void InitializeComponents()
    {
      _animator = GetComponent<Animator>();
      _characterController = GetComponent<UnityEngine.CharacterController>();
    }

    private void MoveCharacter()
    {
      _characterController.Move(transform.forward * (_movementSpeed * Time.deltaTime));
    }
  }
}
