using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
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

    private SpawnEffect _spawnEffect;

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

    public async Task SpawnEnemy(Enemy enemy)
    {
        _boxCollider.enabled = false;

        await UniTask.WaitUntil(() => !_spawnEffect.IsAnimation());

        _objectPooler.ReturnToPool("effect", _spawnEffect.gameObject);

        _checkTapAction.OnTapCollider += CheckTap;

        lifes = enemy.Lifes;
        died = false;

        _visual.sprite = enemy.Visual;

        _enemyChase.speed = enemy.Speed;
        _enemyChase.isReady = true;

        ActiveVisual(true);

        _boxCollider.enabled = true;
    }

    private void OnDisable()
    {
        _visual.sprite = null;
        _checkTapAction.OnTapCollider -= CheckTap;
    }

    public void OnObjectSpawn()
    {
        var temp = _objectPooler.SpawnFromPool("effect", transform.position, Quaternion.identity);
        _spawnEffect = temp.GetComponent<SpawnEffect>();
    }
}
