using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    private Transform player;
    private Enemy_DeathBringer enemy;
    private int MoveDirection;
    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //player = GameObject.Find("Player").transform;
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            Debug.Log("Player is dead");
        //stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        if (enemy.IsPlayDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayDetected().distance < enemy.attackDistance)
            {
                if (canAttack())
                    stateMachine.ChangeState(enemy.attackState);
                else
                    stateMachine.ChangeState(enemy.idleState);
            }
        }
        //else
        //{
        //    if (stateTimer <= 0 || Vector2.Distance(player.position, enemy.transform.position) > 10)
        //        stateMachine.ChangeState(enemy.idleState);
        //}

        if (player.position.x > enemy.transform.position.x)
            MoveDirection = 1;
        else
            MoveDirection = -1;

        if (enemy.IsPlayDetected() && enemy.IsPlayDetected().distance < enemy.attackDistance - .1f)
            return;

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