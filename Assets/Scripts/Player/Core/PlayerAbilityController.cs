using UnityEngine;

public class PlayerAbilityController
{
    private readonly Player player;

    public PlayerAbilityController(Player _player)
    {
        player = _player;
    }

    public void HandleInput()
    {
        HandleDash();
        HandleCrystal();
        HandleFlask();
    }

    public void HandleDash()
    {
        if (player.isWallDetected()) return;

        if (!player.skill.dash.dashUnlocked) return;

        if (!player.Intent.WantsDash) return;

        if (!player.skill.dash.TryUseSkill()) return;

        player.SetDashDirection(player.Intent.MoveX);
        player.RequestState(PlayerStateId.Dash);
    }

    public void HandleCrystal()
    {
        if (!player.Intent.WantsCrystal) return;

        if (!player.skill.crystal.crystalUnlocked) return;

        player.skill.crystal.TryUseSkill();
    }

    public void HandleFlask()
    {
        if (!player.Intent.WantsFlask) return;

        Inventory.instance.UseFlask();
    }

    public AbilityAvailability GetBlackHoleAvailability()
    {
        if (!player.skill.blackHole.blackHoleUnlocked)
            return AbilityAvailability.Locked;

        if (player.skill.blackHole.coolDownTimer > 0)
            return AbilityAvailability.Cooldown;

        return AbilityAvailability.Available;
    }

    public bool CanUseParry()
    {
        return player.skill.parry.parryUnlocked;
    }

    public bool CanUseSword()
    {
        return player.skill.sword.swordUnlocked;
    }

    public void StartDash()
    {
        player.skill.dash.CloneOnDashStart();
    }

    public void EndDash()
    {
        player.skill.dash.CloneOnDashArrival();
    }

    public void UseParry()
    {
        player.skill.parry.UseSkill();
    }

    public void CreateParryMirage(Transform target)
    {
        player.skill.parry.MakeMirageOnParry(target);
    }

    public bool TryUseBlackHole()
    {
        return player.skill.blackHole.TryUseSkill();
    }

    public bool IsBlackHoleCompleted()
    {
        return player.skill.blackHole.SkillCompleted();
    }

    public void SetSwordAimDotsActive(bool active)
    {
        player.skill.sword.DotsActive(active);
    }

    public void ThrowSword()
    {
        player.skill.sword.CreateSword();
    }

    public bool TryPrepareSwordAim()
    {
        if (!CanUseSword())
            return false;

        if (!player.Sword)
            return true;

        Sword_Skill_Controller sword =
            player.Sword.GetComponent<Sword_Skill_Controller>();

        if (sword == null)
            return false;

        sword.ReturnSword();
        return false;
    }
}