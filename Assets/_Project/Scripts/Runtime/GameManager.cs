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

    public CheckTapAction _checkTapAction;
    public AreaCollider[] _areasColliders;
    public bool isTap;
    public TextMeshProUGUI _scoreText;
    public Vector2 _distance;
    public List<ActiveTimeData> activetimes = new();

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

        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < activetimes.Count; i++)
        {
            var areaCollider = _areasMap[activetimes[i].position];

            print("aqui começo  " + i);

            //float randomX = Random.Range(-_distance.x, _distance.x);
            //float randomY = Random.Range(-_distance.y, _distance.y);
            //areaCollider.transform.position = new(randomX, randomY);

            areaCollider.gameObject.SetActive(true);

            yield return new WaitForSeconds(activetimes[i].activeTime);

            areaCollider.gameObject.SetActive(false);

            print("aqui acabo  " + i);

            yield return new WaitForEndOfFrame();
        }
    }

    private void CheckTap(AreaCollider area)
    {
        isTap = true;

        _areasColliders.First(x => x == area).gameObject.SetActive(false);

        print("tap in time");
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

        //print("disable" + _areasColliders.Length);
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
