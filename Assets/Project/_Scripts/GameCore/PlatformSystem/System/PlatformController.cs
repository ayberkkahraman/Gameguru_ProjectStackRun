using System;
using DG.Tweening;
using Project._Scripts.GameCore.PlatformSystem.Core;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.System
{
  public class PlatformController : MonoBehaviour
  {
    public Platform PlatformPrefab;
    public static Platform CurrentPlatform;
    public static Transform PreviousPlatform;

    [Range(0f, 2f)] public float Tolerance = .1f;
    public static float TOLERANCE;

    public static int SnappedPlatformCount;

    public delegate void OnPlatformSnapped(Platform platform);
    public static OnPlatformSnapped OnPlatformSnappedHandler;
   
   public delegate void OnPlatformKilled(Platform platform);
   public static OnPlatformKilled OnPlatformKilledHandler;
   
   public delegate void OnPlatformSpawned();
   public static OnPlatformSpawned OnPlatformSpawnedHandler;

   private int _platformCount;

   private void Start() => OnPlatformSpawnedHandler();

   private void Awake() => TOLERANCE = Tolerance;

   private void OnEnable()
   {
     SnappedPlatformCount = 0;
     
     OnPlatformSpawnedHandler += SpawnPlatform;
     OnPlatformKilledHandler += KillPlatform;

     OnPlatformSnappedHandler += (_) => IncreaseSnappedPlatformCount();
     OnPlatformSnappedHandler += CheckSnappedPlatforms;
     OnPlatformSnappedHandler += (_) => PlayAudio();
   }

   private void OnDisable()
   {
     OnPlatformKilledHandler -= KillPlatform;
     OnPlatformSpawnedHandler -= SpawnPlatform;
   }

   private void Update()
   {
     if (!Input.GetMouseButtonDown(0))
       return;
     OnPlatformKilledHandler(CurrentPlatform);
   }

   private void KillPlatform(Platform platform)
   {
     platform.TransitionTween.Kill();
     platform.KillPlatform();
   }

   private void IncreaseSnappedPlatformCount() => SnappedPlatformCount++;
   private void CheckSnappedPlatforms(Platform platform)
   {
     platform.IncreaseScale();
     // if(Mathf.Approximately(platform.transform.localScale.x, platform.transform.localScale.z)) return;
     
     // if (SnappedPlatformCount >= 3)
     // {
     //   platform.IncreaseScale();
     // }
   }

   private void PlayAudio()
   {
     // Debug.Log("Music Note");
   }

   private void SpawnPlatform()
   {
     PreviousPlatform = transform.GetChild(0);
     int multiplier = _platformCount % 2 == 0 ? 1 : -1;
     var position = new Vector3(multiplier * -6f, PreviousPlatform.transform.position.y, PreviousPlatform.transform.position.z + PreviousPlatform.transform.localScale.z);
     
     Platform platform = Instantiate(PlatformPrefab, position, Quaternion.identity, transform);
     platform.transform.localScale = PreviousPlatform.localScale;

     platform.transform.SetSiblingIndex(0);
     PreviousPlatform = transform.GetChild(1);
     CurrentPlatform = platform;

     _platformCount++;
   }
  }
}
