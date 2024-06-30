// using System;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// namespace Project._Scripts.Global.Library.Color
// {
//   public class Color : MonoBehaviour
//   {
//     private Material _material;
//     [SerializeField] private float _colorChangeSpeed = 1f;
//
//     private UnityEngine.Color _currentColor;
//     private UnityEngine.Color _targetColor;
//
//     public int TimeStep = 10;
//
//     private Vector3 _changeOnStep;
//
//     private void Start()
//     {
//       _material = GetComponent<MeshRenderer>().material;
//       _currentColor = _material.color;
//       _targetColor = GetRandomColor();
//
//       _changeOnStep.x = (_targetColor.r - _currentColor.r) / TimeStep;
//       _changeOnStep.y = (_targetColor.g - _currentColor.g) / TimeStep;
//       _changeOnStep.z = (_targetColor.b - _currentColor.b) / TimeStep;
//       
//     }
//
//     private void Update()
//     {
//       if(!Input.GetMouseButtonDown(0)) return;
//       
//       UpdateColor();
//     }
//
//     private void UpdateColor()
//     {
//       // Smoothly interpolate between the current color and the target color
//       _currentColor = new UnityEngine.Color(
//         _currentColor.r + _changeOnStep.x, 
//         _currentColor.g + _changeOnStep.y, 
//         _currentColor.b + _changeOnStep.z);
//       _material.color = _currentColor;
//
//       // If the current color is close enough to the target color, pick a new target color
//       if (Vector4.Distance(_currentColor, _targetColor) < 0.1f)
//       {
//
//         
//         _changeOnStep.x = (_targetColor.r - _currentColor.r) / TimeStep;
//         _changeOnStep.y = (_targetColor.g - _currentColor.g) / TimeStep;
//         _changeOnStep.z = (_targetColor.b - _currentColor.b) / TimeStep;
//       }
//     }
//   }
// }
