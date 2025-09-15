using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;
    public int maxLife;
    public int life;
    public bool isTap;
    public Vector2 _distance;

    public CheckTapAction _checkTapAction;
    public TextMeshProUGUI _scoreText;

    public AreaCollider[] _areasColliders;
    public List<ActiveTime> activetimes = new();

    private Dictionary<ScreenPositions, AreaCollider> _areasMap = new();

    private void Start()
    {
        life = maxLife;

        foreach (AreaCollider a in _areasColliders)
        {
            a.OnDisableCollider += CheckDisable;
            _areasMap.Add(a.position, a);
            a.gameObject.SetActive(false);
        }

        _checkTapAction.OnTapCollider += CheckTap;
        StartCoroutine(StartWave(1));
    }

    private List<ActiveTime> CreateWave(int level)
    {
        int numberOfEnemies = 5;
        float maxTime = 2f;
        float minTime = 0.5f;

        List<ActiveTime> enemies = new();

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

    private IEnumerator StartWave(int level)
    {
        var enemies = CreateWave(level);

        yield return new WaitForEndOfFrame();

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            var areaCollider = _areasMap[enemy.position];

            print("aqui começo  " + i);

            areaCollider.transform.position = enemy.worldPosition;
            areaCollider.gameObject.SetActive(true);

            yield return new WaitForSeconds(enemy.activeTime);

            areaCollider.gameObject.SetActive(false);

            print("aqui acabo  " + i);
        }

        //yield return new WaitForEndOfFrame();
        //level++;
        //StartCoroutine(StartWave(level));
    }

    private void CheckTap(AreaCollider area)
    {
        isTap = true;
        _areasColliders.First(x => x == area).gameObject.SetActive(false);

        score++;

        _scoreText.text = score.ToString();
        isTap = false;
    }

    private void CheckDisable()
    {
        if (isTap)
        {
            return;
        }

        if (life <= maxLife)
        {
            life--;
            if (life <= 0)
            {
                life = 0;
            }
        }
    }

    private void OnDisable()
    {
        foreach (AreaCollider a in _areasColliders)
        {
            a.OnDisableCollider -= CheckDisable;
        }
        _checkTapAction.OnTapCollider -= CheckTap;
    }
}


[System.Serializable]
public struct Wave
{
    public List<ActiveTime> Activetimes;
    public int WaveLevel;
}
