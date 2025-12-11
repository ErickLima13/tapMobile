using System;
using UnityEngine;
using Zenject;

public enum PointType
{
    Score,
    Damage
}

public class EnemyCollider : MonoBehaviour, IPooledObject
{
    [Inject] private CheckTapAction _checkTapAction;
    [Inject] private ObjectPooler _objectPooler;

    public event Action<PointType> OnTapResult;

    public int lifes;

    public bool died;

    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private EnemyChase _enemyChase;
    [SerializeField] private BoxCollider2D _boxCollider;

    public void ActiveVisual(bool value)
    {
        _visual.enabled = value;
    }

    private void CheckTap(EnemyCollider area)
    {
        if (area != this)
        {
            return;
        }

        if (lifes > 1)
        {
            lifes--;
        }
        else
        {
            died = true;
            OnTapResult?.Invoke(PointType.Score);
            _objectPooler.ReturnToPool("enemy", gameObject);
        }

        print("acertei");
    }

    public bool GetIsDied() { return died; }

    public void SpawnEnemy(Enemy enemy)
    {
        _checkTapAction.OnTapCollider += CheckTap;
        _visual.sprite = enemy.Visual;
        lifes = enemy.Lifes;
        _enemyChase.speed = enemy.Speed;
        _enemyChase.isReady = true;
        died = false;
        ActiveVisual(true);
    }

    private void OnDisable()
    {
        _checkTapAction.OnTapCollider -= CheckTap;
    }

    public void OnObjectSpawn()
    {
    }
}
