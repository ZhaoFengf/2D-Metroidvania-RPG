using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        //player.DamageEffect();
    }

    protected override void Die()
    { 
        base.Die();

        player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    public override void OnEvasion()
    {
        Debug.Log("attack avoiding");
        player.skill.dorge.CreateMirageOnDodge();

    }

    public void CloneDodamage(CharacterStats _targetStats, float _multiplier)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if(_multiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
        }

        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicDamage(_targetStats);//这个可能会导致双重判定，即物理和法术攻击均判定一次，假如其有俩属性
    }
}
