using DG.Tweening;
using Project._Scripts.GameCore.PlatformSystem.Core;
using Project._Scripts.GameCore.PlatformSystem.ScriptableObjects;
using Project._Scripts.Global.Manager.Core;
using Project._Scripts.Global.Manager.ManagerClasses;
using UnityEngine;

namespace Project._Scripts.GameCore.PlatformSystem.System
{
  [DefaultExecutionOrder(750)]
  public class PlatformController : MonoBehaviour
  {
    public static Platform CurrentPlatform;
    public static Transform PreviousPlatform;

    public PlatformControllerData PlatformControllerData;
    public static PlatformControllerData SPlatformControllerData;

    public static int SnappedPlatformCount;

    public delegate void OnPlatformSnapped(Platform platform);
    public static OnPlatformSnapped OnPlatformSnappedHandler;
   
   public delegate void OnPlatformKilled(Platform platform);
   public static OnPlatformKilled OnPlatformKilledHandler;
   
   public delegate void OnPlatformSpawned(float scale = 0f);
   public static OnPlatformSpawned OnPlatformSpawnedHandler;

   private int _platformCount;

   private void Start() => OnPlatformSpawnedHandler(PlatformControllerData.PlatformPrefab.transform.localScale.x);

   private void Awake() => SPlatformControllerData = PlatformControllerData;

   private void OnEnable()
   {
     SnappedPlatformCount = 0;
     
     OnPlatformSpawnedHandler += SpawnPlatform;
     OnPlatformSpawnedHandler += (_) => ManagerCore.Instance.GetInstance<CameraManager>().UpdateFollowTarget(PreviousPlatform);
     OnPlatformKilledHandler += KillPlatform;

     OnPlatformSnappedHandler += (_) => IncreaseSnappedPlatformCount();
     OnPlatformSnappedHandler += CheckSnappedPlatforms;
     OnPlatformSnappedHandler += (_) => PlayAudio();
   }

   private void OnDisable()
   {
     OnPlatformKilledHandler -= KillPlatform;
     OnPlatformSpawnedHandler -= SpawnPlatform;
     OnPlatformSpawnedHandler -= (_) => ManagerCore.Instance.GetInstance<CameraManager>().UpdateFollowTarget(PreviousPlatform);
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
     if(Mathf.Approximately(platform.transform.localScale.x, 3f)) return;
     
     Debug.Log(SnappedPlatformCount);
     if (SnappedPlatformCount >= 3)
     {
       platform.IncreaseScale();
     }
   }

   private void PlayAudio()
   {
     // Debug.Log("Music Note");
   }

   private void SpawnPlatform(float scale = 0f)
   {
     PreviousPlatform = transform.GetChild(0);
     int multiplier = _platformCount % 2 == 0 ? 1 : -1;
     var position = new Vector3(multiplier * -6f, PreviousPlatform.transform.position.y, PreviousPlatform.transform.position.z + PreviousPlatform.transform.localScale.z);
     
     Platform platform = Instantiate(PlatformControllerData.PlatformPrefab, position, Quaternion.identity, transform);
     var targetScale = new Vector3(scale, PreviousPlatform.localScale.y, PreviousPlatform.localScale.z);
     platform.transform.localScale = targetScale;

     platform.transform.SetSiblingIndex(0);
     PreviousPlatform = transform.GetChild(1);
     CurrentPlatform = platform;

     _platformCount++;
   }
  }
}
