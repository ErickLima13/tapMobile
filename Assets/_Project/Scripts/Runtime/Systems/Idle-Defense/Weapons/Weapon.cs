using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Weapon : MonoBehaviour, IPooledObject
{
    [Inject] private ObjectPooler _objectPooler;
    [Inject] private PlayerStatus _playerStatus;

    public EnemyCollider _target;

    public EnemyRuntimeLookup enemyRuntimeLookup;

    private List<EnemyCollider> enemyColliders = new();

    public WeaponData _currentWeaponData;

    public SpriteRenderer _visual;

    public void SetWeapon(WeaponData weapon)
    {
        _currentWeaponData = weapon;
        _visual.sprite = _currentWeaponData.WeaponVisual;
    }

    private void Update()
    {
       // _playerAttributes = _playerStatus.playerAttributes; // tirar quando fizer mecanica roguelike

        if (enemyRuntimeLookup.CurrentEnemies.Count == 0)
        {
            return;
        }

        if (_target == null || !_target.gameObject.activeSelf)
        {
            CheckArea();
        }

        Movement();
    }

    public void CheckArea()
    {
        if (enemyRuntimeLookup.CurrentEnemies.Count > 0)
        {
            enemyColliders.AddRange(enemyRuntimeLookup.CurrentEnemies);
            int idRand = Random.Range(0, enemyColliders.Count);
            _target = enemyColliders[idRand];
            RotateTheEnemy();
        }
    }


    private void Movement()
    {
        if (_target != null && _currentWeaponData != null)
        {
            Vector3 newTarget = Vector3.MoveTowards(transform.position, _target.gameObject.transform.position,
                _currentWeaponData.WeaponSpeed * Time.deltaTime);
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
                enemy.CheckLife(enemy);
                _objectPooler.ReturnToPool("playerAttack", gameObject);
            }
        }
    }

    private void OnDisable()
    {
        _target = null;
        transform.position = new(0, -2, 0);
    }

    public void OnObjectSpawn()
    {
        
    }
}
