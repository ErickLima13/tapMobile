using System;
using UnityEngine;

public class SpawnFireEffect : MonoBehaviour
{
    public event Action<float> OnAnimation;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(Vector3 position)
    {
        transform.position = position;
        _animator.Play("explosionTouch");
        OnAnimation?.Invoke(position.x);
    }

}
