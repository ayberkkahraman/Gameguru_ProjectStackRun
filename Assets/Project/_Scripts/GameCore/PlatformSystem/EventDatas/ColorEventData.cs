using Project._Scripts.Global.EventData.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project._Scripts.GameCore.PlatformSystem.EventDatas
{
  [CreateAssetMenu(fileName = "ColorEventData", menuName = "GameCore/PlatformSystem/EventDatas/ColorEventData")]
  public class ColorEventData : EventData
  {
    public static int STimeStep{ get; set; }
    [Range(1, 20)]public int TimeStep = 10;
    
    public static Color CurrentColor { get; set; }
    public static Color TargetColor{ get; set; }
    private static Vector3 _colorChangeAmountOnStep;
    public override void Execute()
    {
      STimeStep = TimeStep;
      SetTargetColor();
    }
    public override void Dispose() => STimeStep = 0;

    public static void SetTargetColor()
    {
      TargetColor = RandomColor();
        
      _colorChangeAmountOnStep.x = (TargetColor.r - CurrentColor.r) / STimeStep;
      _colorChangeAmountOnStep.y = (TargetColor.g - CurrentColor.g) / STimeStep;
      _colorChangeAmountOnStep.z = (TargetColor.b - CurrentColor.b) / STimeStep;
    }
    public static void SetNextColor(Material material)
    {
      if (Vector4.Distance(CurrentColor, TargetColor) < 0.1f) SetTargetColor();

      CurrentColor = new Color(CurrentColor.r + _colorChangeAmountOnStep.x,
        CurrentColor.g + _colorChangeAmountOnStep.y,
        CurrentColor.b + _colorChangeAmountOnStep.z);

      material.color = CurrentColor;
    }
    
    public static Color RandomColor() => new(Random.value, Random.value, Random.value);
  }
}
