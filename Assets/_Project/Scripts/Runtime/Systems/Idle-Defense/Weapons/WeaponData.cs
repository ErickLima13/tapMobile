using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string WeaponName;

    [Range(0, 20)] public int WeaponCount; // numero de projetil
    [Range(0, 20)] public int WeaponDamage;

    [Range(0, 20)] public float WeaponSpeed; 
    [Range(0, 20)] public float WeaponTime;

    public bool WeaponLiberates;

    public Sprite WeaponVisual;

    public Vector3 WeaponPosition;
}

[System.Serializable]
public struct WeaponAttributes
{
    public bool WeaponLiberates;

    public int WeaponCount;
    public int WeaponDamage;

    public string WeaponName;


    public WeaponAttributes(bool liberates, int count, int damage, string name)
    {
        WeaponLiberates = liberates;
        WeaponCount = count;
        WeaponDamage = damage;
        WeaponName = name;
    }
}