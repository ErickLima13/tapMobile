using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform target;
    public float speed;

    public bool isready;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (isready)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.down);
        }

        print(Vector3.Distance(transform.position, target.position));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isready = false;
    }

}
