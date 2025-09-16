using System;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public ScreenPositions position;

    public event Action OnDisableCollider;

    private void OnDisable()
    {
        OnDisableCollider?.Invoke();
    }
}
