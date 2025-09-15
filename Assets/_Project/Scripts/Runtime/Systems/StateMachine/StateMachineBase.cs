using System;
using UnityEngine;
using Zenject;

public class StateMachineBase : MonoBehaviour
{
    [Inject] protected StateMachineManager Manager;

    public event Action OnEnterEvent;
    public event Action OnDoEvent;
    public event Action OnExitEvent;

    public virtual void Enter()
    {
        print(this.GetType().Name);
        OnEnterEvent?.Invoke();
    }

    public virtual void Do()
    {
        OnDoEvent?.Invoke();
    }

    public virtual void Exit()
    {
        OnExitEvent?.Invoke();
    }
}
