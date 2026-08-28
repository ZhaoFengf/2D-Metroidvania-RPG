public class PlayerStateFactory
{
    private readonly Player player;
    private readonly PlayerStateMachine stateMachine;

    public PlayerStateFactory(Player _player, PlayerStateMachine _stateMachine)
    {
        player = _player;
        stateMachine = _stateMachine;
    }

    public PlayerIdleState CreateIdle()
    {
        return new PlayerIdleState(player, stateMachine, PlayerStateId.Idle, "Idle");
    }

    public PlayerMoveState CreateMove()
    {
        return new PlayerMoveState(player, stateMachine, PlayerStateId.Move, "Move");
    }

    public PlayerJumpState CreateJump()
    {
        return new PlayerJumpState(player, stateMachine, PlayerStateId.Jump, "Jump");
    }

    public PlayerAirState CreateAir()
    {
        return new PlayerAirState(player, stateMachine, PlayerStateId.Air, "Jump");
    }

    public PlayerDashState CreateDash()
    {
        return new PlayerDashState(player, stateMachine, PlayerStateId.Dash, "Dash");
    }

    public PlayerWallSlideState CreateWallSlide()
    {
        return new PlayerWallSlideState(player,stateMachine, PlayerStateId.WallSlide, "WallSlide");
    }

    public PlayerWallJumpState CreateWallJump()
    {
        return new PlayerWallJumpState(player, stateMachine, PlayerStateId.WallJump, "Jump");
    }

    public PlayerPrimaryAttackState CreatePrimaryAttack()
    {
        return new PlayerPrimaryAttackState(player, stateMachine, PlayerStateId.PrimaryAttack, "Attack");
    }

    public PlayerCounterAttackState CreateCounterAttack()
    {
        return new PlayerCounterAttackState(player, stateMachine, PlayerStateId.CounterAttack, "CounterAttack");
    }

    public PlayerAimSwordState CreateAimSword()
    {
        return new PlayerAimSwordState(player, stateMachine, PlayerStateId.AimSword, "AimSword");
    }

    public PlayerCatchSwordState CreateCatchSword()
    {
        return new PlayerCatchSwordState(player, stateMachine, PlayerStateId.CatchSword, "CatchSword");
    }

    public PlayerBlackHoleState CreateBlackHole()
    {
        return new PlayerBlackHoleState(player, stateMachine, PlayerStateId.BlackHole, "Jump");
    }

    public PlayerDeadState CreateDead()
    {
        return new PlayerDeadState(player, stateMachine, PlayerStateId.Dead, "Die");
    }
}