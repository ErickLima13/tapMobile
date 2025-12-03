using Maneuver.SoundSystem;
using System;
using UnityEngine;
using Zenject;

public class SpawnFireEffect : MonoBehaviour
{
    [Inject] private IAudioManager _audioManager;

    [SerializeField] private AudioFileObject _fireballVfx;


    public event Action<float> OnAnimation;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(Vector3 position)
    {
        _audioManager.Play(_fireballVfx);
        transform.position = position;
        _animator.Play("explosionTouch");
        OnAnimation?.Invoke(position.x);
    }

}
