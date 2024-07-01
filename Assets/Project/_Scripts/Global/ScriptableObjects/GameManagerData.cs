using UnityEngine;

namespace Project._Scripts.Global.ScriptableObjects
{
  [CreateAssetMenu(fileName = "GameManagerData", menuName = "Global/Datas/GameManagerData")]
  public class GameManagerData : ScriptableObject
  {
    public delegate void OnLevelFailed();
    public delegate void OnLevelCompleted();
    public delegate void OnGameStarted();

    public static OnLevelFailed OnLevelFailHandler;
    public static OnLevelCompleted OnLevelSuccessHandler;
    public static OnGameStarted OnGameStartedHandler;
    
    public static void CleanUp()
    {
      OnLevelFailHandler = null;
      OnLevelSuccessHandler = null;
      OnGameStartedHandler = null;
    }
  }
}
