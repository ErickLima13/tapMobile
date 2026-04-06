using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Range(0, 20)] public int WeaponCount; // numero de projetil
    [Range(0, 20)] public int WeaponDamage;

    [Range(0, 20)] public float WeaponSpeed; 
    [Range(0, 20)] public float WeaponTime;

    public bool WeaponLiberates;

    public Sprite WeaponVisual;

    public Vector3 WeaponPosition;
}
