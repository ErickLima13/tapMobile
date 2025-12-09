using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform target;
    public float speed;

    public bool isReady;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (isReady)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.down);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isReady = false;
    }

}
