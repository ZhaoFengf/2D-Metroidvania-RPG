using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTriggers : MonoBehaviour
{
    private Enemy enemy=>GetComponentInParent<Enemy>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTriger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stat.DoDamage(target);
            }
        }
    }

    protected void OpenCounterWindow()=> enemy.OpenCounterAttackWindow();
    protected void CloseCounterWindow()=> enemy.CloseCounterAttackWindow();
}
