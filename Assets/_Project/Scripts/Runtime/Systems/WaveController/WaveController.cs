using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WaveController : MonoBehaviour
{
    [Inject] private CheckTapAction _checkTapAction;
    [Inject] private ObjectPooler _objectPooler;
    [Inject] private DamagePlayer _damagePlayer;
    [Inject] private PlayerStatus _playerStatus;

    [SerializeField] private List<Wave> _waves;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private List<Enemy> _enemiesSO;

    [SerializeField] private Collider2D _areaSpawn;

    [SerializeField] private HudController hudController;
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private int _waveCount;
    [SerializeField] private int _totalEnemies;
    [SerializeField] private int enemiesInScene;

    private List<EnemyCollider> _currentWave = new();

    private bool _cantStartWave;

    private CancellationTokenSource cancellationTokenSource = new();

    private void Start()
    {
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        _checkTapAction.OnEnemyDied += EnemyDied;
        _damagePlayer.OnDamageEvent += EnemyDied;
        _playerStatus.OnGameOver += StopWave;

        _ = StartWave(0,cancellationToken);
    }

    private void StopWave()
    {
        foreach (EnemyCollider e in _currentWave)
        {
            e.gameObject.SetActive(false);
        }

        cancellationTokenSource.Cancel();

        _cantStartWave = true;
    }

    private void EnemyDied(PointType type)
    {
        if (enemiesInScene > 0)
        {
            enemiesInScene--;
        }
    }

    private List<Enemy> CreateWave(int level)
    {
        //numero de inimigos minimos será 5, e o maximo será 5 * numero de waves.

        int numberOfEnemies = Random.Range(5, 5 * level);
        enemiesInScene = numberOfEnemies;
        _totalEnemies += numberOfEnemies;

        List<Enemy> enemies = new();

        if(level > 1)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int rand = Random.Range(0, level);
                enemies.Add(_enemiesSO[rand]);
            }

            _waves.Add(new(enemies, level));
        }

        return enemies;
    }

    private async Task StartWave(int level, CancellationToken cancellationToken)
    {
        if (_cantStartWave)
        {
            return;
        }

        CreateWave(level);

        await UniTask.WaitForEndOfFrame();

        _waveCount = level;
        int numberOfEnemies = _waves[level].Enemies.Count;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 pos = GetRandomSpawnPosition(_areaSpawn);
            var temp = _objectPooler.SpawnFromPool("enemy", pos, Quaternion.identity);
            _ = temp.GetComponent<EnemyCollider>().SpawnEnemy(_waves[level].Enemies[i]);
            _currentWave.Add(temp.GetComponent<EnemyCollider>());
            float randSpawn = Random.Range(0.2f, 2f);
            await UniTask.WaitForSeconds(randSpawn);
        }

        await UniTask.WaitUntil(() => enemiesInScene == 0);

        _currentWave.Clear();

        level++;

        _ = StartWave(level, cancellationToken);
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
        _playerStatus.OnGameOver -= StopWave;
    }

    private void OnDestroy()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
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
