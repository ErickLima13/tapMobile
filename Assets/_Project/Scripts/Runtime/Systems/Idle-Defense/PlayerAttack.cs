using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour, IPooledObject
{
    [Inject] private ObjectPooler _objectPooler;
    [Inject] private PlayerStatus _playerStatus;
    [Inject] private WaveController _waveController;

    public event Action<PointType> OnAttackEvent;

    public EnemyCollider _target;

    public PlayerAttributes _playerAttributes;

    private List<EnemyCollider> enemyColliders = new();

    public void CheckArea()
    {
        if (_waveController.GetCurrentWave().Count > 0)
        {
            enemyColliders.AddRange(_waveController.GetCurrentWave());
            int idRand = Random.Range(0, enemyColliders.Count);
            _target = enemyColliders[idRand];
            RotateTheEnemy();
        }
    }

    private void Update()
    {
        _playerAttributes = _playerStatus.playerAttributes; // tirar quando fizer mecanica roguelike

        if (_waveController.GetEnemiesInScene() == 0)
        {
            return;
        }

        if (_target == null || !_target.gameObject.activeSelf)
        {
            CheckArea();
        }

        Movement();
    }

    private void Movement()
    {
        if (_target != null)
        {
            Vector3 newTarget = Vector3.MoveTowards(transform.position, _target.gameObject.transform.position,
                _playerAttributes.AttackSpeed * Time.deltaTime);
            transform.position = newTarget;
        }
    }

    private void RotateTheEnemy()
    {
        Vector3 direction = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        Quaternion rotation = transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyCollider>(out EnemyCollider enemy) == _target)
        {
            if (!enemy.GetIsDied())
            {
                enemy.CheckTap(enemy);
                _objectPooler.ReturnToPool("playerAttack", gameObject);
            }
        }
    }

    public void OnObjectSpawn()
    {       
    }

    private void OnDisable()
    {
        _target = null;
        transform.position = new(0, -2, 0);
    }
}

[System.Serializable]
public struct PlayerAttributes
{
    public float AttackSpeed;
    public int AttackCount;
    public float AttackTime;

    public PlayerAttributes(float attackSpeed, int attackCount, float attackTime)
    {
        AttackSpeed = attackSpeed;
        AttackCount = attackCount;
        AttackTime = attackTime;
    }
}

public enum AttributesType
{
    AttackSpeed,
    AttackCount,
    TimeToAttack
}

