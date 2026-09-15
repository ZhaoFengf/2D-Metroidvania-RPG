using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.Animation.SetCounterSuccess(false);
    }

    public override void Update()
    {
        base.Update();

        player.Movement.Stop();

        player.Combat.PerformCounterAttack();

        if (stateTimer < 0 || triggerCalled)
            ChangeState(PlayerStateId.Idle);
    }

    private void SuccessfulCounterAttack()
    {
        stateTimer = 10;
        player.Animation.SetCounterSuccess(true);
    }
}

/*
 public override void Update()
    {
        base.Update();

        player.Movement.Stop();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                SuccessfulCounterAttack();
            }

            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    SuccessfulCounterAttack();

                    //player.skill.parry.UseSkill();
                    player.UseParrySkill();

                    if (canCreateClone)
                    {
                        canCreateClone = false;

                        //player.skill.parry.MakeMirageOnParry(hit.transform);
                        player.CreateParryMirage(hit.transform);

                    }
                }
            }
        }
        if(stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(PlayerStateId.Idle);
    }
 */
