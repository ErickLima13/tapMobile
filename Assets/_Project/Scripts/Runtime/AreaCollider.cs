using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaCollider : MonoBehaviour
{
    public event Action OnDisableCollider;

    public BoxCollider2D _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = false;
    }

    private void OnDisable()
    {
        OnDisableCollider?.Invoke();
    }

}
