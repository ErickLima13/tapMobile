using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponData))]
public class SetWeaponCount : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space(10f);
        WeaponData controller = (WeaponData)target;
        if (GUILayout.Button("Reset SO"))
        {
            controller.WeaponCount = 1;
        }
    }
}
#endif

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