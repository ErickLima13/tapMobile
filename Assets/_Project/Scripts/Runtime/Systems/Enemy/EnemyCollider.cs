using System;
using UnityEngine;
using Zenject;

public class EnemyCollider : MonoBehaviour
{
    [Inject] private CheckTapAction _checkTapAction;

    [SerializeField] private bool isTap;

    public event Action<bool> OnTapResult;

    public ScreenPositions position;

    private SpriteRenderer _visual;

    private void Start()
    {
        _visual = GetComponentInChildren<SpriteRenderer>();
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
            print("diferente");
            return;
        }

        isTap = true;
        ActiveVisual(false);

        OnTapResult?.Invoke(true);

        isTap = false;

        print("acertei");
    }


    public void CheckDisable()
    {
        if (isTap || !_visual.enabled)
        {
            return;
        }

        OnTapResult?.Invoke(false);

        print("desativou");
    }

    private void OnDisable()
    {
        _checkTapAction.OnTapCollider -= CheckTap;
    }
}
