using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private Enemy_DeathBringer enemy;

    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(enemy.transform.position, PlayerManager.instance.player.transform.position) < 7f)
        {
            enemy.bossFightBegin = true;
            //stateMachine.ChangeState(enemy.battleState);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            stateMachine.ChangeState(enemy.teleportState);
        }

        if(stateTimer <= 0 && enemy.bossFightBegin)
        {
            stateMachine.ChangeState(enemy.battleState);
        }

    }
}