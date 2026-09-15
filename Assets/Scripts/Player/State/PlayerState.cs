using UnityEngine;

public abstract class PlayerState
{
    protected readonly PlayerStateMachine stateMachine;
    protected readonly Player player;

    private readonly string animBoolName;

    public PlayerStateId Id { get; }

    protected float stateTimer;
    protected bool triggerCalled;

    protected PlayerState(Player _player, PlayerStateMachine _stateMachine, PlayerStateId _id, string _animBoolName)
    {
        player = _player;
        stateMachine = _stateMachine;
        Id = _id;
        animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        player.Animation.PlayState(animBoolName);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        //player.Animation.SetYVelocity(player.rb.velocity.y);
        player.Animation.SetYVelocity(player.Movement.VerticalVelocity);
    }
    public virtual void Exit()
    {
        player.Animation.StopState(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    protected bool ChangeState(PlayerStateId stateId)
    {
        return player.RequestState(stateId);
    }
}
