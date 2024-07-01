using DG.Tweening;
using Project._Scripts.GameCore.MapGeneration;
using Project._Scripts.GameCore.PlatformSystem.Core;
using Project._Scripts.GameCore.PlatformSystem.EventDatas;
using Project._Scripts.GameCore.PlatformSystem.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.System
{
  [DefaultExecutionOrder(750)]
  public class PlatformController : MonoBehaviour
  {
    #region Components
    public static Platform CurrentPlatform;
    public static Transform PreviousPlatform;

    public PlatformControllerData PlatformControllerData;
    public static PlatformControllerData SPlatformControllerData;
    #endregion
    
    #region Fields
    private int _platformCount;
    public static int SnappedPlatformCount;
    public static int ComboCount;
   
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
   
    public delegate void OnPlatformSpawned(float scale = 0f);
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
      IsComboActive = false;
      SnappedPlatformCount = 0;
      ColorEventData.CurrentColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
     
      OnPlatformSpawnedHandler += SpawnPlatform;
      
      OnPlatformKilledHandler += KillPlatform;
      OnPlatformKilledHandler += (_) => LevelGenerator.IncreasePlatformCount();

      OnPlatformSnappedHandler += (_) => IncreaseSnappedPlatformCount();
      OnPlatformSnappedHandler += CheckSnappedPlatforms;
      OnPlatformSnappedHandler += (_) => PlayAudio();

      // GameManagerData.OnLevelSuccessHandler += () => Destroy(this);
    }

    private void DeInitialize()
    {
      OnPlatformKilledHandler -= KillPlatform;
      OnPlatformKilledHandler += (_) => LevelGenerator.IncreasePlatformCount();
      
      OnPlatformSpawnedHandler -= SpawnPlatform;
      
      // GameManagerData.OnLevelSuccessHandler -= () => Destroy(this);
    }
    #endregion

    #region Platform Handling
    private void SpawnPlatform(float scale = 0f)
    {
      if(!LevelGenerator.CanGeneratePlatform) return;

      
      PreviousPlatform = transform.GetChild(0);
      
      int multiplier = (_platformCount % 2 == 0) ? 1 : -1;
    
      Vector3 position = new Vector3(
        multiplier * -6f, 
        PreviousPlatform.position.y, 
        PreviousPlatform.position.z + PreviousPlatform.localScale.z
      );
    
      Platform platform = Instantiate(
        PlatformControllerData.PlatformPrefab, 
        position, 
        Quaternion.identity, 
        transform
      );
      ColorEventData.SetNextColor(platform.Material);
    
      platform.transform.localScale = new Vector3(
        PreviousPlatform.localScale.x + scale, 
        PlatformControllerData.PlatformPrefab.transform.localScale.y, 
        PlatformControllerData.PlatformPrefab.transform.localScale.z
      );
    
      platform.transform.SetSiblingIndex(0);
    
      PreviousPlatform = transform.GetChild(1);
      CurrentPlatform = platform;

      _platformCount++;
    }
    internal void KillPlatform(Platform platform) => platform.TransitionTween.Kill();
    #endregion

    #region Combo Handling
    internal void IncreaseSnappedPlatformCount() => SnappedPlatformCount++;
    internal void CheckSnappedPlatforms(Platform platform)
    {
      if(Mathf.Approximately(platform.transform.localScale.x, PlatformControllerData.PlatformPrefab.transform.localScale.x)) return;

      if (SnappedPlatformCount < PlatformControllerData.SnapCombo) return; 
      platform.IncreaseScale();
    }

    internal void PlayAudio(){}
    #endregion
  }
}