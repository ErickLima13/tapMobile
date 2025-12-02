using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HudController : MonoBehaviour
{
    [Inject] private PlayerStatus _playerStatus;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Image[] _livesImg;
    [SerializeField] private Sprite[] _pauseImg;

    public GameObject _pausePanel;

    public Image _pauseButton;

    public bool isPause;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        UpdateHud(PointType.Score, 0);
        _playerStatus.OnUpdateHud += UpdateHud;
    }

    public void UpdateHud(PointType value, int total)
    {
        if (value == PointType.Score)
        {
            _scoreText.text = total.ToString();
        }
        else
        {
            _livesImg[total].enabled = false;
            print(total);
        }
    }

    public void PauseButtonClicked()
    {
        // delay para quando voltar do pause

        isPause = !isPause;
        _pausePanel.SetActive(isPause);
        _pauseButton.sprite = isPause ? _pauseImg[0] : _pauseImg[1];
        Time.timeScale = isPause ? 0f : 1f;
    }

    private void OnDisable()
    {
        _playerStatus.OnUpdateHud -= UpdateHud;
    }
}
