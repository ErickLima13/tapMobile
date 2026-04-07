using System;
using UnityEngine;
using Zenject;

public class DamagePlayer : MonoBehaviour
{
    public event Action<PointType> OnDamageEvent;

    [SerializeField] private PlayerData playerData;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyCollider>(out EnemyCollider enemy))
        {
            playerData.DamageEvent(PointType.Damage);
        }
    }

}
