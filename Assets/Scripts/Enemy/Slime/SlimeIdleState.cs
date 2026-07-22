using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        //AudioManager.instance.PlaySFX(0, enemy.transform); //目前仅有一个音效，主要用于学习方法
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
            stateMachine.ChangeState(enemy.moveState);

    }
}