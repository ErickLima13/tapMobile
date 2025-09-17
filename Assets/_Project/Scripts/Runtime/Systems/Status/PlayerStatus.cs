using System;
using UnityEngine;
using Zenject;

public class PlayerStatus : MonoBehaviour
{
    [Inject] private EnemyCollider[] _enemiesColliders;

    public event Action<bool, int> OnUpdateHud;

    [SerializeField] private int _score;
    [SerializeField] private int _maxLife;
    [SerializeField] private int _currentLife;
    [SerializeField] private bool _isTap;

    private void Start()
    {
        _currentLife = _maxLife;

        foreach (EnemyCollider a in _enemiesColliders)
        {
            a.OnTapResult += TapResult;
        }
    }

    private void TapResult(bool value)
    {
        if (value)
        {
            _score++;
            OnUpdateHud?.Invoke(true, _score);
        }
        else
        {
            TakeDamage();
            OnUpdateHud?.Invoke(false, _currentLife);
        }
    }

    private void TakeDamage()
    {
        if (_currentLife <= _maxLife)
        {
            _currentLife--;
            if (_currentLife <= 0)
            {
                _currentLife = 0;
            }
        }
    }

    private void OnDisable()
    {
        foreach (EnemyCollider a in _enemiesColliders)
        {
            a.OnTapResult -= TapResult;
        }
    }
}

