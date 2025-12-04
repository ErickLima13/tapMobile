using UnityEngine;

public class CreateSettingsPanel : MonoBehaviour
{
    private GameObject _settingsInScene;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _settingsPrefab;
    [SerializeField] private Transform _parentSettings;

    private void Update()
    {
        if (_canvasGroup.alpha == 1)
        {
            return;
        }

        if (!_settingsInScene.activeSelf)
        {
            _canvasGroup.alpha = 1;
        }
    }

    public void CreateSettings()
    {
        _canvasGroup.alpha = 0;

        if (_settingsInScene == null)
        {
            GameObject temp = Instantiate(_settingsPrefab, _parentSettings);
            _settingsInScene = temp;
        }
        else
        {
            _settingsInScene.SetActive(true);
        }
    }
}
