using System;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public event Action<PointType> OnDamageEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyCollider>(out EnemyCollider enemy))
        {
            OnDamageEvent?.Invoke(PointType.Damage);
        }
    }

}
