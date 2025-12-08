using Maneuver.SoundSystem;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class MenuController : MonoBehaviour
{
    [Inject] private IAudioManager _audioManager;

    [SerializeField] private AudioFileObject _musicMenu;


    private void Awake()
    {
        _audioManager.Play(_musicMenu);
    }

    private void Start()
    {
        //_audioManager.Play(_musicMenu);
        Time.timeScale = 1;
       
    }

    public void StopMenuMusic()
    {
        _audioManager.Stop(_musicMenu);
    }
}
