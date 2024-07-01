using Project._Scripts.Global.Manager.Core;
using Project._Scripts.Library.Audio.Manager;

namespace Project._Scripts.Library.Audio.Interfaces
{
  public interface IAudioOwner
  {
    public IAudioOwner AudioOwner { get; set; }
    public static AudioManager AudioManager => ManagerCore.Instance.GetInstance<AudioManager>();
    public void Play(string audioName, bool isIncrementing = false, float pitchAmount = .025f) => AudioManager.PlayAudio(audioName, isIncrementing, pitchAmount);
  }
}
