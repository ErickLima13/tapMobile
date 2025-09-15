using UnityEngine;

public class InitializingState : StateMachineBase
{

    private void Start()
    {
        Manager.ChangeState(this);
    }

    public override void Do()
    {    
        base.Do();
    }

    public override void Enter()
    {
        base.Enter();

        Manager.ChangeState(Manager.GetState<ChoosingTheFirstPlayerState>());
    }

    public override void Exit()
    {
        base.Exit();
    }
}
