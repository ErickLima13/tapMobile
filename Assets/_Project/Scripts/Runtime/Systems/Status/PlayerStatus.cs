using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerStatus : MonoBehaviour
{
    [Inject] private RewardedAdController _rewardedAdController;

    [Inject] private ObjectPooler _objectPooler;
    [Inject] private WaveController _waveController;

    public event Action<PointType, int> OnUpdateHud;

    public event Action OnGameOver;

    [SerializeField] private int _score;

    [SerializeField] private Animator _playerAnimator;

    public WeaponAttributes playerAttributes;

    public List<GameObject> attackObj = new();

    public int _experience;

    public TestBuilder testBuilder;

    public int level = 1;

    private bool _showLevelUp;

    public Image[] _weaponsIcon;

    [SerializeField] private PlayerData playerData;

    public List<WeaponData> weaponDatas = new();

    public List<float> weaponTime = new();

    private void Start()
    {
        // Time.timeScale = 2.0f; // aumenta a velocidade do jogo

        playerData.CurrentLife = playerData.MaxLife;
        playerData.EndGame = false;

        playerData.OnPlayerDamage += TakeDamage;

        _rewardedAdController.OnRewardEvent += GiveLifeReward;

        _waveController.OnInscreaXp += IncreaseExperience;

        level = 1;

        AddWeaponTime();
    }

    private void AddWeaponTime()
    {
        for (int i = 0; i < weaponDatas.Count; i++)
        {
            if (IsWeaponLiberate(weaponDatas[i]))
            {
                weaponTime.Add(weaponDatas[i].WeaponTime);
            }
        }
    }

    public bool IsWeaponLiberate(WeaponData weapon) => weapon.WeaponLiberates;
 
    private void Update()
    {
        if (_waveController.GetEnemiesInScene() == 0 || playerData.EndGame)
        {
            return;
        }

        for (int i = 0; i < weaponDatas.Count; i++)
        {
            if (IsWeaponLiberate(weaponDatas[i]))
            {
                weaponTime[i] += Time.deltaTime;

                _weaponsIcon[i].fillAmount = weaponTime[i];

                if (weaponTime[i] > weaponDatas[i].WeaponTime)
                {
                    CreateAttack(weaponDatas[i]);
                    weaponTime[i] = 0;
                }
            }   
        }
    }

    private void CreateAttack(WeaponData weaponData)
    {
        for (int i = 0; i < weaponData.WeaponCount; i++)
        {
            GameObject temp = _objectPooler.SpawnFromPool("playerAttack", weaponData.WeaponPosition, 
                Quaternion.identity);
            temp.GetComponent<Weapon>().SetWeapon(weaponData);
        }
    }

    private void IncreaseExperience(int xp)
    {
        _experience += xp;

        if (!_showLevelUp)
            CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        List<WeaponAttributes> tempList = new();

        if (_experience >= 30 * level)
        {
            CheckAllWeapons();

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

    private void IncreaseAttributes(WeaponAttributes attributes)
    {
        playerAttributes.WeaponCount += attributes.WeaponCount;
        playerAttributes.WeaponDamage += attributes.WeaponDamage;

        testBuilder.ClearOptions();

        Time.timeScale = 1;

        level++;
        _showLevelUp = false;
    }

    private WeaponAttributes CreateAttributes()
    {
        WeaponAttributes attributes = new();

        attributes.WeaponCount = Random.Range(-1, 2);
        attributes.WeaponDamage = Random.Range(-1, 2);

        return attributes;
    }

    private bool CheckAllWeapons()
    {
        for (int i = 0; i < weaponDatas.Count; i++)
        {
            if (!IsWeaponLiberate(weaponDatas[i]))
            {
                print("nem todas armas liberadas");
                return false;
            }    
        }
        print("todas armas liberadas");
        return true;
    }

    private void GiveLifeReward()
    {
        playerData.CurrentLife = 1;
    }

    private void TakeDamage()
    {
        _playerAnimator.Play("playerHit");
    }

    private void OnDisable()
    {
        _rewardedAdController.OnRewardEvent -= GiveLifeReward;
        _waveController.OnInscreaXp -= IncreaseExperience;
    }
}

