using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using System.Linq;

public class WaveController : MonoBehaviour
{
    [Inject] private CheckTapAction _checkTapAction;
    [Inject] private ObjectPooler _objectPooler;
    [Inject] private DamagePlayer _damagePlayer;
    [Inject] private PlayerStatus _playerStatus;
    [Inject] private ScreenLimits _screenLimits;

    public event Action<int> OnWaveCompleted;

    [SerializeField] private List<Wave> _waves = new();
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private List<Enemy> _enemiesSO;

    [SerializeField] private Collider2D _areaSpawn;

    [SerializeField] private HudController hudController;
    [SerializeField] private GameObject _enemyPrefab;

    [SerializeField] private int _waveCount;
    [SerializeField] private int _totalEnemies;
    [SerializeField] private int enemiesInScene;
    [SerializeField] private Vector2 _timeSpawn;
    [SerializeField] private float _heightMax;

    private List<EnemyCollider> _currentWave = new();

    private bool _cantStartWave;

    public List<Transform> _spawnPositions = new();

    private CancellationTokenSource cancellationTokenSource = new();

    private void OnEnable()
    {
        _checkTapAction.OnEnemyDied += EnemyDied;
        _damagePlayer.OnDamageEvent += EnemyDied;
        _playerStatus.OnGameOver += StopWave;
    }

    private void Start()
    {
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        SetEnemiesSpeed();

        _ = StartWave(_waveCount,cancellationToken);
      

        Vector2 area = _screenLimits.GetLimits();

        GameObject parent = new("SpawnAreas");

        for (int i = 1; i < _heightMax; i++)
        {
            for (int x = 0; x < 4; x++)
            {
                GameObject temp = new("area " + i + $"-{x}");

                Vector2 newArea = new(area.x - 1 - x, i);

                temp.transform.position = newArea;
                temp.transform.SetParent(parent.transform);

                _spawnPositions.Add(temp.transform);
            }
        }
    }

    private void SetEnemiesSpeed()
    {
        float speed = 1f;

        for (int i = 0; i < _enemiesSO.Count; i++)
        {
            _enemiesSO[i].Speed = speed;
            speed += 0.1f;
        }
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

        int numberOfEnemies = 4 * level;
        enemiesInScene = numberOfEnemies;
        _totalEnemies += numberOfEnemies;

        List<Enemy> enemies = new();

        int rand = 0;

        int halfLevel = numberOfEnemies / 2;

        for(int i = 0; i < halfLevel; i++)
        {
            if (level > _enemiesSO.Count-1)
            {
                rand = Random.Range(_enemiesSO.Count-3, _enemiesSO.Count);
                enemies.Add(_enemiesSO[rand]);
            }
            else
            {
                enemies.Add(_enemiesSO[level]);
            }        
        }

        for (int i = enemies.Count; i < numberOfEnemies; i++)
        {
            if (level > _enemiesSO.Count)
            {
                rand = Random.Range(0, _enemiesSO.Count-4);
            }
            else
            {
                rand = Random.Range(0, level);
            }

            enemies.Add(_enemiesSO[rand]);
        }

        _waves.Add(new(enemies, level));

        Shuffle(enemies);

        return enemies;
    }

    private void Shuffle(List<Enemy> a)
    {
        // Loops through array
        for (int i = a.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            var rnd = Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overright when we swap the values
            var temp = a[i];

            // Swap the new and old values
            a[i] = a[rnd];
            a[rnd] = temp;
        }

        // Print
        //for (int i = 0; i < a.Count; i++)
        //{
        //    Debug.Log(a[i]);
        //}
    }

    private async Task StartWave(int level,CancellationToken cancellationToken)
    {
        if (_cantStartWave)
        {
            return;
        }

        List<Enemy> tempEnemies = CreateWave(level);

        await UniTask.WaitForEndOfFrame(cancellationToken);

        _waveCount = level;
        int numberOfEnemies = tempEnemies.Count;
        OnWaveCompleted?.Invoke(_waveCount);

        for (int i = 0; i < numberOfEnemies; i++)
        {
            int rand = Random.Range(0, _spawnPositions.Count);
            Vector2 pos = _spawnPositions[rand].position;
            var temp = _objectPooler.SpawnFromPool("enemy", pos, Quaternion.identity);
            temp.GetComponent<EnemyCollider>().SpawnEnemy(tempEnemies[i]);
            _currentWave.Add(temp.GetComponent<EnemyCollider>());
            float randSpawn = Random.Range(_timeSpawn.x, _timeSpawn.y);
            await UniTask.WaitForSeconds(randSpawn);
        }

        await UniTask.WaitUntil(() => enemiesInScene == 0);

        _currentWave.Clear();

        level++;

        OnWaveCompleted?.Invoke(level);

        _ = StartWave(level,cancellationToken);
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
