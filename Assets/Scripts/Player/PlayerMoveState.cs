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

        //AudioManager.instance.PlaySFX(0); //这里后续如果真想要做可以添加这一句，使用对应的音频；可以分析尝试loop以及在update中使用
    }

    public override void Exit()
    {
        base.Exit();

        //AudioManager.instance.StopSFX(0); //这里后续如果真想要做可以添加这一句，停止对应的音频
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (xInput == 0 || player.isWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
