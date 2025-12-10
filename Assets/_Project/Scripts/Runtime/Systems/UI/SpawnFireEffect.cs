using UnityEngine;
using Zenject;

public class SpawnFireEffect : MonoBehaviour, IPooledObject
{
    [Inject] private ObjectPooler _objectPooler;

    [SerializeField] private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnObjectSpawn()
    {
        _animator.Play("explosionTouch");
    }

    public void EndAnimation()
    {
        _objectPooler.ReturnToPool("explosion", gameObject);
    }
}
