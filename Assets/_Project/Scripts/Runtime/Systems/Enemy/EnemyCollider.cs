using System;
using UnityEngine;
using Zenject;

public enum PointType
{
    Score,
    Damage
}

public class EnemyCollider : MonoBehaviour
{
    [Inject] private CheckTapAction _checkTapAction;

    public event Action<PointType> OnTapResult;

    public int lifes;

    private bool died;

    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private EnemyChase _enemyChase;
    [SerializeField] private BoxCollider2D _boxCollider;

    private void Start()
    {
        _checkTapAction.OnTapCollider += CheckTap;
    }

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
            ActiveVisual(false);
            died = true;
            _boxCollider.enabled = false;
            _enemyChase.speed = 0;
            OnTapResult?.Invoke(PointType.Score);
        }

        print("acertei");
    }

    public bool GetIsDied() { return died; }

    public void SpawnEnemy(Enemy enemy,Vector3 position)
    {
        _visual.sprite = enemy.Visual;
        lifes = enemy.Lifes;
        _enemyChase.speed = enemy.Speed;
        _enemyChase.isReady = true;
        transform.position = position;
        ActiveVisual(true);
    }

    private void OnDisable()
    {
        _checkTapAction.OnTapCollider -= CheckTap;
    }
}
