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

    private void ApplyWeaponEffect(EnemyStats target)
    {
        ItemData_Equipment weaponData =
            Inventory.instance.GetEquipment(EquipmentType.Weapon);

        if (weaponData != null)
            weaponData.Effect(target.transform);
    }

    public void ThrowSword()
    {
        player.skill.sword.CreateSword();
    }
}