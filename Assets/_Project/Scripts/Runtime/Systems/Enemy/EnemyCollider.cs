using Cysharp.Threading.Tasks;
using Maneuver.SoundSystem;
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
    //[Inject] private CheckTapAction _checkTapAction;
    [Inject] private ObjectPooler _objectPooler;
    [Inject] private IAudioManager _audioManager;

    public event Action<int> OnDied;
    public int lifes;

    public bool died;

    [SerializeField] private SpriteRenderer _visual;
    [SerializeField] private EnemyChase _enemyChase;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private Light2D _light2d;

    private SpawnEffect _spawnEffect;

    public Enemy _currentEnemyData;

    [SerializeField] private AudioFileObject _fireballVfx;

    public EnemyRuntimeLookup enemyRuntimeLookup;
    [SerializeField] private PlayerData playerData;

    private void OnEnable()
    {
        _enemyChase.OnTheEnd += DamageInTheEnd;
    }

    public void ActiveVisual(bool value)
    {
        _visual.enabled = value;
        _light2d.enabled = value;
    }

    private void DamageInTheEnd()
    {
        _ = DelayDamage();
    }

    private async Task DelayDamage()
    {
        if (!died)
        {
            await UniTask.WaitForEndOfFrame();

            playerData.DamageEvent(PointType.Damage);

            await UniTask.WaitForSeconds(3);

            DamageInTheEnd();
        }
    }

    public void CheckLife(EnemyCollider area)
    {
        if (area != this)
        {
            return;
        }

        if (lifes > 1)
        {
            lifes--;
            _ = _enemyChase.DelayHit();
        }
        else
        {
            died = true;
            _objectPooler.SpawnFromPool("explosion", transform.position, Quaternion.identity);
            _audioManager.Play(_fireballVfx);
            _objectPooler.ReturnToPool("enemy", gameObject);
            OnDied?.Invoke(_currentEnemyData.Lifes);

            playerData.IncreaseScore(PointType.Score);
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
        _currentEnemyData = enemy;

        _boxCollider.enabled = false;

        _ = EffectAnimation();

        //_checkTapAction.OnTapCollider += CheckTap;

        lifes = enemy.Lifes;
        died = false;

        _visual.sprite = enemy.Visual;

        _enemyChase.speed = enemy.Speed;
        _enemyChase.isReady = true;
        _enemyChase.isInTheEnd = false;

        ActiveVisual(true);

        _boxCollider.enabled = true;

        enemyRuntimeLookup.Register(this);
    }

    private void OnDisable()
    {
        transform.position = Vector3.one;
        _visual.sprite = null;
        _light2d.enabled = false;

        enemyRuntimeLookup.Unregister(this);

        _enemyChase.OnTheEnd -= DamageInTheEnd;


        // _checkTapAction.OnTapCollider -= CheckTap;
    }


    public void OnObjectSpawn()
    {
        var temp = _objectPooler.SpawnFromPool("effect", transform.position, Quaternion.identity);
        _spawnEffect = temp.GetComponent<SpawnEffect>();
    }
}
