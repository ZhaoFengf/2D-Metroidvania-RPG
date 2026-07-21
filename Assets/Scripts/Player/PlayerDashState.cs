using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //player.skill.clone.CreateClone(player.transform, Vector2.zero);
        player.skill.dash.CloneOnDashStart();

        stateTimer = player.dashDuration;

        player.stat.MakeInvencible(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.skill.dash.CloneOnDashArrival();

        player.SetVelocity(0f, rb.velocity.y);

        player.stat.MakeInvencible(false);
    }

    public override void Update()
    {
        base.Update();

        if (!player.isGroundedDeteced() && player.isWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);

        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
