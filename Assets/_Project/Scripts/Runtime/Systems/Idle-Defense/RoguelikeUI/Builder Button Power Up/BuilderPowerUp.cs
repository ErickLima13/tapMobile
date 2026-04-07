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
        _buttonChoose.attributes.WeaponCount = count;
        return this;
    }

    public IBuilderPowerUp SetAttackDamage(int damage)
    {
        _buttonChoose.attributes.WeaponDamage = damage;
        return this;
    }

    public IBuilderPowerUp SetWeaponLiberates(bool value)
    {
        _buttonChoose.attributes.WeaponLiberates = value;
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
