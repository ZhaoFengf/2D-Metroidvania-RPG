using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    protected Enemy_DeathBringer enemy;

    private int amountOfSpells;
    //private float spellCooldown;
    private float spellTimer;

    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        amountOfSpells = enemy.amountOfSpells;
        spellTimer =  .5f; //spellCooldown +
    }

    public override void Update()
    {
        base.Update();

        spellTimer -= Time.deltaTime;

        if (canCast())
        {
            enemy.CastSpell();
        }
        if(amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.teleportState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeCast = Time.time;
    }

    private bool canCast()
    {
        if(amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCooldown;
            return true;
        }
        return false;
    }
}