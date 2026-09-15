using UnityEngine;

public class PlayerCombat
{
    private readonly Player player;

    public PlayerCombat(Player _player)
    {
        player = _player;
    }

    public void PerformAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy == null)
                continue;

            EnemyStats target = hit.GetComponent<EnemyStats>();

            if (target == null)
                continue;

            player.stat.DoDamage(target);

            ApplyWeaponEffect(target);
        }
    }

    public void PerformCounterAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //HandleCounterTarget(hit);
            if (HandleCounterTarget(hit))
            {
                return;
            }
        }
    }

    private bool HandleCounterTarget(Collider2D hit)
    {
        Arrow_Controller arrow = hit.GetComponent<Arrow_Controller>();

        if (arrow != null)
        {
            arrow.FlipArrow();
            return true;
        }

        Enemy enemy = hit.GetComponent<Enemy>();

        if (enemy == null)
            return false;

        if (!enemy.CanBeStunned())
            return false;

        player.Animation.SetCounterSuccess(true);
        //player.UseParrySkill();
        //player.CreateParryMirage(enemy.transform);
        player.Ability.UseParry();
        player.Ability.CreateParryMirage(enemy.transform);

        return true;
    }

    private void ApplyWeaponEffect(EnemyStats target)
    {
        ItemData_Equipment weaponData =
            Inventory.instance.GetEquipment(EquipmentType.Weapon);

        if (weaponData != null)
            weaponData.Effect(target.transform);
    }

    public void ThrowSword()
    {
        //player.skill.sword.CreateSword();
        player.Ability.ThrowSword();
    }
}