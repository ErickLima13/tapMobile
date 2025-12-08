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

    [SerializeField] private EnemyVisual _enemyVisual;
    [SerializeField] private Enemy _enemySO;

    private SpriteRenderer _visual;

    public int lifes;

    private bool isDied;

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


        _visual.sprite = _enemySO.Visual;
        lifes = _enemySO.Lifes;

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

        if (lifes > 1)
        {
            lifes--;
        }
        else
        {
            ActiveVisual(false);
            isDied = true;
            OnTapResult?.Invoke(PointType.Score);
        }

        print("acertei");
    }

    public bool GetIsDied() { return isDied; }

    public Vector2 GetScreenLimits()
    {
        return new Vector2((screenBounds.x - objectWidth) - offset, 1.8f);
    }

    public void SpawnEnemy(Vector3 position)
    {
        transform.position = position;
        ActiveVisual(true);
    }

    private void OnDisable()
    {
        _checkTapAction.OnTapCollider -= CheckTap;
    }


}
