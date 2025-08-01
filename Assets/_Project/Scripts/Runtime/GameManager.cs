using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AreaCollider[] _areasColliders;

    public CheckTapAction _checkTapAction;

    public int score;
    public int maxLife;
    public int life;

    public List<ActiveTimeData> activetimes = new();

    public bool isTap;

    public TextMeshProUGUI _scoreText;

    private void Start()
    {
        life = maxLife;

        foreach (AreaCollider a in _areasColliders)
        {
            a.OnDisableCollider += CheckDisable;
        }

        _checkTapAction.OnTapCollider += CheckTap;

        StartCoroutine(DelayActive());
    }

    private IEnumerator DelayActive()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < activetimes.Count; i++)
        {
            if (activetimes[i].isLeft)
            {
                print("aqui começo esqueda " + i);

                _areasColliders[0].gameObject.SetActive(true);

                yield return new WaitForSeconds(activetimes[i].activeTime);

                _areasColliders[0].gameObject.SetActive(false);

                print("aqui acabo esqueda " + i);
            }
            else
            {
                print("aqui começo direita " + i);

                _areasColliders[1].gameObject.SetActive(true);

                yield return new WaitForSeconds(activetimes[i].activeTime);

                _areasColliders[1].gameObject.SetActive(false);

                print("aqui acabo direita " + i);
            }
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
