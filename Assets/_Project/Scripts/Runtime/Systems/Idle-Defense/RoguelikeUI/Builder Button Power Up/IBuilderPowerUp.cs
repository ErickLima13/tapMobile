using System;
using UnityEngine;

public interface IBuilderPowerUp
{
    public IBuilderPowerUp SetAttackSpeed(float speed);
    public IBuilderPowerUp SetAttackCount(int count);
    public IBuilderPowerUp SetAttackTime(float time);
    public IBuilderPowerUp SetAttackTittle(string tittle);
    public IBuilderPowerUp SetAttackDescription(string description);
    public IBuilderPowerUp SetAttackImage(Sprite icon);
    public IBuilderPowerUp SetClickAction(Action action);
    public ButtonChoose Builder();
}
