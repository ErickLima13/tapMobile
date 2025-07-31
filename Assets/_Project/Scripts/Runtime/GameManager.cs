using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AreaCollider[] _areasColliders;

    public CheckTapAction _checkTapAction;

    public int score;
    public int life;
    public int maxLife;

    public List<ActiveTimeData> activetimes = new();

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
                _areasColliders[0]._collider.enabled = true;
                yield return new WaitForSeconds(activetimes[i].activeTime);

                print("aqui esqueda");
            }
            else
            {
                _areasColliders[1]._collider.enabled = true;
                yield return new WaitForSeconds(activetimes[i].activeTime);
                print("aqui direita");
            }

            _areasColliders[i]._collider.enabled = false;
            _areasColliders[i].gameObject.SetActive(false);

            print("aqui acabo");

        }
    }

    private void CheckTap()
    {
        print("tap in time");
        score++;
    }

    private void CheckDisable()
    {
        if (life <= maxLife)
        {
            life--;

            if (life <= 0)
            {
                life = 0;
            }
        }


        print("disable" + _areasColliders.Length);
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
