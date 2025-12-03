using Maneuver.SoundSystem;
using UnityEngine;
using Zenject;

public class MenuController : MonoBehaviour
{
    [Inject] private IAudioManager _audioManager;

    [SerializeField] private AudioFileObject _musicMenu;

    private void Start()
    {
        Time.timeScale = 1;
        _audioManager.Play(_musicMenu);
    }


    public void StopMenuMusic()
    {
        _audioManager.Stop(_musicMenu);
    }
}
