using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    { }
    public override void Update()
    {
        base.Update();

        if (player.isWallDetected() == false)
        {
            ChangeState(PlayerStateId.Air);
            return;
        }


        if (player.Intent.WantsJump)
        {
            ChangeState(PlayerStateId.WallJump);
            return;
        }
            

        if (player.Intent.MoveX != 0 && player.Movement.FacingDirection != Mathf.Sign(player.Intent.MoveX))
        {
            ChangeState(PlayerStateId.Idle);
            return;
        }

        //if(player.PlayerInput.YInput < 0)
        //    player.rb.velocity = new Vector2(0, player.rb.velocity.y * 1.5f);
        //else
        //    player.rb.velocity = new Vector2(0, player.rb.velocity.y * 0.7f);
        if (player.Intent.MoveY < 0)
            player.Movement.WallSlide(1.5f);
        else
            player.Movement.WallSlide(0.7f);

        if (player.isGroundedDeteced())
        {
            ChangeState(PlayerStateId.Idle);
            return;
        }
    }

}
