using System.Collections;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int MoveDirection;

    private bool flippedOnce;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //player = GameObject.Find("Player").transform;
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);

        stateTimer = enemy.battleTime;
        flippedOnce = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        enemy.anim.SetFloat("xVelocity", enemy.rb.velocity.x);

        if (enemy.IsPlayDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayDetected().distance < enemy.attackDistance)
            {
                if(canAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if(flippedOnce == false)
            {
                flippedOnce = true;
                enemy.Flip();
            }
                

            if (stateTimer <= 0 || Vector2.Distance(player.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }
        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);

        if (distanceToPlayerX < .8f)
            return;

        if (player.position.x > enemy.transform.position.x)
            MoveDirection = 1;
        else
            MoveDirection = -1;

        enemy.SetVelocity(enemy.moveSpeed * MoveDirection, rb.velocity.y);
    }

    private bool canAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCoolDown)
        {
            enemy.attackCoolDown = Random.Range(enemy.minAttackCoolDown, enemy.maxAttackCoolDown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        else
            return false;
    } 
}
