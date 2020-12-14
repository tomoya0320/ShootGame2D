using UnityEngine;

public class MathUtil {
  public const float epsilon = 0.0001f;

  /// <summary>
  /// 旋转一个Vector2
  /// </summary>
  public static Vector2 RotateVector2(Vector2 v2, float angle) {
    float x = v2.x * Mathf.Cos(Mathf.Deg2Rad * angle) - v2.y * Mathf.Sin(Mathf.Deg2Rad * angle);
    float y = v2.x * Mathf.Sin(Mathf.Deg2Rad * angle) + v2.y * Mathf.Cos(Mathf.Deg2Rad * angle);
    return new Vector2(x, y);
  }
}