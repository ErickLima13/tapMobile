using System;
using UnityEngine;

public class BuilderPowerUp : IBuilderPowerUp
{
    private ButtonChoose _buttonChoose;
    private Action ActionButton;

    public BuilderPowerUp(ButtonChoose buttonChoose)
    {
        _buttonChoose = buttonChoose;
    }

    public ButtonChoose Builder()
    {
        ButtonChoose temp = GameObject.Instantiate(_buttonChoose);
        temp._onClick = ActionButton;
        return temp;
    }

    public IBuilderPowerUp SetAttackCount(int count)
    {
        _buttonChoose.attributes.AttackCount = count;
        return this;
    }

    public IBuilderPowerUp SetAttackTime(float time)
    {
        _buttonChoose.attributes.TimeToAttack = time;
        return this;
    }

    public IBuilderPowerUp SetAttackSpeed(float speed)
    {
        _buttonChoose.attributes.AttackSpeed = speed;
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
        ActionButton = action;
        return this;
    }
}
