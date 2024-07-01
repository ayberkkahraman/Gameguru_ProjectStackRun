using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;
namespace Project._Scripts.Global.Manager.ManagerClasses
{
  [DefaultExecutionOrder(960)]
  public class GameManager : MonoBehaviour
  {
    public void Start() => GameManagerData.OnGameStartedHandler();
  }
}
