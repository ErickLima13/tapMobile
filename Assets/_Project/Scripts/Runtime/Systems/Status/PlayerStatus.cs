using System;
using UnityEngine;
using Zenject;

public class PlayerStatus : MonoBehaviour
{
    [Inject] private DamagePlayer _damagePlayer;
    [Inject] private CheckTapAction _checkTapAction;

    public event Action<PointType, int> OnUpdateHud;

    [SerializeField] private int _score;
    [SerializeField] private int _maxLife;
    [SerializeField] private int _currentLife;

    [SerializeField] private Animator _playerAnimator;

    private void Start()
    {
        _currentLife = _maxLife;

        _damagePlayer.OnDamageEvent += DamageEvent;
        _checkTapAction.OnEnemyDied += IncreaseScore;
    }

    private void IncreaseScore(PointType value)
    {
        _score++;
        OnUpdateHud?.Invoke(value, _score);
    }

    private void DamageEvent(PointType value)
    {
        TakeDamage();
        OnUpdateHud?.Invoke(value, _currentLife);
    }

    private void TakeDamage()
    {
        if (_currentLife <= _maxLife)
        {
            _currentLife--;
            _playerAnimator.Play("playerHit");
            if (_currentLife <= 0)
            {
                _currentLife = 0;
            }
        }
    }

    private void OnDisable()
    {
        _damagePlayer.OnDamageEvent -= DamageEvent;
        _checkTapAction.OnEnemyDied -= IncreaseScore;
    }
}

