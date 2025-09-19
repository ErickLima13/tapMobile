using System;
using UnityEngine;
using Zenject;

public class EnemyCollider : MonoBehaviour
{
    [Inject] private CheckTapAction _checkTapAction;

    public event Action<bool> OnTapResult;

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

        ActiveVisual(false);

        OnTapResult?.Invoke(true);

        print("acertei");
    }

    public bool GetVisualStatus()
    {
        return _visual.enabled;
    }

    public void TakeDamage()
    {
        OnTapResult?.Invoke(false);
        print("Dano");
    }

    public Vector2 GetScreenLimits()
    {
        return new Vector2((screenBounds.x - objectWidth) - offset, 3);
    }

    private void OnDisable()
    {
        _checkTapAction.OnTapCollider -= CheckTap;
    }


}
