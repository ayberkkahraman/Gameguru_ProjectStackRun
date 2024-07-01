using DG.Tweening;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;

namespace Project._Scripts.Global.Manager.ManagerClasses
{
  public class UIManager : MonoBehaviour
  {
    #region Fields
    public RectTransform SuccessPanel;
    public RectTransform FailPanel;
    #endregion

    #region Unity Functions
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
    #endregion

    #region UI Behaviours
    public void ExecutePanel(RectTransform rectTransform)
    {
      var defaultScale = rectTransform.localScale;
      
      rectTransform.localScale = Vector3.zero;
      rectTransform.gameObject.SetActive(true);
      rectTransform.DOScale(defaultScale, .5f).SetEase(Ease.InOutBounce);
    }
    #endregion
  }
}