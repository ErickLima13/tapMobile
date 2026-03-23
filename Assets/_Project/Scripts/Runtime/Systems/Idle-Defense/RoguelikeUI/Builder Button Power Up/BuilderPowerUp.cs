using System;
using UnityEngine;

public class BuilderPowerUp : IBuilderPowerUp
{
    private PlayerAttributes _playerAttributes;

    private ButtonChoose _buttonChoose;


    public BuilderPowerUp(ButtonChoose buttonChoose)
    {
        _buttonChoose = buttonChoose;
    }

    public ButtonChoose Builder()
    {
        ButtonChoose temp = GameObject.Instantiate(_buttonChoose);

        temp.attributes = _playerAttributes;

        return temp;
    }

    public IBuilderPowerUp SetAttackCount(int count)
    {
        _playerAttributes.AttackCount = count;
        return this;
    }

    public IBuilderPowerUp SetAttackTime(float time)
    {
        _playerAttributes.TimeToAttack = time;
        return this;
    }

    public IBuilderPowerUp SetAttackSpeed(float speed)
    {
        _playerAttributes.AttackSpeed = speed;
        return this;
    }

    public IBuilderPowerUp SetAttackDescription(string description)
    {
        _buttonChoose._description.text = description;
        return this;
    }

    public IBuilderPowerUp SetAttackImage(Sprite icon)
    {
        _buttonChoose._icon.sprite = icon;
        return this;
    }

    public IBuilderPowerUp SetAttackTittle(string tittle)
    {
        _buttonChoose._tittle.text = tittle;
        return this;
    }

    public IBuilderPowerUp SetClickAction(Action action)
    {
        _buttonChoose._onClick = action;
        return this;
    }
}
