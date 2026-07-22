using System.Collections;
using UnityEngine;

public class ShadyMoveState : ShadyGroundedState
{
    public ShadyMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, rb.velocity.y);

        if (enemy.isWallDetected() || !enemy.isGroundedDeteced())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }

    }
}
