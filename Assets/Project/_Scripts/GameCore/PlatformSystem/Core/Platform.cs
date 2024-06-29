using DG.Tweening;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Global.Manager.Core;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.Core
{
  public class Platform : MonoBehaviour
  {
    [Range(1f,5f)]public float TransitionDuration = 2f;
    private Tweener _transitionTween;

    private void OnEnable()
    {
      // ManagerCore.Instance.GetInstance<PlatformController>().CurrentPlatform = this;
      PlatformController.OnPlatformKilledHandler += (_) =>
      {
        _transitionTween.Kill();
        SlicePlatform();
      };
    }

    private void OnDisable()
    {
      PlatformController.OnPlatformKilledHandler -= (_) =>
      {
        _transitionTween.Kill();
        SlicePlatform();
      };
    }

    private void Start()
    {
      _transitionTween = transform.DOMoveX(-transform.position.x, TransitionDuration)
        .SetEase(Ease.Linear)
        .SetLoops(-2, LoopType.Yoyo);
    }

    private void SlicePlatform()
    {
      float currentPlatformPositionX = transform.position.x;
      float previousPlatformPositionX = ManagerCore.Instance.GetInstance<PlatformController>().PreviousPlatform.transform.position.x;

      var distance = currentPlatformPositionX - previousPlatformPositionX;
      int multiplier = distance >= 0 ? 1 : -1;

      transform.localScale = new Vector3(transform.localScale.x - Mathf.Abs(distance), transform.localScale.y, transform.localScale.z);
      transform.position = new Vector3(transform.position.x - (distance / 2), transform.position.y, transform.position.z);
      SpawnDropCube(distance);
    }

    private void SpawnDropCube(float differenceXPosition)
    {
      var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
      int multiplier = differenceXPosition >= 0 ? 1 : -1;
      platform.transform.localScale = new Vector3(differenceXPosition, transform.localScale.y, transform.localScale.z);
      Transform previousPlatform = ManagerCore.Instance.GetInstance<PlatformController>().PreviousPlatform.transform;
      platform.transform.position = new Vector3((previousPlatform.localScale.x/2 + multiplier*differenceXPosition/2)*multiplier, transform.position.y, transform.position.z);

    }
  }
}
