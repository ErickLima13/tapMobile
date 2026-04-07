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

    public WeaponAttributes attributes;

    public Action _onClick;


    public void OnclickAction()
    {
        _onClick?.Invoke();
    }
}
