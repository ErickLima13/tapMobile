using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WaveController : MonoBehaviour
{
    [Inject] private EnemyCollider[] _enemiesColliders;

    private const float _maxTime = 2f;
    private const float _minTime = 0.5f;

    [SerializeField] private float _minBottom, _maxBottom;
    [SerializeField] private List<Wave> _waves;

    public Vector2 _distance;

    public int _totalEnemies;

    public int _waveCount;

    public float _currentTime;

    public HudController hudController;

    private void Start()
    {

       // _distance = _enemiesColliders[0].GetScreenLimits();
        _ = StartWave(1);
    }

    private List<Enemy> CreateWave(int level)
    {
        //numero de inimigos minimos será 5, e o maximo será 5 * numero de waves.

        int numberOfEnemies = Random.Range(5, 5 * level);
        _totalEnemies = numberOfEnemies;

        List<Enemy> enemies = new();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            float time = Random.Range(_minTime, _maxTime);
           

          

            float randomX = Random.Range(-_distance.x, _distance.x);
            float randomY = Random.Range(_minBottom, _maxBottom);

            enemies.Add(new()
            {
              
                WorldPosition = new(randomX, randomY),
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

            await UniTask.WaitForSeconds(_currentTime);

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
