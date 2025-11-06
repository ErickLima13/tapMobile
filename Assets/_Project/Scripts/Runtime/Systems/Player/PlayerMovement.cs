using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private SpawnFireEffect _spawnFireEffect;

    private void Start()
    {
        _spawnFireEffect.OnAnimation += SetPosition;
    }

    private void OnDisable()
    {
        _spawnFireEffect.OnAnimation -= SetPosition;
    }

    private void SetPosition(float x)
    {
        transform.position = new(x, transform.position.y);
    }
}
