using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project._Scripts.Global.Manager.ManagerClasses
{
  public class StageManager : MonoBehaviour
  {
    public bool CanRestart = true;
    private static int GetNextSceneIndex() => SceneManager.GetActiveScene().buildIndex + 1;
    private static int GetCurrentSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    public static void NextStage() => SceneManager.LoadScene(GetNextSceneIndex());
    public static void RestartStage() => SceneManager.LoadScene(GetCurrentSceneIndex());
    public void UIF_RestartStage() => RestartStage();
  }
}
