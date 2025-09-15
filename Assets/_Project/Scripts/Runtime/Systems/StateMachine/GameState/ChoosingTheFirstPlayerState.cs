using Random = UnityEngine.Random;

public class ChoosingTheFirstPlayerState : StateMachineBase
{


    public bool IsAIFirstPlayer
    {
        get; private set;
    }

    public override void Do()
    {
        base.Do();
    }

    public override void Enter()
    {
        base.Enter();
        ChooseFirstPlayer();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void ChooseFirstPlayer()
    {
        int rand = Random.Range(0, 100);

        if (rand % 2 == 0)
        {

            IsAIFirstPlayer = false;
        }
        else
        {

            IsAIFirstPlayer = true;
        }

        Manager.ChangeState(Manager.GetState<InGameState>());
    }
}
