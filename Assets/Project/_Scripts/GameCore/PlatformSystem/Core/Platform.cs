﻿using DG.Tweening;
using Project._Scripts.GameCore.PlatformSystem.System;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.Core
{
  public class Platform : MonoBehaviour
  {
    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    
    [Range(1f,5f)]public float TransitionDuration = 2f;
    public Tweener TransitionTween;

    private float _scaleAmount;
    private void Start()
    {
      TransitionTween = transform.DOMoveX(-transform.position.x, TransitionDuration)
        .SetEase(Ease.Linear)
        .SetLoops(-2, LoopType.Yoyo);
    }

    public void SnapPlatform()
    {
      transform.DOMoveX(PlatformController.PreviousPlatform.position.x, PlatformController.SPlatformControllerData.SnapDuration)
        .SetEase(PlatformController.SPlatformControllerData.SnapCurve);
      PlatformController.OnPlatformSnappedHandler(this);
    }

    public void RescalePlatform(float distance)
    {
      transform.localScale = new Vector3(transform.localScale.x - Mathf.Abs(distance), transform.localScale.y, transform.localScale.z);
      transform.position = new Vector3(transform.position.x - (distance / 2), transform.position.y, transform.position.z);
    }
    
    public void KillPlatform()
    {
      var distance = GetDistanceFromPlatform();

      if (Mathf.Abs(distance) <= PlatformController.SPlatformControllerData.SnapTolerance)
      {
        SnapPlatform();
      }

      else
      {
        PlatformController.SnappedPlatformCount = 0;
        RescalePlatform(distance);
        SpawnDropCube(distance);
      }
      
      PlatformController.OnPlatformSpawnedHandler(GetScaleAmount());
    }

    private float GetDistanceFromPlatform() => transform.position.x - PlatformController.PreviousPlatform.transform.position.x;

    private void SpawnDropCube(float differenceXPosition)
    {
      var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
      int multiplier = differenceXPosition >= 0 ? 1 : -1;
      platform.transform.localScale = new Vector3(Mathf.Abs(differenceXPosition), transform.localScale.y, transform.localScale.z);
      platform.transform.position = new Vector3(transform.position.x + multiplier*transform.localScale.x/2 + (differenceXPosition/2), transform.position.y, transform.position.z);

      platform.AddComponent<Rigidbody>();
      
      
      Destroy(this);
    }

    private float GetScaleAmount()
    {
      var scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
      var scaleTreshold = PlatformController.SPlatformControllerData.ScaleAmount;

      var defaultScale = PlatformController.SPlatformControllerData.PlatformPrefab.transform.localScale.x;
      var scaleAmount = scale.x < defaultScale ? (defaultScale - scale.x > scaleTreshold ? scaleTreshold : defaultScale - scale.x) : 0f;

      scale.x += scaleAmount;
      return scale.x;
    }

    public void IncreaseScale()
    {
      transform.DOScaleX(GetScaleAmount(), PlatformController.SPlatformControllerData.SnapDuration).SetEase(PlatformController.SPlatformControllerData.ScaleCurve);

    }
  }
}
