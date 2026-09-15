using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private readonly Dictionary<PlayerStateId, PlayerState> states = new();

    public PlayerState CurrentState { get; private set; }

    private bool isInitialized;

    public void RegisterState(PlayerState state)
    {
        if (state == null)
        {
            Debug.LogError("PlayerStateMachine: Cannot register a null state.");
            return;
        }

        if (states.ContainsKey(state.Id))
        {
            Debug.LogError($"PlayerStateMachine: State '{state.Id}' is already registered.");
            return;
        }

        states.Add(state.Id, state);
    }

    public PlayerState GetState(PlayerStateId id)
    {
        states.TryGetValue(id, out PlayerState state);
        return state;
    }

    public bool Initialize(PlayerStateId startingState)
    {
        if (isInitialized)
        {
            Debug.LogWarning($"PlayerStateMachine: Initialize was called more than once. " + $"Current state is '{CurrentState?.Id}'.");
            return false;
        }

        isInitialized = true;

        return ChangeState(startingState);
    }

    public bool ChangeState(PlayerStateId newState)
    {
        if (!states.TryGetValue(newState, out PlayerState nextState))
        {
            Debug.LogError($"PlayerStateMachine: Cannot change to state '{newState}'. " + $"The state has not been registered.");

            return false;
        }

        if (CurrentState == nextState)return false;

        CurrentState?.Exit();

        CurrentState = nextState;
        CurrentState.Enter();

        return true;
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}


/* ====== second version ======
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
*/

// ========== first version =============
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
