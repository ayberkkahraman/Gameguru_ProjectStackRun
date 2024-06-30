using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project._Scripts.Global.Manager.ManagerClasses
{
  [DefaultExecutionOrder(960)]
  public class GameManager : MonoBehaviour
  {
    public delegate void OnGameStarted();
    public static OnGameStarted OnGameStartedHandler;
    public void Start() => OnGameStartedHandler();
  }
}
