using System;
using UnityEngine;
using Zenject;

public class DamagePlayer : MonoBehaviour
{
    [Inject] private ObjectPooler _objectPooler;

    public event Action<PointType> OnDamageEvent;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyCollider>(out EnemyCollider enemy))
        {
            OnDamageEvent?.Invoke(PointType.Damage);
            enemy.died = true;
            _objectPooler.ReturnToPool("enemy", enemy.gameObject);
        }
    }

}
