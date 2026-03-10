using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerAttack : MonoBehaviour, IPooledObject
{
    [Inject] private ObjectPooler _objectPooler;
    [Inject] private PlayerStatus _playerStatus;

    public event Action<PointType> OnAttackEvent;

    public EnemyCollider _target;

    public PlayerAttributes _playerAttributes;

    private void OnEnable()
    {
        CheckArea();
    }

    private void CheckArea()
    {
        _playerAttributes = _playerStatus.playerAttributes;
        _target = FindFirstObjectByType<EnemyCollider>();     
        Debug.Log("Found a collider: " + _target.name);
    }

    private void Update()
    {
        Attack();
    }

    private void Attack()
    {
        if (_target != null)
        {
            Vector3 newTarget = Vector3.MoveTowards(transform.position, _target.gameObject.transform.position,
                _playerAttributes._attackSpeed * Time.deltaTime);
            transform.position = newTarget;
        }
        else
        {
            CheckArea();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyCollider>(out EnemyCollider enemy))
        {
            if (!enemy.GetIsDied())
            {
                enemy.CheckTap(enemy);
                
                _objectPooler.ReturnToPool("playerAttack", gameObject);
            }
            else
            {
                CheckArea();
            }
        }
    }

    public void OnObjectSpawn()
    {
        CheckArea();
    }

    private void OnDisable()
    {
        _target = null;
        transform.position = new(0, -5, 0);
    }

    [System.Serializable]
    public struct PlayerAttributes
    {
        public float _attackSpeed;
        public int _attackCount;
        public float _timeToAttack;

        public PlayerAttributes(float attackSpeed, int attackCount, float timeToAttack)
        {
            _attackSpeed = attackSpeed;
            _attackCount = attackCount;
            _timeToAttack = timeToAttack;
        }
    }
}


