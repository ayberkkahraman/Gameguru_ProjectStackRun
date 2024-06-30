using Project._Scripts.GameCore.InteractionSystem.Interactables.Elements;
using Project._Scripts.GameCore.PlatformSystem.System;
using Project._Scripts.Global.Manager.ManagerClasses;
using UnityEngine;

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
    private void Awake()
    {
      SetLevelSize();
      GenerateFinishObstacle();
    }
    private void OnEnable() => GameManager.OnGameStartedHandler += ResetPlatformCount;
    private void OnDisable() => GameManager.OnGameStartedHandler -= ResetPlatformCount;
    
    private void GenerateFinishObstacle() => Instantiate(FinishObstaclePrefab, new Vector3(0, 0, PlatformController.SPlatformControllerData.PlatformPrefab.transform.localScale.z * PlatformCount), Quaternion.identity);
    private void SetLevelSize() => PlatformCount = Random.Range(PlatformCountLimits.x, PlatformCountLimits.y+1);
    private void ResetPlatformCount() => CurrentPlatformCount = 1;
    public static void IncreasePlatformCount() => CurrentPlatformCount++;
  }
}
