using UnityEngine;

public class TestBuilder : MonoBehaviour
{
    public ButtonChoose game;

    public Canvas canvas;

    public Sprite testIcon;

    private void Start()
    {
        var builder = new BuilderPowerUp(game).SetAttackCount(1)
            .SetAttackSpeed(3)
            .SetAttackTime(5)
            .SetClickAction(TestAQui)
            .SetAttackTittle("teste titulo")
            .SetAttackDescription("teste descrição")
            .SetAttackImage(testIcon);


        var bc = builder.Builder();


        bc.transform.SetParent(canvas.transform);
        bc.transform.localScale = Vector3.one;
    }


    private void TestAQui()
    {
        print("aqui");
    }
}
