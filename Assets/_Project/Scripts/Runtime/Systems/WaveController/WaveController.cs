using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WaveController : MonoBehaviour
{
    [Inject] private ScreenLimits _screenLimits;

    [SerializeField] private float _minBottom, _maxTop;
    [SerializeField] private List<Wave> _waves;

    public Vector2 _distance;

    public int _totalEnemies;

    public int _waveCount;

    public HudController hudController;

    public GameObject _enemyPrefab;

    private void Start()
    {
        _distance = _screenLimits.GetScreenLimits();

        float randomX = Random.Range(-_distance.x, _distance.x);
        float randomY = Random.Range(_minBottom, _maxTop);

        Vector3 pos = new(randomX, randomY);

        GameObject temp = Instantiate(_enemyPrefab);
        temp.GetComponent<EnemyCollider>().SpawnEnemy(pos, _waves[0].Enemies[0]);

        // _ = StartWave(1);
    }

    private List<Enemy> CreateWave(int level)
    {
        //numero de inimigos minimos será 5, e o maximo será 5 * numero de waves.

        int numberOfEnemies = Random.Range(5, 5 * level);
        _totalEnemies = numberOfEnemies;

        List<Enemy> enemies = new();

        for (int i = 0; i < numberOfEnemies; i++)
        {





            float randomX = Random.Range(-_distance.x, _distance.x);
            float randomY = Random.Range(_minBottom, _maxTop);

            enemies.Add(new()
            {


            });
        }

        return enemies;
    }

    private async Task StartWave(int level)
    {
        _waveCount = level;
        var enemies = CreateWave(level);

        await UniTask.WaitForEndOfFrame();

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            // var enemyCollider = _areasMap[enemy.position];

            // enemyCollider.SpawnEnemy(enemy.worldPosition);

            //_currentTime = enemy.activeTime;


            //  enemyCollider.CheckDamage();
        }

        await UniTask.WaitForEndOfFrame();
        level++;
        _ = StartWave(level);
    }

}

[System.Serializable]
public struct Wave
{
    public List<Enemy> Enemies;
    public int WaveLevel;
}
