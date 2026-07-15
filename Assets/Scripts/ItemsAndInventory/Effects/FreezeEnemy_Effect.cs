using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//当角色的体力小于等于10%的时候，将敌人进行冻结判定
[CreateAssetMenu(fileName = "Freeze enemies Effect", menuName = "Data/Item Effect/Freeze Effect")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transfoem)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
            return;


        if (Inventory.instance.canUseArmor())
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_transfoem.position, 2);
            foreach (var hit in colliders)
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.FreezeTimeFor(duration);
                }
            }
        }

    }
}
