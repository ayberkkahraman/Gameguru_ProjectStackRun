using System;
using Project._Scripts.GameCore.InteractionSystem.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Project._Scripts.GameCore.InteractionSystem.Interactables.Core
{
  public abstract class InteractableBase : MonoBehaviour, IInteractable
  {
     #region Fields
    public BaseSettings BaseSettings;
    public bool IsInteractable { get; set; }
    #endregion

    #region Actions
    public Action InteractionStartCallback { get; set; }
    public Action InteractionEndCallback { get; set; }
    #endregion
    

    #region Unity Functions
    
    public virtual void OnTriggerEnter(Collider other)
    {
      if(!other.gameObject.CompareTag($"Player")) return;
      
      IsInteractable = true;

      if(BaseSettings.InteractOnTrigger)StartInteraction();
    }

    public virtual void OnTriggerExit(Collider other)
    {
      if(!other.gameObject.CompareTag($"Player")) return;

      IsInteractable = false;

      if (!BaseSettings.InteractOnTrigger) return;
      
      EndInteraction();
      
      if(BaseSettings.DestroyAfterTriggerEnd)
        Destroy(gameObject);
    }
    #endregion
    
    public virtual void StartInteraction() => InteractionStartCallback?.Invoke();

    public virtual void EndInteraction() => InteractionEndCallback?.Invoke();
  }
  
  [Serializable]
  public struct BaseSettings
  {
    public bool DestroyAfterTriggerEnd;
    public bool InteractOnTrigger;
  }
}
