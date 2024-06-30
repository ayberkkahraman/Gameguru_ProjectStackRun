using Project._Scripts.GameCore.PlatformSystem.Core;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.ScriptableObjects
{
  [CreateAssetMenu(fileName = "PlatformControllerData", menuName = "GameCore/PlatformSystem/PlatformControllerData")]
  public class PlatformControllerData : ScriptableObject
  {
    public Platform PlatformPrefab;
    [Space]
    [Header("Attributes")]
    [Range(0f, 2f)] public float SnapTolerance = .1f;
    [Range(0f, 1f)] public float SnapDuration = .15f;
    [Range(0f, 2f)] public float ScaleAmount = .4f;
    [Range(0f, 1f)] public float ScaleDuration = .15f;

    [Space]
    public AnimationCurve SnapCurve;
    public AnimationCurve ScaleCurve;
  }
}
