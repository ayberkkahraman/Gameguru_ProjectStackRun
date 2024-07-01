using System;
using System.Collections;
using Project._Scripts.Global.Manager.Core;
using UnityEngine;

namespace Project._Scripts.Global.SubSystem
{
  public class SubSystem : MonoBehaviour
  {
    public static IEnumerator Invoke(Action action, float delay)
    {
      yield return new WaitForSeconds(delay);
      action?.Invoke();
    }

    public static void RunAfterSeconds(Action action, float delay) => ManagerCore.Instance.StartCoroutine(Invoke(action, delay));
  }
}
