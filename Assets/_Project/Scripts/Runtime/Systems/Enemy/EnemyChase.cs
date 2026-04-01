using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public event Action OnTheEnd;

    public Transform _target;
    public float speed;

    public bool isReady;

    public bool isInTheEnd;

    public float _startPosX;

    private void Start()
    {
        _target = GameObject.FindWithTag("Player").transform;

        _startPosX = transform.position.x;
    }

    private void Update()
    {
        if (isInTheEnd) { return; }

        if (isReady)
        {
            Vector3 newTarget = Vector3.MoveTowards(transform.position, new(_startPosX, _target.transform.position.y),
               speed * Time.deltaTime);
            transform.position = newTarget;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<DamagePlayer>(out DamagePlayer damage))
        {
            isReady = false;
            isInTheEnd = true;

            OnTheEnd?.Invoke();
        }
    }

    public async Task DelayHit()
    {
        if (isInTheEnd) { return; }

        isReady = false;

        await UniTask.WaitForSeconds(0.3f);

        isReady = true;
    }

   
}
