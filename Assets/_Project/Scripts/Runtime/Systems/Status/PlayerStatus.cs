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

    public List<GameObject> attackObj = new();

    public int _experience;

    public TestBuilder testBuilder;

    public int level = 1;

    private bool _showLevelUp;

    public Image[] _weaponsIcon;

    [SerializeField] private PlayerData playerData;

    public List<WeaponData> weaponDatas = new();

    public Dictionary<WeaponData,float> weaponTime = new();

    public bool hasWeaponToUnlock;

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
            if (IsWeaponLiberate(weaponDatas[i]) && !weaponTime.ContainsKey(weaponDatas[i]))
            {
                weaponTime.Add(weaponDatas[i], weaponDatas[i].WeaponTime);
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
                weaponTime[weaponDatas[i]] += Time.deltaTime; 

                _weaponsIcon[i].fillAmount = weaponTime[weaponDatas[i]];

                if (weaponTime[weaponDatas[i]] > weaponDatas[i].WeaponTime)
                {
                    CreateAttack(weaponDatas[i]);
                    weaponTime[weaponDatas[i]] = 0;
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
        if (_experience >= 30 * level)
        {
            //depois que funcionar mudar uma porcentagem mais complexa

            int sort = Random.Range(0, 100);

            if (sort <= 25)
            {
                hasWeaponToUnlock = false;
            }

            for (int i = 0; i <= 2; i++)
            {
                int idTemp = i;
                WeaponData temp = GetWeapon();
                WeaponAttributes attributes = CreateAttributes();
                attributes.WeaponName = temp.WeaponName;

                if (!hasWeaponToUnlock && !CheckAllWeaponUnlock())
                {
                    testBuilder.CreateUnlockWeaponButton(attributes, () => UnlockWeapon(temp));
                    hasWeaponToUnlock = true;
                }
                else
                {
                    testBuilder.CreateAttributesButton(attributes, () => IncreaseAttributes(temp, attributes));
                }

                Time.timeScale = 0;

                _showLevelUp = true;
            }
        }
    }

    private bool CheckAllWeaponUnlock()
    {
        foreach (WeaponData weapon in weaponDatas)
        {
            if (!IsWeaponLiberate(weapon))
            {
                return false;
            }
        }

        return true;
    }

    private WeaponData GetWeapon()
    {
        List<WeaponData> tempList = new();
        List<WeaponData> blockeds = new();

        foreach(WeaponData weapon in weaponDatas)
        {
            if (IsWeaponLiberate(weapon))
            {
                tempList.Add(weapon);
            }
            else
            {
                blockeds.Add(weapon);
            }
        }

        if (!hasWeaponToUnlock && blockeds.Count > 0)
        {
            return weaponDatas[weaponDatas.IndexOf(blockeds[0])];
        }

        int idRand = Random.Range(0, tempList.Count);

        return weaponDatas[weaponDatas.IndexOf(tempList[idRand])];
    }

    private void UnlockWeapon(WeaponData weapon)
    {
        weaponDatas[weaponDatas.IndexOf(weapon)].WeaponLiberates = true;
        AddWeaponTime();
        ResultChoose();
    }

    private void IncreaseAttributes(WeaponData attributes, WeaponAttributes weaponAttributes)
    {
        attributes.WeaponCount += weaponAttributes.WeaponCount;
        attributes.WeaponDamage += weaponAttributes.WeaponDamage;

        ResultChoose();
    }

    private void ResultChoose()
    {
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

