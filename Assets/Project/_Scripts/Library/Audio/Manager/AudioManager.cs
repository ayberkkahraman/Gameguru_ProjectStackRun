using System;
using System.Collections.Generic;
using System.Linq;
using Project._Scripts.Global.ScriptableObjects;
using Project._Scripts.Library.Audio.Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project._Scripts.Library.Audio.Manager
{
    public class AudioManager : MonoBehaviour
    {
        #region Components
        private AudioSource _bgmSource;
        #endregion
        
        #region Fields
        [Space]
        [Header("PoolObjects")]
        [SerializeField] private GameObject MainSfxPoolObject;
        [SerializeField] private GameObject EffectPoolObject;
        [SerializeField] private GameObject SecondaryEffectSfxPoolObject;
        [SerializeField] private GameObject SecondarySfxPoolObject;
        
        private List<AudioSource> _mainSfxPool = new();
        private List<AudioSource> _effectPool = new();
        private List<AudioSource> _secondaryEffectPool = new();
        private List<AudioSource> _secondarySfxPool = new();

        private List<AudioData> _audioDatas;
        public static int IncrementalPitchCounter;

        #endregion

        #region Unity Functions

        protected void Awake()
        {
            Initialize();
            InitializeAudioResources();
        }

        private void OnEnable() => GameManagerData.OnGameStartedHandler += () => ResetPitch(true);
        private void OnDisable() => GameManagerData.OnGameStartedHandler += () => ResetPitch(true);
        #endregion

        #region Initialization
        private void Initialize()
        {
            //------------------------------INITIALIZING THE AUDIO CHANNELS----------------------------------
            _mainSfxPool = MainSfxPoolObject.GetComponentsInChildren<AudioSource>().ToList();
            _effectPool = SecondaryEffectSfxPoolObject.GetComponentsInChildren<AudioSource>().ToList();
            _secondaryEffectPool = EffectPoolObject.GetComponentsInChildren<AudioSource>().ToList();
            _secondarySfxPool = SecondarySfxPoolObject.GetComponentsInChildren<AudioSource>().ToList();

        }

        private void InitializeAudioResources() => _audioDatas = Resources.LoadAll<AudioData>("AudioDatas").ToList();
        #endregion

        #region Pitch Behaviours
        public static void ResetPitch(bool initial) => IncrementalPitchCounter = initial ? -1 : 0;
        public static void IncreasePitch(){if(IncrementalPitchCounter < 10)IncrementalPitchCounter++;}
        #endregion


        #region Audio Interactions
        public void PlayAudio(string audioName, bool isIncrementing, float pitchAmount = .025f)
        {
            var audioObject = GetAudioByName(audioName);
            
            if (audioObject.AudioClip == null) return;

            //Get the audio source
            AudioSource source = GetAvailableAudioSource(audioObject.Type);
            
            //Updates the pitch according to the incremental value
            if(isIncrementing)IncreasePitch();
            else ResetPitch(false);
            //------------------------------------------------------------------------------------------
            

            //-----------------------------------AUDIO SETTINGS-----------------------------------------
            source.clip = audioObject.AudioClip;
            source.pitch = isIncrementing ? 1 + ((IncrementalPitchCounter) * pitchAmount) : 1;
            source.volume = audioObject.Volume;
            //------------------------------------------------------------------------------------------
            
            //Play audio
            source.Play();
        }
  
        #endregion

        #region Audio Gathering
        /// <summary>
        /// Returns the audio based on it's name from the given audio list
        /// </summary>
        /// <param name="audioName"></param>
        /// <returns></returns>
        public AudioData GetAudioByName(string audioName) => _audioDatas.Find(x => x.name == audioName);

        /// <summary>
        /// Returns the available audio source channel
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public AudioSource GetAvailableAudioSource(AudioData.AudioType type)
        {
            AudioSource source;
            switch (type)
            {
                case AudioData.AudioType.BGM:
                    return _bgmSource;
                case AudioData.AudioType.MainSfx:
                    if (_mainSfxPool.Exists(x => x.isPlaying == false))
                    {
                        return _mainSfxPool.Find(x => x.isPlaying == false);
                    }
                    source = _mainSfxPool.OrderBy(x => x.time).Last();

                    source.Stop();
                    return source;
                case AudioData.AudioType.Effect:
                    if (_effectPool.Exists(x => x.isPlaying == false))
                    {
                        return _effectPool.Find(x => x.isPlaying == false);
                    }
                    source = _effectPool.OrderBy(x => x.time).Last();

                    source.Stop();
                    return source;
                case AudioData.AudioType.SecondaryEffect:
                    if (_secondaryEffectPool.Exists(x => x.isPlaying == false))
                    {
                        return _secondaryEffectPool.Find(x => x.isPlaying == false);
                    }
                    source = _secondaryEffectPool.OrderBy(x => x.time).Last();

                    source.Stop();
                    return source;
                case AudioData.AudioType.SecondarySfx:
                    if (_secondarySfxPool.Exists(x => x.isPlaying == false))
                    {
                        return _secondarySfxPool.Find(x => x.isPlaying == false);
                    }
                    source = _secondarySfxPool.OrderBy(x => x.time).Last();

                    source.Stop();
                    return source;
                default:
                    return null;
            }
        }

        #endregion

    }
    #region Audio Class

    [Serializable]
    public class Audio
    {
        public string AudioName;
        public AudioData AudioData;
    }
    #endregion
}