using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WaveController : MonoBehaviour
{

    [Inject] private EnemyFactory Factory;

    [SerializeField] private float _minBottom, _maxTop;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private LayerMask _enemyLayer;

    public int _totalEnemies;

    public int _waveCount;

    public HudController hudController;

    public GameObject _enemyPrefab;

    public Collider2D _areaSpawn;


    private void Start()
    {
        _ = StartWave(1);
    }

    private async Task StartWave(int level)
    {
        await UniTask.WaitForEndOfFrame();

        _waveCount = level;
        int numberOfEnemies = _waves[level - 1].Enemies.Count;
        _totalEnemies = numberOfEnemies;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 pos = GetRandomSpawnPosition(_areaSpawn);
            Factory.Create(_enemyPrefab,_waves[level - 1].Enemies[i],pos);

            await UniTask.WaitForSeconds(2f);
        }

        await UniTask.WaitForEndOfFrame();
        level++;
        // _ = StartWave(level);
    }


    private Vector2 GetRandomPosition(Collider2D collider)
    {
        Bounds colBounds = collider.bounds;

        Vector2 minBounds = new(colBounds.min.x, colBounds.min.y);
        Vector2 maxBounds = new(colBounds.max.x, colBounds.max.y);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);

        return new(randomX, randomY);
    }

    private Vector2 GetRandomSpawnPosition(Collider2D collider)
    {
        Vector2 spawnPos = Vector2.zero;
        bool isSpawnPosValid = false;

        int count = 0;
        int maxCount = 200;

        int enemyLayer = LayerMask.NameToLayer("CheckTap");

        while (!isSpawnPosValid && count < maxCount)
        {
            spawnPos = GetRandomPosition(collider);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPos, 1f);

            bool isInvalidCollision = false;

            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.layer == enemyLayer)
                {
                    isInvalidCollision = true;
                    break;
                }
            }

            if (!isInvalidCollision)
            {
                isSpawnPosValid = true;
            }

            count++;
        }

        if (!isSpawnPosValid)
        {
            Debug.LogWarning("not found valid pos");
        }

        return spawnPos;
    }
}

[System.Serializable]
public struct Wave
{
    public List<Enemy> Enemies;
    public int WaveLevel;
}
