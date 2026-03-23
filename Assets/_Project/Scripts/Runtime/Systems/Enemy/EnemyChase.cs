using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyChase : MonoBehaviour
{
    public Transform target;
    public float speed;

    public bool isReady;

    public bool isInTheEnd;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (isReady)
        {
            transform.Translate(speed * Time.unscaledDeltaTime * Vector3.down);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<DamagePlayer>(out DamagePlayer damage))
        {
            isReady = false;
            isInTheEnd = true;
        }         
    }

    public async Task DelayHit()
    {
        if(isInTheEnd) { return; }

        isReady = false;

        await UniTask.WaitForSeconds(0.3f);

        isReady = true;
    }


}
