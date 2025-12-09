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


    private void Start()
    {
        _checkTapAction.OnTapCollider += CheckTap;
    }

    private void ActiveVisual(bool value)
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
            OnTapResult?.Invoke(PointType.Score);
        }

        print("acertei");
    }

    public bool GetIsDied() { return died; }

    public void SpawnEnemy(Vector3 position,Enemy enemy)
    {
        _visual.sprite = enemy.Visual;
        lifes = enemy.Lifes;
        transform.position = position;
        ActiveVisual(true);
    }

    private void OnDisable()
    {
        _checkTapAction.OnTapCollider -= CheckTap;
    }
}
