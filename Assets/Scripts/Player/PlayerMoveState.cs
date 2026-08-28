using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.PlayerInput.XInput == 0 || player.isWallDetected())
        {
            stateMachine.ChangeState(PlayerStateId.Idle);
            return;
        }

        player.Movement.Move(player.PlayerInput.XInput);
    }
}
