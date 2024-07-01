using DG.Tweening;
using Project._Scripts.GameCore.PlatformSystem.EventDatas;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Library.Audio.Interfaces;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.Core
{
  public class Platform : MonoBehaviour, IAudioOwner
  {
    #region Components
    public Material Material { get; set; }
    public IAudioOwner AudioOwner { get; set; }
    #endregion
    
    #region Fields
    public Tweener TransitionTween { get; set; } // Handles the horizontal motion of the platform

    public float ScaleAmount => GetScaleAmount(); // Necessary for the next platform that will be spawned
    #endregion

    #region Unity Functions
    private void Awake() => InitializationComponents();
    #endregion

    #region Initialization
    private void InitializationComponents()
    {
      AudioOwner = this;
      Material = GetComponent<MeshRenderer>().material;
    }
    #endregion

    #region Platform Behaviour
    /// <summary>
    /// Handles the horizontal motion of the platform
    /// </summary>
    public void RunPlatform()
    {
      TransitionTween = transform.DOMoveX(-transform.position.x, PlatformController.SPlatformControllerData.PlatformTransitionDuration)
        .SetEase(Ease.Linear)
        .SetLoops(-2, LoopType.Yoyo)
        .OnKill(KillPlatform);
    }
    
    /// <summary>
    /// Kills the horizontal motion of the platform
    /// </summary>
    public void KillPlatform()
    {
      var distance = GetDistanceFromPlatform();

      //Checks if the current position of platform allows to the snapping
      if (Mathf.Abs(distance) <= PlatformController.SPlatformControllerData.SnapTolerance)
      {
        AudioOwner.Play("MusicNote", true, .1f);
        SnapPlatform();
      }

      else
      {
        //Checks if the platform is touching to previous one or not
        if (Mathf.Abs(distance) >= PlatformController.PreviousPlatform.transform.localScale.x)
        {
          gameObject.AddComponent<Rigidbody>();
          return;
        }
        
        AudioOwner.Play("MusicNote", false, .1f);
        PlatformController.SnappedPlatformCount = 0;
        RescalePlatform(distance);
        SpawnFallingPlatform(distance);
      }
      
      //Spawns a new platform after all events be done
      PlatformController.OnPlatformSpawnedHandler(false, PlatformController.IsComboActive ? GetScaleAmount() : 0f);
    }
    
    /// <summary>
    /// Snaps the platform with the previous platform
    /// </summary>
    public void SnapPlatform()
    {
      transform.DOMoveX(PlatformController.PreviousPlatform.position.x, PlatformController.SPlatformControllerData.SnapDuration)
        .SetEase(PlatformController.SPlatformControllerData.SnapCurve);
      
      PlatformController.OnPlatformSnappedHandler(this);
    }
    
    /// <summary>
    /// Updates the scale of the platform
    /// </summary>
    /// <param name="distance"></param>
    public void RescalePlatform(float distance)
    {
      transform.localScale = new Vector3(transform.localScale.x - Mathf.Abs(distance), transform.localScale.y, transform.localScale.z);
      transform.position = new Vector3(transform.position.x - (distance / 2), transform.position.y, transform.position.z);
    }
    
    /// <summary>
    /// Spawns the falling platform if the platform has been sliced
    /// </summary>
    /// <param name="differenceXPosition"></param>
    private void SpawnFallingPlatform(float differenceXPosition)
    {
      var platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
      int multiplier = differenceXPosition >= 0 ? 1 : -1;
      platform.transform.localScale = new Vector3(Mathf.Abs(differenceXPosition), transform.localScale.y, transform.localScale.z);
      platform.transform.position = new Vector3(transform.position.x + multiplier*transform.localScale.x/2 + (differenceXPosition/2), transform.position.y, transform.position.z);

      //Adds physics motion
      platform.GetComponent<MeshRenderer>().material.color = Material.color;
      platform.AddComponent<BoxCollider>().isTrigger = true;
      platform.AddComponent<Rigidbody>();
    }
    
    /// <summary>
    /// Increases the scale of the platform according to snap combo
    /// </summary>
    public void IncreasePlatformScale() => transform.DOScaleX
      (
        GetScaleAmount() + transform.localScale.x, 
        PlatformController.SPlatformControllerData.SnapDuration
        )
      .SetEase(PlatformController.SPlatformControllerData.ScaleCurve);
    
    /// <summary>
    /// Sets the platform color
    /// </summary>
    /// <param name="reset"></param>
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
    
    /// <summary>
    /// Necessary for the platforms which will be spawned after the "Kill" section
    /// </summary>
    /// <returns></returns>
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