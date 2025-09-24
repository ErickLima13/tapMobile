using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WaveController : MonoBehaviour
{
    [Inject] private EnemyCollider[] _enemiesColliders;

    public Vector2 _distance;
    private Dictionary<ScreenPositions, EnemyCollider> _areasMap = new();

    public int _totalEnemies;

    public int _waveCount;

    private void Start()
    {
        foreach (EnemyCollider a in _enemiesColliders) _areasMap.Add(a.position, a);

        _distance = _enemiesColliders[0].GetScreenLimits();
        _ = StartWave(1);
    }
   
    private List<Enemy> CreateWave(int level)
    {
        //numero de inimigos minimos será 5, e o maximo será 5 * numero de waves.

        int numberOfEnemies = Random.Range(5, 5 * level);
        _totalEnemies = numberOfEnemies;

        float maxTime = 2f;
        float minTime = 0.5f;

        List<Enemy> enemies = new();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            float time = Random.Range(minTime, maxTime);
            ScreenPositions position = ScreenPositions.Left;

            if (Random.Range(0f, 1f) < 0.5f)
            {
                position = ScreenPositions.Right;
            }

            float randomX = Random.Range(-_distance.x, _distance.x);
            float randomY = Random.Range(-_distance.y, _distance.y);

            enemies.Add(new()
            {
                activeTime = time,
                position = position,
                worldPosition = new(randomX, randomY),
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
            var enemyCollider = _areasMap[enemy.position];

            enemyCollider.transform.position = enemy.worldPosition;
            enemyCollider.ActiveVisual(true);

            await UniTask.WaitForSeconds(enemy.activeTime);

            if (enemyCollider.GetVisualStatus())
            {
                enemyCollider.ActiveVisual(false);
                enemyCollider.TakeDamage();
            }
        }

        await UniTask.WaitForEndOfFrame();
        level++;
        _ = StartWave(level);
    }

}

[System.Serializable]
public struct Wave
{
    public List<Enemy> Activetimes;
    public int WaveLevel;
}
