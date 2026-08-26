using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (player.isWallDetected() == false)
            stateMachine.ChangeState(player.airState);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
            

        if (player.PlayerInput.XInput != 0 && player.facingDirection != player.PlayerInput.XInput)
            stateMachine.ChangeState(player.idleState);

        if(player.PlayerInput.YInput < 0)
            player.rb.velocity = new Vector2(0, player.rb.velocity.y * 1.5f);
        else
            player.rb.velocity = new Vector2(0, player.rb.velocity.y * 0.7f);

        if(player.isGroundedDeteced())
            stateMachine.ChangeState(player.idleState);
    }

}
