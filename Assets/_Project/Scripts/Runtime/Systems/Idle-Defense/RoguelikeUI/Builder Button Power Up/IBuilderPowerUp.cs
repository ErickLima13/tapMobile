using System;
using UnityEngine;

public interface IBuilderPowerUp
{
    public IBuilderPowerUp SetAttackCount(int count);
    public IBuilderPowerUp SetAttackDamage(int damage);
    public IBuilderPowerUp SetWeaponLiberates(bool value);
    public IBuilderPowerUp SetAttackTittle(string tittle);
    public IBuilderPowerUp SetAttackDescription(string description);
    public IBuilderPowerUp SetAttackImage(Sprite icon);
    public IBuilderPowerUp SetClickAction(Action action);
    public ButtonChoose Builder();

    // public IBuilderPowerUp SetAttackTime(float time);
    //public IBuilderPowerUp SetAttackSpeed(float speed);

}
