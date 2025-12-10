using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WaveController : MonoBehaviour
{
    [Inject] private CheckTapAction _checkTapAction;
    [Inject] private EnemyFactory Factory;
    [Inject] private DamagePlayer _damagePlayer;

    [SerializeField] private List<Wave> _waves;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private List<Enemy> _enemiesSO;

    [SerializeField] private Collider2D _areaSpawn;

    [SerializeField] private HudController hudController;
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private int _waveCount;
    [SerializeField] private int _totalEnemies;
    [SerializeField] private int enemiesInScene;


    private void Start()
    {
        _checkTapAction.OnEnemyDied += EnemyDied;
        _damagePlayer.OnDamageEvent += EnemyDied;

        _ = StartWave(1);
    }

    private void EnemyDied(PointType type)
    {
        enemiesInScene--;
    }

    private List<Enemy> CreateWave(int level)
    {
        //numero de inimigos minimos será 5, e o maximo será 5 * numero de waves.

        int numberOfEnemies = Random.Range(5, 5 * level);
        enemiesInScene = numberOfEnemies;
        _totalEnemies += numberOfEnemies;

        List<Enemy> enemies = new();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            int rand = Random.Range(0, _enemiesSO.Count);

            enemies.Add(_enemiesSO[rand]);
        }


        _waves.Add(new(enemies, level - 1));

        return enemies;
    }

    private async Task StartWave(int level)
    {
        CreateWave(level);

        await UniTask.WaitForEndOfFrame();

        _waveCount = level;
        int numberOfEnemies = _waves[level - 1].Enemies.Count;

        GameObject temp = new GameObject("Wave " + (level - 1));
        temp.transform.SetParent(_areaSpawn.transform);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 pos = GetRandomSpawnPosition(_areaSpawn);
       
            var e = Factory.Create(_enemyPrefab, _waves[level - 1].Enemies[i], pos);
            e.transform.SetParent(temp.transform);

            await UniTask.WaitForSeconds(2f);
        }

        await UniTask.WaitUntil(() => enemiesInScene == 0);

        level++;
        _ = StartWave(level);
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

    private void OnDisable()
    {
        _checkTapAction.OnEnemyDied -= EnemyDied;
        _damagePlayer.OnDamageEvent -= EnemyDied;
    }
}

[System.Serializable]
public struct Wave
{
    public Wave(List<Enemy> enemies, int waveLevel)
    {
        Enemies = enemies;
        WaveLevel = waveLevel;
    }

    public List<Enemy> Enemies;
    public int WaveLevel;
}
