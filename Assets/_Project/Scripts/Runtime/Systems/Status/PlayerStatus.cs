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
        playerAttributes = new(2, 3, 2);
        _currentLife = _maxLife;

        _damagePlayer.OnDamageEvent += DamageEvent;
        // _checkTapAction.OnEnemyDied += IncreaseScore;
        _rewardedAdController.OnRewardEvent += GiveLifeReward;

    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > playerAttributes._timeToAttack)
        {
            timer = 0;

            if (attackObj.Count < playerAttributes._attackCount)
            {
                print("chmaei o attack");

                GameObject temp = _objectPooler.SpawnFromPool("playerAttack", new(0, -5, 0), Quaternion.identity);

                attackObj.Add(temp);

                print("ataqeieigf");

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

