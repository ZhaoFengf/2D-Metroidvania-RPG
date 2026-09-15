public class PlayerStateTransitionPolicy
{
    public bool CanTransition(PlayerStateId from, PlayerStateId to)
    {
        // Dead 是终态，进入 Dead 后不允许离开
        if (from == PlayerStateId.Dead) return false;

        // 死亡可以从任何非 Dead 状态进入
        if (to == PlayerStateId.Dead) return true;

        // 接剑是外部事件触发，可以从当前非 Dead 状态进入
        if (to == PlayerStateId.CatchSword) return true;

        // 当前 Dash 的请求来自 AbilityController，为保持原有行为，暂不限制来源状态。
        if (to == PlayerStateId.Dash) return true;

        switch (from)
        {
            case PlayerStateId.Idle: return IsIdleTransition(to);

            case PlayerStateId.Move: return IsMoveTransition(to);

            case PlayerStateId.Jump: return to == PlayerStateId.Air;

            case PlayerStateId.Air: return to == PlayerStateId.WallSlide || to == PlayerStateId.Idle;

            case PlayerStateId.Dash: return to == PlayerStateId.WallSlide || to == PlayerStateId.Idle;

            case PlayerStateId.WallSlide: return to == PlayerStateId.Air || to == PlayerStateId.WallJump || to == PlayerStateId.Idle;

            case PlayerStateId.WallJump: return to == PlayerStateId.Air || to == PlayerStateId.Idle;

            case PlayerStateId.PrimaryAttack: return to == PlayerStateId.Idle;

            case PlayerStateId.CounterAttack: return to == PlayerStateId.Idle;

            case PlayerStateId.AimSword: return to == PlayerStateId.Idle;

            case PlayerStateId.BlackHole: return to == PlayerStateId.Air;

            case PlayerStateId.CatchSword: return to == PlayerStateId.Idle;

            default: return false;
        }
    }

    private bool IsIdleTransition(PlayerStateId to)
    {
        return to == PlayerStateId.Move ||
               to == PlayerStateId.Jump ||
               to == PlayerStateId.BlackHole ||
               to == PlayerStateId.CounterAttack ||
               to == PlayerStateId.PrimaryAttack ||
               to == PlayerStateId.AimSword;
    }

    private bool IsMoveTransition(PlayerStateId to)
    {
        return to == PlayerStateId.Idle ||
               to == PlayerStateId.Jump ||
               to == PlayerStateId.BlackHole ||
               to == PlayerStateId.CounterAttack ||
               to == PlayerStateId.PrimaryAttack ||
               to == PlayerStateId.AimSword;
    }
}