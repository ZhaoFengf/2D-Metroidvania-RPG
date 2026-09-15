using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //player.skill.dash.CloneOnDashStart();
        //player.StartDash();
        player.Ability.StartDash();

        stateTimer = player.DashDuration;

        //player.stat.MakeInvencible(true);
        player.SetInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();

        //player.skill.dash.CloneOnDashArrival();
        //player.EndDash();
        player.Ability.EndDash();

        player.Movement.Stop();

        //player.stat.MakeInvencible(false);
        player.SetInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (!player.isGroundedDeteced() && player.isWallDetected())
        {
            ChangeState(PlayerStateId.WallSlide);
            return;
        }


        if (stateTimer <= 0f)
        {
            ChangeState(PlayerStateId.Idle);
            return;
        }

        player.Movement.Dash(player.DashDirection);

        //player.fx.CreateAfterImage();
        player.CreateDashAfterImage();
    }
}
