using UnityEngine;

public static class TrajectoryEvaluator
{
    public static Vector2 Evaluate(float t, Vector2 startPoint, Vector2 endPoint, Vector2 controlPoint)
    {
        Vector2 sc = Vector2.Lerp(startPoint, controlPoint, t);
        Vector2 ce = Vector2.Lerp(controlPoint, endPoint, t);
        return Vector2.Lerp(sc, ce, t);
    }
}