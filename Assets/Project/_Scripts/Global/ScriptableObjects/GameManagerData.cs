using UnityEngine;

namespace Project._Scripts.Global.ScriptableObjects
{
  [CreateAssetMenu(fileName = "GameManagerData", menuName = "Global/Datas/GameManagerData")]
  public class GameManagerData : ScriptableObject
  {
    public enum State{Running, Completed, Failed}
    public static State GameState { get; set; }
    public delegate void OnLevelFailed();
    public delegate void OnLevelCompleted();
    public delegate void OnGameStarted();
    public delegate void OnCharacterRestricted();

    public static OnLevelFailed OnLevelFailedHandler;
    public static OnLevelCompleted OnLevelCompletedHandler;
    public static OnGameStarted OnGameStartedHandler;
    public static OnCharacterRestricted OnCharacterRestrictedHandler;

    
    public static void CleanUp()
    {
      GameState = State.Running;
      OnLevelFailedHandler = null;
      OnLevelCompletedHandler = null;
      OnGameStartedHandler = null;
      OnCharacterRestrictedHandler = null;
    }
  }
}
