using UnityEngine;

public class TestBuilder : MonoBehaviour
{
    public ButtonChoose game;

    public Transform containerButtons;

    public Sprite testIcon;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            CreateChooseButton();
        } 
    }

    private void CreateChooseButton()
    {
        var builder = new BuilderPowerUp(game)
            .SetAttackCount(1)
            .SetAttackSpeed(3)
            .SetAttackTime(5)
            .SetAttackTittle("teste titulo")
            .SetAttackDescription("teste descrição")
            .SetAttackImage(testIcon)
            .SetClickAction(TestAQui);

        var bc = builder.Builder();

        bc.transform.SetParent(containerButtons);
        bc.transform.localScale = Vector3.one;
    }

    private void TestAQui()
    {
        print("aqui");
    }
}
