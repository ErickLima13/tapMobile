using System;
using UnityEngine;

public class TestBuilder : MonoBehaviour
{
    public ButtonChoose game;

    public Transform containerButtons;

    public Sprite testIcon;

    public void CreateChooseButton(WeaponAttributes attributes, Action result)
    {
        var builder = new BuilderPowerUp(game)
            .SetAttackCount(attributes.WeaponCount)
            .SetAttackDamage(attributes.WeaponDamage)
            .SetWeaponLiberates(attributes.WeaponLiberates)
            .SetAttackTittle("Subiu de nivel")
            .SetAttackDescription($"Escolha o \n Attack Count :{attributes.WeaponCount}," +
            $"\n Attack Damage :{attributes.WeaponDamage.ToString("F2")}," +
            $"\n Release Weapon :{attributes.WeaponLiberates.ToString()}")
            .SetAttackImage(testIcon)
            .SetClickAction(result);

        var bc = builder.Builder();

        bc.transform.SetParent(containerButtons);
        bc.transform.localScale = Vector3.one;
        containerButtons.gameObject.SetActive(true);
    }

    public void ClearOptions()
    {
        containerButtons.gameObject.SetActive(false);

        for (int i = containerButtons.childCount - 1; i >= 0; i--)
        {
            Destroy(containerButtons.GetChild(i).gameObject);
        }
    }
}
