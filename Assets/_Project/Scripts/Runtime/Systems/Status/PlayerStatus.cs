using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private Animator _playerAnimator;

    public PlayerAttributes playerAttributes;

    public List<GameObject> attackObj = new();

    public float timer;

    public int _experience;

    public TestBuilder testBuilder;

    public int level = 1;

    private bool _showLevelUp;

    public Image _weapon;

    [SerializeField] private PlayerData playerData;

    private void Start()
    {
        // Time.timeScale = 2.0f; // aumenta a velocidade do jogo

        playerAttributes = new(3, 2, 1.8f);

        playerData.CurrentLife = playerData.MaxLife;

        playerData.OnPlayerDamage += TakeDamage;

        _damagePlayer.OnDamageEvent += DamageEvent;

        // _checkTapAction.OnEnemyDied += IncreaseScore;

        _rewardedAdController.OnRewardEvent += GiveLifeReward;

        _waveController.OnInscreaXp += IncreaseExperience;

        level = 1;

    }

    private void Update()
    {
        if (_waveController.GetEnemiesInScene() == 0)
        {
            return;
        }

        timer += Time.deltaTime;

        _weapon.fillAmount = timer;

        if (timer > playerAttributes.AttackTime)
        {
            timer = 0;

            for(int i = 0; i < playerAttributes.AttackCount;i++)
            {
                GameObject temp = _objectPooler.SpawnFromPool("playerAttack", new(0, -2, 0), Quaternion.identity);
            }
        }

    }

    private void IncreaseExperience(int xp)
    {
        _experience += xp;

        if(!_showLevelUp)
            CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        List<PlayerAttributes> tempList = new();

        if (_experience >= 30 * level)
        {
            for (int i = 0; i <= 2; i++)
            {
                int idTemp = i;
                tempList.Add(CreateAttributes());
                testBuilder.CreateChooseButton(tempList[i], () => IncreaseAttributes(tempList[idTemp]));

                Time.timeScale = 0;

                _showLevelUp = true;
            }
        }
    }

    private void IncreaseAttributes(PlayerAttributes attributes)
    {
        playerAttributes.AttackCount += attributes.AttackCount;
        playerAttributes.AttackSpeed += attributes.AttackSpeed;

        if (playerAttributes.AttackTime > 0.2f)
        {
            playerAttributes.AttackTime -= attributes.AttackTime;
        }

        testBuilder.ClearOptions();

        Time.timeScale = 1;

        level++;
        _showLevelUp = false;
    }

    private PlayerAttributes CreateAttributes()
    {
        PlayerAttributes attributes = new();

        attributes.AttackCount = Random.Range(-1, 2);
        attributes.AttackSpeed = Random.Range(-0.02f, 0.02f);
        attributes.AttackTime = Random.Range(-0.02f, 0.02f);

        return attributes;
    }


    private void GiveLifeReward()
    {
        playerData.CurrentLife = 1;
    }

    private void IncreaseScore(PointType value)
    {
        _score++;
        OnUpdateHud?.Invoke(value, _score);
    }

    private void DamageEvent(PointType value)
    {
      //  TakeDamage();
      //  OnUpdateHud?.Invoke(value, _currentLife);
    }

    private void TakeDamage()
    {
        _playerAnimator.Play("playerHit");
    }

    private void OnDisable()
    {
        _damagePlayer.OnDamageEvent -= DamageEvent;
        //_checkTapAction.OnEnemyDied -= IncreaseScore;
        _rewardedAdController.OnRewardEvent -= GiveLifeReward;

        _waveController.OnInscreaXp -= IncreaseExperience;

    }
}

