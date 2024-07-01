using System;
using UnityEngine;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Elements
{
  public class Destroyer : MonoBehaviour
  {
    private void OnTriggerEnter(Collider other)
    {
      Destroy(other.gameObject);
    }
  }
}
