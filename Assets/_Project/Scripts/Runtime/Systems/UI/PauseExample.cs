using UnityEngine;

public class PauseExample : MonoBehaviour
{
    [SerializeField] float lifespan = 2f; // segundos de vida
    float deathTime;                       // Time.time em que ele deve explodir
    bool exploded;

    void OnEnable()
    {
        deathTime = Time.time + lifespan;
        exploded = false;
    }

    void Update()
    {
        if (!exploded && Time.time >= deathTime)
        {
            Explode();
        }
    }

    // Quanto tempo falta AGORA (bom para mostrar em UI, inclusive durante o pause)
    public float RemainingTime => Mathf.Max(0f, deathTime - Time.time);

    void Explode()
    {
        exploded = true;
        // TODO: efeitos, dano, etc.
        Destroy(gameObject);
    }
}
