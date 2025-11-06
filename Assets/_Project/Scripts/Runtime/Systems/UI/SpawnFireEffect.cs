using UnityEngine;

public class SpawnFireEffect : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(Vector3 position)
    {
        transform.position = position;
        _animator.Play("explosionTouch");
    }

}
