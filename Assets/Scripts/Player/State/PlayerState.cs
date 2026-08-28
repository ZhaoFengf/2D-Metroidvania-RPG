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
        player.Animation.PlayState(animBoolName);
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        player.Animation.SetYVelocity(player.rb.velocity.y);
    }
    public virtual void Exit()
    {
        player.Animation.StopState(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}

//public class PlayerState
//{
//    protected readonly PlayerStateMachine stateMachine;
//    protected readonly Player player;

//    private readonly string animBoolName;

//    protected float stateTimer;
//    protected bool triggerCalled;

//    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
//    {
//        this.player = _player;
//        this.stateMachine = _stateMachine;
//        this.animBoolName = _animBoolName;
//    }
