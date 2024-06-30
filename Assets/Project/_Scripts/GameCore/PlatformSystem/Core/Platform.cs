using DG.Tweening;
using Project._Scripts.GameCore.PlatformSystem.System;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.Core
{
  public class Platform : MonoBehaviour
  {
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;

    private const float SnapDuration = .05f;
    [Range(1f,5f)]public float TransitionDuration = 2f;
    public Tweener TransitionTween;
    

    private void Start()
    {
      TransitionTween = transform.DOMoveX(-transform.position.x, TransitionDuration)
        .SetEase(Ease.Linear)
        .SetLoops(-2, LoopType.Yoyo);
    }

    public void SnapPlatform()
    {
      transform.DOMoveX(PlatformController.PreviousPlatform.position.x, SnapDuration)
        .SetEase(Ease.InOutSine)
        .OnComplete(() => PlatformController.OnPlatformSnappedHandler(this));
    }

    public void RescalePlatform(float distance)
    {
      transform.localScale = new Vector3(transform.localScale.x - Mathf.Abs(distance), transform.localScale.y, transform.localScale.z);
      transform.position = new Vector3(transform.position.x - (distance / 2), transform.position.y, transform.position.z);
    }
    
    public void KillPlatform()
    {
      var distance = GetDistanceFromPlatform();

      if (Mathf.Abs(distance) <= PlatformController.TOLERANCE)
      {
        SnapPlatform();
      }

      else
      {
        PlatformController.SnappedPlatformCount = 0;
        RescalePlatform(distance);
        SpawnDropCube(distance);
      }
      
      PlatformController.OnPlatformSpawnedHandler();
    }

    private float GetDistanceFromPlatform()
    {
      float currentPlatformPositionX = transform.position.x;
      float previousPlatformPositionX = PlatformController.PreviousPlatform.transform.position.x;
      return currentPlatformPositionX - previousPlatformPositionX;
    }

    private void SpawnDropCube(float differenceXPosition)
    {
      var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
      int multiplier = differenceXPosition >= 0 ? 1 : -1;
      platform.transform.localScale = new Vector3(Mathf.Abs(differenceXPosition), transform.localScale.y, transform.localScale.z);
      platform.transform.position = new Vector3(transform.position.x + multiplier*transform.localScale.x/2 + (differenceXPosition/2), transform.position.y, transform.position.z);

      platform.AddComponent<Rigidbody>();
      
      
      Destroy(this);
    }

    public void IncreaseScale()
    {
      var currentScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
      var scaleTreshold = .25f;
      var scaleAmount = currentScale.x < currentScale.z ? (currentScale.z - currentScale.x > scaleTreshold ? scaleTreshold : currentScale.z - currentScale.x) : 0f;
      Debug.Log(scaleAmount);
      if(Mathf.Approximately(currentScale.x, currentScale.z)) return;
      currentScale.x += scaleAmount;
      transform.localScale = currentScale;
    }
  }
}
