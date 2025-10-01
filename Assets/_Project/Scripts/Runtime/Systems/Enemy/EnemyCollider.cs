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

    public ScreenPositions position;

    private SpriteRenderer _visual;

    #region ScreenLimit

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private float offset = 0.10f;
    private Camera main;

    #endregion


    private void Awake()
    {
        _visual = GetComponentInChildren<SpriteRenderer>();
        main = Camera.main;

        screenBounds = main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, main.transform.position.z));
        objectWidth = _visual.bounds.size.x / 2;
        objectHeight = _visual.bounds.size.y / 2;
    }

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

        ActiveVisual(false);

        OnTapResult?.Invoke(PointType.Score);

        print("acertei");
    }

    private void TakeDamage()
    {
        OnTapResult?.Invoke(PointType.Damage);
        print("Dano");
    }

    public Vector2 GetScreenLimits()
    {
        return new Vector2((screenBounds.x - objectWidth) - offset, 3f);
    }

    public void SpawnEnemy(Vector3 position)
    {
        transform.position = position;
        ActiveVisual(true);
    }

    public void CheckDamage()
    {
        if (_visual.enabled)
        {
            ActiveVisual(false);
            TakeDamage();
        }
    }

    private void OnDisable()
    {
        _checkTapAction.OnTapCollider -= CheckTap;
    }


}
