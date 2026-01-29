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
    [Inject] private ScreenLimits _screenLimits;

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

    public List<Transform> _spawnPositions = new();

    private CancellationTokenSource cancellationTokenSource = new();

    private void Start()
    {
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        _checkTapAction.OnEnemyDied += EnemyDied;
        _damagePlayer.OnDamageEvent += EnemyDied;
        _playerStatus.OnGameOver += StopWave;

        _ = StartWave(0,cancellationToken);

        Vector2 area = _screenLimits.GetLimits();

        GameObject parent = new("SpawnAreas");

        for (int i = 1; i < 5; i++)
        {
            for(int x = 0; x < 4; x++)
            {
                GameObject temp = new("area " + i + $"-{x}");

                Vector2 newArea = new(area.x - 1 - x, i);

                temp.transform.position = newArea;
                temp.transform.SetParent(parent.transform);

                _spawnPositions.Add(temp.transform);
            }
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
            int rand = Random.Range(0, _spawnPositions.Count);

            Vector2 pos = _spawnPositions[rand].position;

            var temp = _objectPooler.SpawnFromPool("enemy", pos, Quaternion.identity);

            Debug.LogWarning(_waves[level].Enemies[i].name + "-nasce em : " + pos);

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
