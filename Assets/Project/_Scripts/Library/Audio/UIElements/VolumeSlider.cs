using Project._Scripts.Library.Configuration.Progress;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Project._Scripts.Library.Audio.UIElements
{
  public class VolumeSlider : MonoBehaviour
  {
    public AudioMixer AudioMixer;
    private Slider _slider;
    private void Awake() => _slider = GetComponent<Slider>();
    private void OnEnable() => LoadSlider();
    private void OnDisable() => SaveSlider();
    private void Start() => SLIDER_OnChange(_slider.value);
    public void SLIDER_OnChange(float value) => AudioMixer.SetFloat($"MasterVolume", Core.Core.TrueVolume(value));
    public void SaveSlider() => Progress.Save("Volume", _slider.value);
    public void LoadSlider() => _slider.value = Progress.Load("Volume", 1f);
  }
}
