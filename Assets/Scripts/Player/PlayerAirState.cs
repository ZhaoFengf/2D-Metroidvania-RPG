using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (player.isWallDetected())
        {
            ChangeState(PlayerStateId.WallSlide);
            return;
        }

        if (player.isGroundedDeteced())
        {
            ChangeState(PlayerStateId.Idle);
            return;
        }

        if (player.Intent.MoveX != 0)
            player.Movement.AirMove(player.Intent.MoveX);
    }
}
