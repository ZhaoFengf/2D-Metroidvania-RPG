public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName) : base(_player, _stateMachine, _id, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Movement.Jump();
    }

    public override void Update()
    {
        base.Update();

        if(player.Movement.VerticalVelocity < 0f)
        {
            ChangeState(PlayerStateId.Air);
            return;
        }
    }
}
