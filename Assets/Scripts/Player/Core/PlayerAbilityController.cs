using UnityEngine;

public class PlayerAbilityController
{
    private readonly Player player;

    public PlayerAbilityController(Player _player)
    {
        player = _player;
    }

    public void HandleDash()
    {
        if (player.isWallDetected())
            return;

        if (!player.skill.dash.dashUnlocked)
            return;

        if (!player.PlayerInput.DashPressed)
            return;

        if (!player.skill.dash.CanUseSkill())
            return;

        player.SetDashDirection(player.PlayerInput.XInput);

        player.StateMachine.ChangeState(PlayerStateId.Dash);
    }

    public void HandleCrystal()
    {
        if (!player.PlayerInput.CrystalPressed)
            return;

        if (!player.skill.crystal.crystalUnlocked)
            return;

        player.skill.crystal.CanUseSkill();
    }

    public void HandleFlask()
    {
        if (!player.PlayerInput.FlaskPressed)
            return;

        Inventory.instance.UseFlask();
    }
}