using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
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
        if (player.isWallDetected())
        {
            stateMachine.ChangeState(PlayerStateId.WallSlide);
            return;
        }

        if (player.isGroundedDeteced())
        {
            stateMachine.ChangeState(PlayerStateId.Idle);
            return;
        }

        if (player.PlayerInput.XInput != 0)
            player.Movement.AirMove(player.PlayerInput.XInput);
    }
}
