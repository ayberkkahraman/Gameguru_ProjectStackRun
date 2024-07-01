using DG.Tweening;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.Global.Manager.ManagerClasses
{
  public class UIManager : MonoBehaviour
  {
    public RectTransform SuccessPanel;
    public RectTransform FailPanel;

    private void OnEnable()
    {
      GameManagerData.OnLevelSuccessHandler += () => ExecutePanel(SuccessPanel);
      GameManagerData.OnLevelFailHandler += () => ExecutePanel(FailPanel);
    }

    private void OnDisable()
    {
      GameManagerData.OnLevelSuccessHandler -= () => ExecutePanel(SuccessPanel);
      GameManagerData.OnLevelFailHandler -= () => ExecutePanel(FailPanel);
    }

    public void ExecutePanel(RectTransform rectTransform)
    {
      var defaultScale = rectTransform.localScale;
      
      rectTransform.localScale = Vector3.zero;
      rectTransform.gameObject.SetActive(true);
      rectTransform.DOScale(defaultScale, .5f).SetEase(Ease.InOutBounce);
    }
  }
}
