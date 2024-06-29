using System;
using Project._Scripts.GameCore.PlatformSystem.Core;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.System
{
  public class PlatformController : MonoBehaviour
  {
    public Platform CurrentPlatform;
    public Platform PreviousPlatform;
   
   public delegate void OnPlatformKilled(Platform platform);
   public static OnPlatformKilled OnPlatformKilledHandler;
   
   public delegate void OnPlatformSpawned(Platform platform);
   public static OnPlatformSpawned OnPlatformSpawnedHandler;

   private void Start()
   {
     PreviousPlatform = GameObject.Find("Platform_2").GetComponent<Platform>();
   }

   private void OnDisable()
   {
     OnPlatformKilledHandler = null;
   }

   private void Update()
   {
     if (Input.GetMouseButtonDown(0))
     {
       OnPlatformKilledHandler(CurrentPlatform);
     }
   }
  }
}
