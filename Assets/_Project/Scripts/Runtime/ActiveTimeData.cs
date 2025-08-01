using UnityEngine;

[CreateAssetMenu(fileName = "ActiveTimeData", menuName = "Scriptable Objects/ActiveTimeData")]
public class ActiveTimeData : ScriptableObject
{
    public ScreenPositions position;
    public float activeTime;
}

public enum ScreenPositions
{
    Left,
    Right
}
