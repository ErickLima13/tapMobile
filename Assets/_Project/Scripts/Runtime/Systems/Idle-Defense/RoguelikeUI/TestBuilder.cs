using System;
using UnityEngine;

public class TestBuilder : MonoBehaviour
{
    public ButtonChoose game;

    public Transform containerButtons;

    public Sprite testIcon;

    public void CreateChooseButton(PlayerAttributes attributes, Action result)
    {
        var builder = new BuilderPowerUp(game)
            .SetAttackCount(attributes.AttackCount)
            .SetAttackSpeed(attributes.AttackSpeed)
            .SetAttackTime(attributes.AttackTime)
            .SetAttackTittle("Subiu de nivel")
            .SetAttackDescription($"Escolha o \n Attack Count :{attributes.AttackCount}," +
            $"\n Attack Speed :{attributes.AttackSpeed.ToString("F2")}," +
            $"\n Attack Time:{attributes.AttackTime.ToString("F2")}")
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
