using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (!player.isGroundedDeteced())
        {
            ChangeState(PlayerStateId.Air);
            return;
        }

        if (player.Intent.WantsJump)
        {
            ChangeState(PlayerStateId.Jump);
            return;
        }

        TryHandleGroundedActions();
    }

    protected bool TryHandleGroundedActions()
    {
        if (player.Intent.WantsBlackHole)
        {
            AbilityAvailability availability =
                player.Ability.GetBlackHoleAvailability();

            if (availability == AbilityAvailability.Available)
            {
                ChangeState(PlayerStateId.BlackHole);
                return true;
            }

            if (availability == AbilityAvailability.Cooldown)
            {
                player.fx.CreatePopupText("cooldown");
                return true;
            }
        }

        if (player.Intent.WantsCounter &&
            player.Ability.CanUseParry())
        {
            ChangeState(PlayerStateId.CounterAttack);
            return true;
        }

        if (player.Intent.WantsAttack)
        {
            ChangeState(PlayerStateId.PrimaryAttack);
            return true;
        }

        if (player.Intent.WantsAim && player.Ability.TryPrepareSwordAim())
        {
            ChangeState(PlayerStateId.AimSword);
            return true;
        }

        return false;
    }

}
