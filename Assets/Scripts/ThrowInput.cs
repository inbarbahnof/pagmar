using UnityEngine;

public struct ThrowInput
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    public float throwSpeed;
    public float arcHeight;

    public ThrowInput(Vector2 startPoint, Vector2 endPoint, float throwSpeed, float arcHeight)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.throwSpeed = throwSpeed;
        this.arcHeight = arcHeight;
    }
}