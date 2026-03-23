using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuilderPowerUp : MonoBehaviour, IBuilderPowerUp
{
    private PlayerAttributes _playerAttributes;

    private ButtonChoose _buttonChoose;


    public ButtonChoose Builder()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public IBuilderPowerUp SetAttackImage(Sprite icon)
    {
        throw new NotImplementedException();
    }

    public IBuilderPowerUp SetAttackTittle(string tittle)
    {
        throw new NotImplementedException();
    }

    public IBuilderPowerUp SetClickAction(Action action)
    {
        throw new NotImplementedException();
    }
}
