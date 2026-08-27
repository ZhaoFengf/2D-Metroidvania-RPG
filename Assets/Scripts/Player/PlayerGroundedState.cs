using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (!player.isGroundedDeteced())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        if (player.PlayerInput.JumpPressed)
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (player.PlayerInput.BlackHolePressed && player.skill.blackHole.blackHoleUnlocked)
        {
            if (player.skill.blackHole.coolDownTimer > 0)
            {
                player.fx.CreatePopupText("cooldown");
                return;
            }
                
            stateMachine.ChangeState(player.blackHoleState);
            return;
        }

        if (player.PlayerInput.CounterPressed && player.skill.parry.parryUnlocked)
        {
            stateMachine.ChangeState(player.counterAttackState);
            return;
        }

        if (player.PlayerInput.AttackPressed)
        {
            stateMachine.ChangeState(player.primaryAttackState);
            return;
        }
        /* 20260826 对其进行重构，先进行注释，后续恢复并转移到其它的地方

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked) 
            stateMachine.ChangeState(player.aimSwordState);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        */
    }

    private bool HasNoSword()
    {
        if(!player.sword)
            return true;
        
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
