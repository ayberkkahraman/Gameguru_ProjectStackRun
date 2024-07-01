using UnityEngine;

namespace Project._Scripts.Library.Audio.Scriptable
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Library/Audio/AudioData")]
    public class AudioData : ScriptableObject
    {
        #region References
        public AudioClip AudioClip;
        #endregion
    
        #region Fields
        [Header("Attributes")]
        public AudioType Type;
        [Range(0,1f)]public float Volume = .5f;

        [Range(-.5f,.5f)]
        public float PitchVariation;
        public enum AudioType
        {
            BGM,
            MainSfx,
            SecondaryEffect,
            SecondarySfx,
            Effect
        }
        #endregion
    }
}