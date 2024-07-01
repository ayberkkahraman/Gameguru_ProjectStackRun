using System;
using Project._Scripts.GameCore.InteractionSystem.Interactables.Elements;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Global.Manager.Core;
using Project._Scripts.Global.Manager.ManagerClasses;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project._Scripts.GameCore.MapGeneration
{
  [DefaultExecutionOrder(920)]
  public class LevelGenerator : MonoBehaviour
  {
    public Finish FinishObstaclePrefab;
    public Vector2Int PlatformCountLimits;
    public static int PlatformCount;
    public static int CurrentPlatformCount;
    public static bool CanGeneratePlatform => CurrentPlatformCount < PlatformCount - 1;
    public Finish CurrentFinish { get; set; }
    // private Vector3 _levelInitialPosition;

    private delegate void GenerateLevel(Vector3 position);
    private GenerateLevel _generateLevelHandler;
    
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

    private void GenerateFinishObstacle(Vector3 position) => CurrentFinish = Instantiate(FinishObstaclePrefab, position, Quaternion.identity);
    private void SetLevelSize() => PlatformCount = Random.Range(PlatformCountLimits.x, PlatformCountLimits.y+1);
    private void ResetPlatformCount() => CurrentPlatformCount = 1;
    public static void IncreasePlatformCount() => CurrentPlatformCount++;
    public void UIF_GenerateLevel()
    {
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
      
      GameManagerData.OnGameStartedHandler();
      PlatformController.OnPlatformSpawnedHandler();
      
      
      _generateLevelHandler?.Invoke(position);
    }
  }
}
