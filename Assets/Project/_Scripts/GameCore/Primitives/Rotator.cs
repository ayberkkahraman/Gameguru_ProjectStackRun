using DG.Tweening;
using UnityEngine;

namespace Project._Scripts.GameCore.Primitives
{
  public class Rotator : MonoBehaviour
  {
    public void Rotate()
    {
      transform.DORotate(Vector3.up * 60f, 1f)
        .SetEase(Ease.Linear)
        .SetLoops(-2, LoopType.Incremental);
    }
  }
}
