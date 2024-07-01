using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project._Scripts.Global.Manager.ManagerClasses
{
  public class StageManager : MonoBehaviour
  {
    private static int GetCurrentSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    public static void RestartStage() => SceneManager.LoadScene(GetCurrentSceneIndex());
    public void UIF_RestartStage() => RestartStage();
  }
}
