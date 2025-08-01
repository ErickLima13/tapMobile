using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaCollider : MonoBehaviour
{
    public ScreenPositions position;
    public event Action OnDisableCollider;

    private void OnDisable()
    {
        OnDisableCollider?.Invoke();
    }
}
