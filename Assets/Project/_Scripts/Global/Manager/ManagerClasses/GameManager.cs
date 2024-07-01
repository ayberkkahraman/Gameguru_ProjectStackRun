using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.Global.Manager.ManagerClasses
{
  [DefaultExecutionOrder(960)]
  public class GameManager : MonoBehaviour
  {
    private void Start() => GameManagerData.OnGameStartedHandler();
    private void OnDisable() => GameManagerData.CleanUp();
  }
}
