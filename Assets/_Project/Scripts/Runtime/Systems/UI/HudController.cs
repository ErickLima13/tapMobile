using TMPro;
using UnityEngine;
using Zenject;

public class HudController : MonoBehaviour
{
    [Inject] private EnemyCollider[] _enemiesColliders;
    [Inject] private PlayerStatus _playerStatus;

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _livesText;

    private void Start()
    {
        _playerStatus.OnUpdateHud += UpdateHud;
    }

    public void UpdateHud(bool value,int total)
    {
        if (value)
        {
            _scoreText.text = total.ToString();
        }
        else
        {
            // vidas
        }
    }

    private void OnDisable()
    {
        _playerStatus.OnUpdateHud -= UpdateHud;
    }
}
