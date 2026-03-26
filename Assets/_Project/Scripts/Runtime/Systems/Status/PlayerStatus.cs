using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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

    public int _experience;

    public TestBuilder testBuilder;

    private void Start()
    {
        // Time.timeScale = 2.0f; // aumenta a velocidade do jogo

        playerAttributes = new(6, 2, 1.5f);
        _currentLife = _maxLife;

        _damagePlayer.OnDamageEvent += DamageEvent;
        // _checkTapAction.OnEnemyDied += IncreaseScore;
        _rewardedAdController.OnRewardEvent += GiveLifeReward;

        _waveController.OnInscreaXp += IncreaseExperience;

    }

    private void Update()
    {
        if (_waveController.GetEnemiesInScene() == 0)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer > playerAttributes.TimeToAttack)
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

    private void IncreaseExperience(int xp)
    {
        _experience += xp;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        List<PlayerAttributes> tempList = new();

        if (_experience % 8 == 0)
        {
            for (int i = 0; i <= 2; i++)
            {
                int idTemp = i;
                tempList.Add(CreateAttributes());
                testBuilder.CreateChooseButton(tempList[i], () => IncreaseAttributes(tempList[idTemp]));

                Time.timeScale = 0;
            }
        }
    }

    private void IncreaseAttributes(PlayerAttributes attributes)
    {
        playerAttributes.AttackCount += attributes.AttackCount;
        playerAttributes.AttackSpeed += attributes.AttackSpeed;

        if(playerAttributes.TimeToAttack > 0.2f)
        {
            playerAttributes.TimeToAttack -= attributes.TimeToAttack;
        }

        testBuilder.ClearOptions();

        Time.timeScale = 1;
    }

    private PlayerAttributes CreateAttributes()
    {
        PlayerAttributes attributes = new();

        attributes.AttackCount = Random.Range(0, 3);
        attributes.AttackSpeed = Random.Range(0, 3);
        attributes.TimeToAttack = Random.Range(0f, 0.3f);

        return attributes;
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

        _waveController.OnInscreaXp -= IncreaseExperience;

    }
}

