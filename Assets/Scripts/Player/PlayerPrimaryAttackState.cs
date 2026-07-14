using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter { get; private set; }
    private float lastAttackTime = 0f;
    private float comboWindow = 1f; 
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0; //用于在攻击状态中锁定xInput，防止在攻击过程中改变方向
        if (Time.time - lastAttackTime > comboWindow || comboCounter > 2)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDirection = player.facingDirection;
        if(xInput != 0)
            attackDirection = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();


        player.StartCoroutine("BusyFor", .12f);
        comboCounter++;
        lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
