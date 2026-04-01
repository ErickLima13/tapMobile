using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRuntimeLookup", menuName = "Scriptable Objects/Lookup")]
public class EnemyRuntimeLookup : ScriptableObject
{
    public List<EnemyCollider> CurrentEnemies = new();

    public LayerMask TargetMask;

    public void Register(EnemyCollider enemy) { if (!CurrentEnemies.Contains(enemy)) CurrentEnemies.Add(enemy); }
    public void Unregister(EnemyCollider enemy) { if (CurrentEnemies.Contains(enemy)) CurrentEnemies.Remove(enemy); }


    List<EnemyCollider> FindEnemiesInArea(Vector2 center, float radius, LookupCriteria sortCriteria)
    {
        List<EnemyCollider> enemiesInArea = new();

        Collider2D[] enemies = Physics2D.OverlapCircleAll(center, radius, TargetMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemiesInArea.Add(enemies[i].GetComponent<EnemyCollider>());
        }

        return enemiesInArea;
    }

    EnemyCollider FindEnemy(LookupCriteria criteria)
    {
        return null;
    }





}

enum LookupCriteria
{
    Random,
    Closest,
    Fartest,
    LowLife,
    HigherLife,
}

