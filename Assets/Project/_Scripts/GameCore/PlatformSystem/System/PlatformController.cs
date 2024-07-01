using System.Collections.Generic;
using DG.Tweening;
using Project._Scripts.GameCore.MapGeneration.System;
using Project._Scripts.GameCore.PlatformSystem.Core;
using Project._Scripts.GameCore.PlatformSystem.EventDatas;
using Project._Scripts.GameCore.PlatformSystem.ScriptableObjects;
using Project._Scripts.Global.Manager.ManagerClasses;
using UnityEngine;
using Zenject;

namespace Project._Scripts.GameCore.PlatformSystem.System
{
  [DefaultExecutionOrder(750)]
  public class PlatformController : MonoBehaviour
  {
    #region Dependency Injection
    private PoolManager _poolManager;
    [Inject]
    public void Construct(PoolManager poolManager) => _poolManager = poolManager;
    #endregion
    
    #region Components
    public static Platform CurrentPlatform;
    public static Transform PreviousPlatform;

    public PlatformControllerData PlatformControllerData;
    public static PlatformControllerData SPlatformControllerData;
    #endregion
    
    #region Fields
    private List<Platform> _platforms;
    private int _platformCount; //The total platform count
    public static int SnappedPlatformCount; //Snapped platform count is necessary for combo check
   
    private static bool _isComboActive;
    public static bool IsComboActive
    {
      get => SnappedPlatformCount >= SPlatformControllerData.SnapCombo;
      private set => _isComboActive = value;
    }
    #endregion

    #region Delegates
    public delegate void OnPlatformSnapped(Platform platform);
    public static OnPlatformSnapped OnPlatformSnappedHandler;
   
    public delegate void OnPlatformKilled(Platform platform);
    public static OnPlatformKilled OnPlatformKilledHandler;
   
    public delegate void OnPlatformSpawned(bool reset = false, float scale = 0f);
    public static OnPlatformSpawned OnPlatformSpawnedHandler;
    #endregion

    #region Unity Functions
    private void Awake() => InitializeData();
    private void OnEnable() => Initialize();
    private void OnDisable() => DeInitialize();
    private void Start() => OnPlatformSpawnedHandler();
    private void Update() {if (Input.GetMouseButtonDown(0)) OnPlatformKilledHandler(CurrentPlatform);}
    #endregion
    
    #region Initialization / DeInitialization
    internal void InitializeData() => SPlatformControllerData = PlatformControllerData;
    private void Initialize()
    {
      //-------------------------------------------PRE INITIALIZATION-------------------------------------------
      _platforms = new List<Platform>();
      IsComboActive = false;
      SnappedPlatformCount = 0;
      ColorEventData.CurrentColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
      //-------------------------------------------PRE INITIALIZATION-------------------------------------------
      
      
      OnPlatformSpawnedHandler += (reset,_) => SpawnPlatform(reset);

      OnPlatformKilledHandler += KillPlatform;
      OnPlatformKilledHandler += (_) => LevelGenerator.IncreasePlatformCount();

      OnPlatformSnappedHandler += (_) => IncreaseSnappedPlatformCount();
      OnPlatformSnappedHandler += CheckSnappedPlatforms;
    }

    private void DeInitialize()
    {
      OnPlatformKilledHandler = null;
      OnPlatformSpawnedHandler = null;
      OnPlatformSnappedHandler = null;
    }
    #endregion

    #region Platform Handling
    /// <summary>
    /// Spawns a new platform according the scale of previous platform
    /// </summary>
    /// <param name="reset"></param>
    /// <param name="scale"></param>
    private void SpawnPlatform(bool reset = false, float scale = 0f)
    {
      if (!LevelGenerator.CanGeneratePlatform) return;

      //---------------GENERATING NEW PLATFORM---------------
      Platform platform = NewPlatform(reset, scale);
      platform.RunPlatform();
      platform.SetPlatformColor(reset);
      //-----------------------------------------------------

      
      PreviousPlatform = transform.GetChild(1);
      CurrentPlatform = platform;
      _platformCount++;

      //---------------POOLING OPTIMIZATION---------------
      if (_platformCount <= 10)
        return;
      Platform platformToDestroy = _platforms[0];
      _poolManager.DestroyPoolObject(platformToDestroy);
      _platforms.Remove(platformToDestroy);
      //---------------------------------------------------
    }

    private Platform NewPlatform(bool reset, float scale)
    {
      Transform previousPlatformTransform = transform.GetChild(0);
      previousPlatformTransform.TryGetComponent(out Platform previousPlatform);
      
      //"multiplier" checks if the new platform should spawn from left of the character or from right
      int multiplier = (_platformCount % 2 == 0) ? 1 : -1;

      //----------------------------------GENERATING PLATFORM TRANSFORM PROPERTIES----------------------------------
      Vector3 position = new Vector3(
        multiplier * -6f,
        previousPlatformTransform.position.y,
        previousPlatformTransform.position.z + previousPlatformTransform.localScale.z
      );

      var comboScale = previousPlatform is not null ? (IsComboActive ? previousPlatform.ScaleAmount : 0f) : 0f;
      
      Vector3 newScale = reset
        ? PlatformControllerData.PlatformPrefab.transform.localScale
        : new Vector3(
          previousPlatformTransform.localScale.x + scale + comboScale,
          PlatformControllerData.PlatformPrefab.transform.localScale.y,
          PlatformControllerData.PlatformPrefab.transform.localScale.z
        );
      
      //-----------------------------------------------------------------------------------------------------------

      //---------------------------------------------GENERATING PLATFORM---------------------------------------------
      Platform platform = _poolManager.SpawnFromPool<Platform>("Platform", position, Quaternion.identity);
      platform.transform.parent = transform;

      platform.transform.localScale = newScale;
      platform.transform.SetSiblingIndex(0);
      
      _platforms.Add(platform);
      //-------------------------------------------------------------------------------------------------------------
      
      return platform;
    }
    
    /// <summary>
    /// Kills the marked platform before the new platform initializes
    /// </summary>
    /// <param name="platform"></param>
    internal void KillPlatform(Platform platform) => platform.TransitionTween.Kill();
    #endregion

    #region Combo Handling
    internal void IncreaseSnappedPlatformCount() => SnappedPlatformCount++;
    internal void CheckSnappedPlatforms(Platform platform)
    {
      //Check the distance for activating the combo sequence
      if(Mathf.Approximately(platform.transform.localScale.x, PlatformControllerData.PlatformPrefab.transform.localScale.x)) return;

      if (SnappedPlatformCount < PlatformControllerData.SnapCombo) return; 
      platform.IncreasePlatformScale();
    }
    #endregion
  }
}