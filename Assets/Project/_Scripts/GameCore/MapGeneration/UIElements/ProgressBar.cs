using Project._Scripts.GameCore.MapGeneration.System;
using Project._Scripts.Global.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Project._Scripts.GameCore.MapGeneration.UIElements
{
  [DefaultExecutionOrder(1200)]
  public class ProgressBar : MonoBehaviour
  {
    private Slider _slider;
    private Transform _characterTransform;
    private float _distance;
    private float _initialPosition;

    private bool _isActive;
    private void Awake()
    {
      _slider = GetComponentInChildren<Slider>();
      _characterTransform = FindObjectOfType<CharacterController.Core.CharacterController>(true).transform;
    }

    private void OnEnable()
    {
      GameManagerData.OnGameStartedHandler += () =>
      {
        _slider.value = 0f;
        _initialPosition = _characterTransform.position.z;
        _distance = LevelGenerator.SCurrentFinish.transform.Find("EndPoint").transform.position.z - _initialPosition;
        _isActive = true;
      };
      GameManagerData.OnLevelFailHandler += () => _isActive = false;
      GameManagerData.OnLevelSuccessHandler += () => _isActive = false;
    }

    private void OnDisable()
    {
      GameManagerData.OnGameStartedHandler -= () =>
      {
        _slider.value = 0f;
        _initialPosition = _characterTransform.position.z;
        _distance = LevelGenerator.SCurrentFinish.transform.Find("EndPoint").transform.position.z - _initialPosition;
        _isActive = true;
      };
      GameManagerData.OnLevelFailHandler -= () => _isActive = false;
      GameManagerData.OnLevelSuccessHandler -= () => _isActive = false;
    }

    private void LateUpdate()
    {
      if(!_isActive) return;

      _slider.value = (_characterTransform.position.z - _initialPosition) / _distance;
    }
  }
}
