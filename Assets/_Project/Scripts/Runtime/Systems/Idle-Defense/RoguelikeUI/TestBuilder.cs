using System;
using UnityEngine;

public class TestBuilder : MonoBehaviour
{
    public ButtonChoose game;

    public Transform containerButtons;

    public Sprite testIcon;

    public void CreateUnlockWeaponButton(WeaponAttributes attributes, Action result)
    {
        var builder = new BuilderPowerUp(game)
            .SetWeaponLiberates(attributes.WeaponLiberates)
            .SetAttackTittle($"Libere : {attributes.WeaponName}")
            .SetAttackImage(testIcon)
            .SetClickAction(result);

        var bc = builder.Builder();

        SetButtonParent(bc);
    }

    public void CreateAttributesButton(WeaponAttributes attributes, Action result)
    {
        var builder = new BuilderPowerUp(game)
            .SetAttackCount(attributes.WeaponCount)
            .SetAttackDamage(attributes.WeaponDamage)
            .SetAttackTittle($"Melhore : {attributes.WeaponName}")
            .SetAttackDescription($"Attack Count :{attributes.WeaponCount}," +
            $"\n Attack Damage :{attributes.WeaponDamage.ToString("F2")}")
            .SetAttackImage(testIcon)
            .SetClickAction(result);

        var bc = builder.Builder();

        SetButtonParent(bc);
    }

    private void SetButtonParent(ButtonChoose bc)
    {
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
