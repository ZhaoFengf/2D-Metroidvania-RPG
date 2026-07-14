using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//区别于另一个shocktrike controller，这个是用于装备的雷霆特效
public class ThunderStrikeController : MonoBehaviour
{

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.DoMagicDamage(enemyTarget);
        }
    }

}
