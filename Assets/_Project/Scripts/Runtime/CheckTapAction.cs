using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckTapAction : MonoBehaviour
{
    private Vector3 _curScreenPos;
    private Camera _mainCamera;

    public event Action OnTapCollider;

    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private LayerMask _layerCollider;

    public GameObject _clickedObject;

    private Vector3 WorldPos
    {
        get
        {
            return _mainCamera.ScreenToWorldPoint(_curScreenPos);
        }
    }

    private bool IsClickedOn
    {
        get
        {
            var hits = Physics2D.RaycastAll(WorldPos, Vector2.zero, float.MaxValue, _layerCollider);

            if (hits != null && hits.Length > 0)
            {
                _clickedObject = hits[0].collider.gameObject;
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputActions.FindAction("Point").performed += context => { _curScreenPos = context.ReadValue<Vector2>(); };
        _inputActions.FindAction("Click").performed += _ => { if (IsClickedOn) OnTapCollider?.Invoke(); };
    }

    private void OnDisable()
    {
        _inputActions.FindAction("Point").performed -= context => { _curScreenPos = context.ReadValue<Vector2>(); };
        _inputActions.FindAction("Click").performed -= _ => { if (IsClickedOn) OnTapCollider?.Invoke(); };

    }
}
