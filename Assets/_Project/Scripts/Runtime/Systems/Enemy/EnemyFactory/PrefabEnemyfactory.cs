using UnityEngine;
using Zenject;


public class EnemyFactory : PlaceholderFactory<GameObject,Enemy,Vector3, EnemyCollider>
{
}

public class PrefabEnemyfactory : IFactory<GameObject, Enemy, Vector3, EnemyCollider>
{

    private readonly DiContainer _container;

    public PrefabEnemyfactory(DiContainer container)
    {
        _container = container;
    }

    public EnemyCollider Create(GameObject prefab,  Enemy enemy, Vector3 pos)
    {
        var enemyCol = _container.InstantiatePrefab(prefab).GetComponent<EnemyCollider>();
        _container.BindInstance(enemyCol);
        enemyCol.SpawnEnemy(enemy,pos);
        return enemyCol;
    }



}
