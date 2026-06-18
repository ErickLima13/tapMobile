using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider[] _volumeSliders;
    [SerializeField] private Toggle[] _muteToogles;

    private void Start()
    {
        SetVolumeBGM(_volumeSliders[0].value);
        SetVolumeSFX(_volumeSliders[1].value);
    }

    public void SetVolumeBGM(float volume)
    {
        if (volume > _volumeSliders[0].minValue)
        {
            _muteToogles[0].isOn = false;
        }

        float dBValue = Mathf.Log10(volume) * 20;

        _audioMixer.SetFloat("MusicVolume", dBValue);
    }

    public void SetVolumeSFX(float volume)
    {
        if (volume > _volumeSliders[1].minValue)
        {
            _muteToogles[1].isOn = false;
        }

        float dBValue = Mathf.Log10(volume) * 20;

        _audioMixer.SetFloat("VfxVolume", dBValue);
    }

    public void MuteVolumeBMG(bool value)
    {
        if (value)
        {
            _volumeSliders[0].value = -80;
            _audioMixer.SetFloat("MusicVolume", -80);
        }
        else
        {
            if (_volumeSliders[0].value > _volumeSliders[0].minValue)
            {
                return;
            }

            _volumeSliders[0].value = 1;
            _audioMixer.SetFloat("MusicVolume", 1);
        }

    }

    public void MuteVolumeSFX(bool value)
    {
        if (value)
        {
            _volumeSliders[1].value = -80;
            _audioMixer.SetFloat("VfxVolume", -80);
        }
        else
        {
            if (_volumeSliders[1].value > _volumeSliders[1].minValue)
            {
                return;
            }

            _volumeSliders[1].value = 1;
            _audioMixer.SetFloat("VfxVolume", 1);
        }

    }
}
