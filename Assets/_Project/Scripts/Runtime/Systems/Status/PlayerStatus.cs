using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using static PlayerAttack;

public class PlayerStatus : MonoBehaviour
{
    [Inject] private DamagePlayer _damagePlayer;
    // [Inject] private CheckTapAction _checkTapAction;
    [Inject] private RewardedAdController _rewardedAdController;

    [Inject] private ObjectPooler _objectPooler;
    [Inject] private WaveController _waveController;

    public event Action<PointType, int> OnUpdateHud;

    public event Action OnGameOver;

    [SerializeField] private int _score;
    [SerializeField] private int _maxLife;
    [SerializeField] private int _currentLife;

    [SerializeField] private Animator _playerAnimator;

    public PlayerAttributes playerAttributes;

    public List<GameObject> attackObj = new();

    public float timer;

    private void Start()
    {
        Time.timeScale = 3.0f; // aumenta a velocidade do jogo

        playerAttributes = new(4, 4, 0.5f);
        _currentLife = _maxLife;

        _damagePlayer.OnDamageEvent += DamageEvent;
        // _checkTapAction.OnEnemyDied += IncreaseScore;
        _rewardedAdController.OnRewardEvent += GiveLifeReward;

    }

    private void Update()
    {
        if(_waveController.GetEnemiesInScene() == 0)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer > playerAttributes.TimeToAttack)
        {
            timer = 0;

            if (attackObj.Count < playerAttributes.AttackCount)
            {
                GameObject temp = _objectPooler.SpawnFromPool("playerAttack", new(0, -2, 0), Quaternion.identity);
                attackObj.Add(temp);
            }
            else
            {
                attackObj.Clear();
            }
        }

    }


    private void GiveLifeReward()
    {
        _currentLife = 1;
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
                OnGameOver?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        _damagePlayer.OnDamageEvent -= DamageEvent;
        //_checkTapAction.OnEnemyDied -= IncreaseScore;
        _rewardedAdController.OnRewardEvent -= GiveLifeReward;
    }
}

