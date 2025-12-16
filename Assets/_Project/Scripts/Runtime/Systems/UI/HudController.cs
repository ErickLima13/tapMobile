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
    [SerializeField] private GameObject _gameOverAnimation;

    public GameObject _pausePanel;

    public GameObject[] _buttonsPausePanel;

    public Image _pauseButton;

    public bool isPause;

    public bool isGameOver;

    public int count = 2;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        _pausePanel.SetActive(false);

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
            if(count >= 0)
            {
                _livesImg[count].enabled = false;
                count--;
            }

            print(count);

            if(total == 0)
            {
                GameOver();
            }
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

    private void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;
        Vector2 posGameOver = new(0, 2);

        GameObject temp =  Instantiate(_gameOverAnimation);
        temp.transform.position = posGameOver;

        _pauseButton.gameObject.SetActive(false);
        _buttonsPausePanel[0].SetActive(true);
        _buttonsPausePanel[1].SetActive(false);
        _pausePanel.SetActive(true);

    }
}
