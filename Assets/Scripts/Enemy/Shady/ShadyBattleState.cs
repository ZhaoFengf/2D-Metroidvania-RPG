using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShadyBattleState : EnemyState
{
    private Transform player;
    private Enemy_Shady enemy;
    private int MoveDirection;

    private float defaultSpeed;

    public ShadyBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        
        defaultSpeed = enemy.moveSpeed;

        enemy.moveSpeed = enemy.battleStateMoveSpeed;

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.moveSpeed = defaultSpeed;
    }

    public override void Update()
    {
        base.Update();


        if (enemy.IsPlayDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayDetected().distance < enemy.attackDistance)
            {
                //stateMachine.ChangeState(enemy.deadState);
                enemy.stat.KillEntity();
            }
        }
        else
        {
            if (stateTimer <= 0 || Vector2.Distance(player.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x)
            MoveDirection = 1;
        else
            MoveDirection = -1;

        enemy.SetVelocity(enemy.moveSpeed * MoveDirection, rb.velocity.y);
    }

    private bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCoolDown)
        {
            enemy.attackCoolDown = Random.Range(enemy.minAttackCoolDown, enemy.maxAttackCoolDown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        else
            return false;
    }
}
