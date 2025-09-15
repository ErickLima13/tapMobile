using System;
using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    [SerializeField] private StateMachineBase[] _statesBase;

    [SerializeField] private StateMachineBase _currentState;

    public T GetState<T>() where T : StateMachineBase
    {
        T state = null;
        foreach (var st in _statesBase)
        {
            if (st is T)
            {
                state = st as T;
                break;
            }
        }

        return state;
    }

    public void ChangeState(StateMachineBase newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
