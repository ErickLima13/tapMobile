using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
    [SerializeField] private Light2D _light2d;

    private SpawnEffect _spawnEffect;

    public void ActiveVisual(bool value)
    {
        _visual.enabled = value;
        _light2d.enabled = value;
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
    }

    public bool GetIsDied() { return died; }

    private async Task EffectAnimation()
    {
        await UniTask.WaitUntil(() => !_spawnEffect.IsAnimation());

        _objectPooler.ReturnToPool("effect", _spawnEffect.gameObject);
    }

    public void SpawnEnemy(Enemy enemy)
    {
        _boxCollider.enabled = false;

        _ = EffectAnimation();

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
        transform.position = Vector3.zero;
        _visual.sprite = null;
        _light2d.enabled = false;
        _checkTapAction.OnTapCollider -= CheckTap;
    }

    public void OnObjectSpawn()
    {
        var temp = _objectPooler.SpawnFromPool("effect", transform.position, Quaternion.identity);
        _spawnEffect = temp.GetComponent<SpawnEffect>();
    }
}
