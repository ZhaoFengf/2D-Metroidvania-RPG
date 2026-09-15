using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.CurrentState != this) return;

        //if (player.PlayerInput.XInput == 0 || player.isWallDetected())
        if (player.Intent.MoveX == 0 || player.isWallDetected())
        {
            ChangeState(PlayerStateId.Idle);
            return;
        }

        //player.Movement.Move(player.PlayerInput.XInput);
        player.Movement.Move(player.Intent.MoveX);

    }
}
