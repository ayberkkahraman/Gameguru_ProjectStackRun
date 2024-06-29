using UnityEngine;

namespace Project._Scripts.GameCore.CharacterController.ScriptableObjects
{
  [CreateAssetMenu(fileName = "CharacterLocomotionData", menuName = "GameCore/CharacterController/CharacterLocomotionData")]
  public class CharacterLocomotionData : ScriptableObject
  {
    [Range(1f, 10f)] public float MovementSpeed = 2f;
    [Range(.1f, 5f)] public float TransitionSpeed = 1f;
  }
}
