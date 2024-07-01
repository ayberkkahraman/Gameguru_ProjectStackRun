using UnityEngine;
using UnityEngine.Audio;

namespace Project._Scripts.Library.Audio.Core
{
  public static class Core
  {
    public static float TrueVolume(float value) => Mathf.Log10(value) * 20;
    public static void SetMixerValue(this AudioMixer audioMixer, string name, float value) => audioMixer.SetFloat(name, value);
  }
}
