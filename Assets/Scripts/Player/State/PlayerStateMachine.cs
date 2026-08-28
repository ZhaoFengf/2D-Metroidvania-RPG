using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private readonly Dictionary<PlayerStateId, PlayerState> states = new();
    public PlayerState CurrentState { get; private set; }

    public void RegisterState(PlayerState state)
    {
        if (state == null)
            return;

        states[state.Id] = state;
    }

    public PlayerState GetState(PlayerStateId id)
    {
        states.TryGetValue(id, out PlayerState state);
        return state;
    }

    public void Initialize(PlayerStateId startingState)
    {
        ChangeState(startingState);
    }

    public void ChangeState(PlayerStateId newState)
    {
        if (!states.TryGetValue(newState, out PlayerState nextState))
            return;

        if (CurrentState == nextState)
            return;

        CurrentState?.Exit();

        CurrentState = nextState;

        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
//    public PlayerState CurrentState { get; private set; } //公有获取，私有设置

//    public void Initialize(PlayerState _startingState)
//    {
//        CurrentState = _startingState;
//        CurrentState.Enter();
//    }

//    public void ChangeState(PlayerState _newState)
//    {
//        if (_newState == null)
//            return;

//        CurrentState?.Exit();
//        CurrentState = _newState;
//        CurrentState.Enter();
//    }

//    public void Update()
//    {
//        CurrentState?.Update();
//    }
//}
