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

    private void Start()
    {
        foreach (EnemyCollider a in _enemiesColliders) _areasMap.Add(a.position, a);

        _distance = _enemiesColliders[0].GetScreenLimits();
        _ = StartWave(1);
    }

    private List<Enemy> CreateWave(int level)
    {
        int numberOfEnemies = 5;
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

        //yield return new WaitForEndOfFrame();
        //level++;
        //StartCoroutine(StartWave(level));
    }

}

[System.Serializable]
public struct Wave
{
    public List<Enemy> Activetimes;
    public int WaveLevel;
}
