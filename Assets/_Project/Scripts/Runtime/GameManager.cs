using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private EnemyCollider[] _enemiesColliders;

    [SerializeField] private int score;
    [SerializeField] private int maxLife;
    [SerializeField] private int life;
    [SerializeField] private bool isTap;

    [SerializeField] private CheckTapAction _checkTapAction;
    [SerializeField] private TextMeshProUGUI _scoreText;


    private void Start()
    {
        life = maxLife;

        foreach (EnemyCollider a in _enemiesColliders)
        {
            a.OnDisableCollider += CheckDisable;
            a.gameObject.SetActive(false);
        }

        _checkTapAction.OnTapCollider += CheckTap;
    }

    private void CheckTap(EnemyCollider area)
    {
        isTap = true;
        _enemiesColliders.First(x => x == area).gameObject.SetActive(false);

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
        foreach (EnemyCollider a in _enemiesColliders)
        {
            a.OnDisableCollider -= CheckDisable;
        }
        _checkTapAction.OnTapCollider -= CheckTap;
    }
}

