using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [Range(0, 1)] public float Speed;
    [Range(0, 10)] public int Lifes;
    public Sprite Visual;
}


