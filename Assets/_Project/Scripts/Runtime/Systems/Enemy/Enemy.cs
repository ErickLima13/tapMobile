using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [Range(0.1f, 1)] public float Speed;
    [Range(1, 10)] public int Lifes;
    public Sprite Visual;
}


