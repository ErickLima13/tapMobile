using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChoose : MonoBehaviour
{
    public TextMeshProUGUI _tittle;
    public TextMeshProUGUI _description;

    public Image _icon;
    public Button _buttonAction;

    public PlayerAttributes attributes;

    public Action _onClick;

    private void Start()
    {
        _buttonAction.onClick.AddListener(OnclickAction);

        print("aqui start");
    }

    public void OnclickAction()
    {
        _onClick?.Invoke();
    }
}
