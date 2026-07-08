using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("ActiveRedBlink", 0, .1f);

        stateTimer = enemy.stunDuration;
        //enemy.SetVelocity(-enemy.facingDirection * enemy.stunDircetion.x, enemy.stunDircetion.y); //会导致敌人被击退时，翻转
        rb.velocity = new Vector2(-enemy.facingDirection * enemy.stunDircetion.x, enemy.stunDircetion.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer <= 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
