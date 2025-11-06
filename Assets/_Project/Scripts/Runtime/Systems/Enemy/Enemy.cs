using UnityEngine;

public class Enemy
{
    public ScreenPositions position;
    public float activeTime;
    public Vector2 worldPosition;
}

public enum ScreenPositions
{
    Left,
    Right
}
