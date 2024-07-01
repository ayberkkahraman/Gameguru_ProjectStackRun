using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.Global.Manager.ManagerClasses
{
    public class CameraManager : MonoBehaviour
    {
        #region Cameras
        public CinemachineVirtualCamera DefaultCam{ get; set; }
        public CinemachineVirtualCamera CharacterCamera;
        public CinemachineVirtualCamera LevelEndCamera;
        public CinemachineVirtualCameraBase CurrentCamera { get; set; }
        #endregion

        #region Fields
        public List<CinemachineVirtualCamera> CinemachineVirtualCameras { get; set; }
        private CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin{ get; set; }
        [Header("Camera Shake")]
        [Space]
        [Range(.5f, 7.5f)]
        public float ShakeIntensity = 1.0f;
        [Range(0.0f, .5f)]
        public float ShakeDuration = .1f;
        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
        {
            DeInit();
        }
        #endregion

    
        #region Init
        public void Init()
        {
            //Setting the cinemachine cameras for the list to be let it accessible
            CinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>().ToList();

            DefaultCam = CharacterCamera;
            CurrentCamera = CharacterCamera;
            CinemachineBasicMultiChannelPerlin = DefaultCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            GameManagerData.OnLevelFailHandler += () => UpdateFollowTarget(null);
            GameManagerData.OnLevelFailHandler += () =>
            {
                ShakeCamera(4.5f, .35f, .1f);
            };

            GameManagerData.OnGameStartedHandler += () => ChangeActiveCamera(CharacterCamera);
        }

        private void DeInit()
        {
            GameManagerData.OnLevelFailHandler -= () => UpdateFollowTarget(null);
            GameManagerData.OnLevelFailHandler -= () =>
            {
                ShakeCamera(8f, 1f, .075f);
            };
            
            GameManagerData.OnGameStartedHandler -= () => ChangeActiveCamera(CharacterCamera);
        }
        #endregion
    
        #region Camera

        /// <summary>
        /// Changes the current active camera in a spesific time
        /// </summary>
        /// <param name="targetCamera"></param>
        public void ChangeActiveCamera(CinemachineVirtualCameraBase targetCamera)
        {
            if (targetCamera == CurrentCamera) return;
            
            //Disables all the cameras to access only one camera easily
            foreach (CinemachineVirtualCamera cinemachineVirtualCamera in CinemachineVirtualCameras)
            {
                cinemachineVirtualCamera.Priority = 0;
            }

            CurrentCamera.Priority = 0;
            targetCamera.Priority = 10;

            CurrentCamera = targetCamera;
   
            DefaultCam = CharacterCamera;
            CinemachineBasicMultiChannelPerlin = DefaultCam != null ? DefaultCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() : null;
            //-----------------------------------------------
        }

        /// <summary>
        /// Updates the follow transform of the Camera
        /// </summary>
        /// <param name="targetTransform"></param>
        public void UpdateFollowTarget(Transform targetTransform)
        {
            CurrentCamera.Follow = targetTransform;
        }
        
        public void UpdateLookAtTarget(Transform targetTransform)
        {
            CurrentCamera.LookAt = targetTransform;
        }

        public static void ShakeCamera(Action action) => action?.Invoke();

        public void ShakeCamera()
        {
            StartCoroutine(ShakeCameraCoroutine(ShakeIntensity, ShakeDuration));
        }
    
        public void ShakeCamera(float intensity, float duration, float frequencyGain = 0f)
        {
            if(CurrentCamera != CharacterCamera) return;
            
            StartCoroutine(ShakeCameraCoroutine(intensity, duration, frequencyGain));
        }
        
        
        /// <summary>
        /// Shakes the Camera
        /// </summary>
        /// <returns></returns>
        public IEnumerator ShakeCameraCoroutine(float intensity, float duration, float frequencyGain = 0f)
        {
            StartCoroutine(SmoothIncreaseAmplitude(duration/3, intensity));
        
            var time = Time.time;
            while (Time.time < time + duration)
            {
                CinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequencyGain;
                yield return null;
            }

            StartCoroutine(SmoothReturnToZero(duration/3));
        }
        
        public IEnumerator SmoothIncreaseAmplitude(float duration, float targetValue)
        {
            float startTime = Time.time;
            float startValue = CinemachineBasicMultiChannelPerlin.m_AmplitudeGain;

            while (Time.time < startTime + duration)
            {
                float elapsedTime = Time.time - startTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float newValue = Mathf.Lerp(startValue, targetValue, t);

                CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = newValue;

                yield return null;
            }
            
            CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = targetValue;
        }
        public IEnumerator SmoothReturnToZero(float duration)
        {
            float startTime = Time.time;
            float startValue = CinemachineBasicMultiChannelPerlin.m_AmplitudeGain;

            while (Time.time < startTime + duration)
            {
                float elapsedTime = Time.time - startTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float newValue = Mathf.Lerp(startValue, 0f, t);

                CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = newValue;

                yield return null;
            }
            
            CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
        #endregion
    }
}