using DG.Tweening;
using Project._Scripts.GameCore.PlatformSystem.EventDatas;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Global.Manager.Core;
using Project._Scripts.Library.Audio.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project._Scripts.GameCore.PlatformSystem.Core
{
  public class Platform : MonoBehaviour, IAudioOwner
  {
    #region Components
    public Material Material { get; set; }
    public IAudioOwner AudioOwner { get; set; }
    #endregion
    
    #region Fields
    public Tweener TransitionTween { get; set; }

    public float ScaleAmount => GetScaleAmount();
    #endregion

    #region Unity Functions
    private void Awake() => InitializationComponents();
    private void Start() => RunPlatform();
    #endregion

    #region Initialization
    private void InitializationComponents()
    {
      AudioOwner = this;
      Material = GetComponent<MeshRenderer>().material;
    }
    #endregion

    #region Platform Behaviour
    public void RunPlatform()
    {
      TransitionTween = transform.DOMoveX(-transform.position.x, PlatformController.SPlatformControllerData.PlatformTransitionDuration)
        .SetEase(Ease.Linear)
        .SetLoops(-2, LoopType.Yoyo)
        .OnKill(KillPlatform);
    }
    
    public void KillPlatform()
    {
      var distance = GetDistanceFromPlatform();

      if (Mathf.Abs(distance) <= PlatformController.SPlatformControllerData.SnapTolerance)
      {
        AudioOwner.Play("MusicNote", true, .1f);
        SnapPlatform();
      }

      else
      {
        AudioOwner.Play("MusicNote", false, .1f);
        PlatformController.SnappedPlatformCount = 0;
        RescalePlatform(distance);
        SpawnFallingPlatform(distance);
      }

      PlatformController.OnPlatformSpawnedHandler(false, PlatformController.IsComboActive ? GetScaleAmount() : 0f);
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
    
    private void SpawnFallingPlatform(float differenceXPosition)
    {
      var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
      int multiplier = differenceXPosition >= 0 ? 1 : -1;
      platform.transform.localScale = new Vector3(Mathf.Abs(differenceXPosition), transform.localScale.y, transform.localScale.z);
      platform.transform.position = new Vector3(transform.position.x + multiplier*transform.localScale.x/2 + (differenceXPosition/2), transform.position.y, transform.position.z);

      platform.GetComponent<MeshRenderer>().material.color = Material.color;
      platform.AddComponent<Rigidbody>();
      
      
      Destroy(this);
    }
    
    public void IncreaseScale() => transform.DOScaleX
      (
        GetScaleAmount() + transform.localScale.x, 
        PlatformController.SPlatformControllerData.SnapDuration
        )
      .SetEase(PlatformController.SPlatformControllerData.ScaleCurve);
    
    public void SetPlatformColor(bool reset)
    {
      if (reset)
      {
        ColorEventData.CurrentColor = ColorEventData.RandomColor();
        ColorEventData.TargetColor = ColorEventData.RandomColor();
        Material.color = ColorEventData.CurrentColor;
      }
      else
      {
        ColorEventData.SetNextColor(Material);
      }
    }
    #endregion

    #region Neccessary Calculations
    private float GetDistanceFromPlatform() => transform.position.x - PlatformController.PreviousPlatform.transform.position.x;
    
    private float GetScaleAmount()
    {
      var scale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
      var scaleTreshold = PlatformController.SPlatformControllerData.ScaleAmount;

      var defaultScale = PlatformController.SPlatformControllerData.PlatformPrefab.transform.localScale.x;
      var scaleAmount = scale.x < defaultScale ? (defaultScale - scale.x > scaleTreshold ? scaleTreshold : defaultScale - scale.x) : 0f;

      return scaleAmount;
    }
    #endregion
  }
}