using UnityEngine;

public class SpawnEffect : MonoBehaviour, IPooledObject
{

   [SerializeField] private ParticleSystem _rootParticleSystem;

    public void OnObjectSpawn()
    {
    }

    public bool IsAnimation()
    {
        return _rootParticleSystem.IsAlive(true);
    }
}
