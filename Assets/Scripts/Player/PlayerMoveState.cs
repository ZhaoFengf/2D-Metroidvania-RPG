using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
            stateMachine.ChangeState(player.idleState);
            return;
        }

        player.Movement.Move(player.PlayerInput.XInput);
    }
}
