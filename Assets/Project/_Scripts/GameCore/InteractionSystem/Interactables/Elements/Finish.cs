using System;
using Project._Scripts.GameCore.InteractionSystem.Interactables.Core;
using UnityEngine;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Elements
{
  public class Finish : InteractableBase
  {
    public override void StartInteraction()
    {
      base.StartInteraction();
      Debug.Log("Test");
    }

    private void OnDisable()
    {
      
    }
  }
}
