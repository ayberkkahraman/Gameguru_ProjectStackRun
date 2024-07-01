using Project._Scripts.GameCore.InteractionSystem.Interactables.Elements;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project._Scripts.GameCore.MapGeneration.System
{
  [DefaultExecutionOrder(920)]
  public class LevelGenerator : MonoBehaviour
  {
    #region Depencency Injection
    public CameraManager CameraManager { get; set; }
    private PoolManager _poolManager;
    [Inject]
    public void Construct(CameraManager cameraManager, PoolManager poolManager)
    {
      CameraManager = cameraManager;
      _poolManager = poolManager;
    }
    #endregion

    #region Fields
    public Finish CurrentFinish { get; set; }
    public static Finish SCurrentFinish;
    
    public Vector2Int PlatformCountLimits;
    
    public static int PlatformCount;
    public static int CurrentPlatformCount;
    public static bool CanGeneratePlatform => CurrentPlatformCount < PlatformCount - 1;
    #endregion

    #region Delegates
    private delegate void GenerateLevel(Vector3 position);
    private GenerateLevel _generateLevelHandler;
    #endregion

    #region Unity Functions
    private void OnEnable()
    {
      GameManagerData.OnGameStartedHandler += ResetPlatformCount;
      
      _generateLevelHandler += GenerateFinishObstacle;
      _generateLevelHandler += (_) => ResetPlatformCount();
    }
    private void OnDisable()
    {
      GameManagerData.OnGameStartedHandler -= ResetPlatformCount;
      
      _generateLevelHandler -= GenerateFinishObstacle;
      _generateLevelHandler -= (_) => ResetPlatformCount();
    }
    private void Start()
    {
      SetLevelSize();
      GenerateFinishObstacle(new Vector3(0, 0, PlatformController.SPlatformControllerData.PlatformPrefab.transform.localScale.z * PlatformCount));
      ResetPlatformCount();
    }
    #endregion

    #region Level Generation
    /// <summary>
    /// Generates the new Finish platform
    /// </summary>
    /// <param name="position"></param>
    private void GenerateFinishObstacle(Vector3 position)
    {
      CurrentFinish = _poolManager.SpawnFromPool<Finish>("Finish",position, Quaternion.identity);
      SCurrentFinish = CurrentFinish;
      CurrentFinish.CameraManager = CameraManager;
    }
    
    /// <summary>
    /// Allows to generate the next level with the UI Button -> UIManager.StartButton
    /// </summary>
    public void UIF_GenerateLevel()
    {
      //Generates a placeholder platform for the initial position
      var initialPlatform = new GameObject
      {
        transform =
        {
          position = new Vector3(CurrentFinish.CharacterPositionReference.transform.position.x, 0f, CurrentFinish.transform.position.z + CurrentFinish.transform.localScale.z-1),
          parent = PlatformController.PreviousPlatform.transform.parent
        },
        name = "InitialPlatform"
      };

      initialPlatform.transform.SetSiblingIndex(0);
      
      SetLevelSize();
      
      var position = new Vector3
      (
        CurrentFinish.CharacterPositionReference.transform.position.x,
        0f,
        (CurrentFinish.transform.position.z + CurrentFinish.transform.localScale.z * PlatformCount)
      );

      Destroy(CurrentFinish);
      
      _generateLevelHandler?.Invoke(position);
      
      GameManagerData.OnGameStartedHandler();
      PlatformController.OnPlatformSpawnedHandler(true);
    }
    #endregion

    #region Level Behaviours
    /// <summary>
    /// Sets the platform count of the new level
    /// </summary>
    private void SetLevelSize() => PlatformCount = Random.Range(PlatformCountLimits.x, PlatformCountLimits.y+1);
    /// <summary>
    /// Resets the platform count of the current level
    /// </summary>
    private void ResetPlatformCount() => CurrentPlatformCount = 1;
    /// <summary>
    /// Increases the platform count of the current level
    /// </summary>
    public static void IncreasePlatformCount() => CurrentPlatformCount++;
    #endregion

    
  }
}
