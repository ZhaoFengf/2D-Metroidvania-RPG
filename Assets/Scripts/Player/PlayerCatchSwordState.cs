using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.Sword.transform;

        player.fx.PlayDustFX();
        player.fx.ScreenShake(player.fx.shakeSwordImpact);

        //if (player.transform.position.x > sword.position.x && player.facingDirection == 1)
        //    player.Flip();
        //else if (player.transform.position.x < sword.position.x && player.facingDirection == -1)
        //    player.Flip();
        player.Movement.FaceTargetX(sword.position.x);

        //player.rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDirection, player.rb.velocity.y);
        player.Movement.SetHorizontalVelocity(player.swordReturnImpact * -player.Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine(player.BusyFor(.2f));
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            ChangeState(PlayerStateId.Idle);
    }
}
