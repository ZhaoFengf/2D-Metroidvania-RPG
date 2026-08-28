using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    #region Component
    public PlayerInput PlayerInput { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerAnimation Animation { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerAbilityController Ability { get; private set; }
    public PlayerStateFactory StateFactory { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    public Player_FX fx { get; private set; }
    #endregion

    [Header("Attack details")]
    public Vector2 [] attackMovement;
    public float counterAttackDuration = .2f;
    

    public bool isBusy { get; private set; }
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float swordReturnImpact = 8f;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    private float defaultDashSpeed;
    public float dashDirection { get;private set; }


    #region States
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        PlayerInput = GetComponent<PlayerInput>();

        Movement = new PlayerMovement(this);
        Animation = new PlayerAnimation(this);
        Combat = new PlayerCombat(this);
        Ability = new PlayerAbilityController(this);

        StateMachine = new PlayerStateMachine();
        StateFactory = new PlayerStateFactory(this, StateMachine);

        StateMachine.RegisterState(StateFactory.CreateIdle());
        StateMachine.RegisterState(StateFactory.CreateMove());
        StateMachine.RegisterState(StateFactory.CreateJump());
        StateMachine.RegisterState(StateFactory.CreateAir());
        StateMachine.RegisterState(StateFactory.CreateDash());

        StateMachine.RegisterState(StateFactory.CreateWallSlide());
        StateMachine.RegisterState(StateFactory.CreateWallJump());

        StateMachine.RegisterState(StateFactory.CreatePrimaryAttack());
        StateMachine.RegisterState(StateFactory.CreateCounterAttack());

        StateMachine.RegisterState(StateFactory.CreateAimSword());
        StateMachine.RegisterState(StateFactory.CreateCatchSword());

        StateMachine.RegisterState(StateFactory.CreateBlackHole());
        StateMachine.RegisterState(StateFactory.CreateDead());
    }

    protected override void Start()
    {
        base.Start();

        fx = GetComponent<Player_FX>();//原本这里对应的是fx，因为这里改了，所以其余地方也要改成playerFX

        skill = SkillManager.instance;

        StateMachine.Initialize(PlayerStateId.Idle);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

    }

    protected override void Update()
    {
        if(Time.timeScale == 0f)
            return;

        base.Update();
        StateMachine.Update();

        Ability.HandleDash();
        Ability.HandleCrystal();
        Ability.HandleFlask();

    }

    public void SetDashDirection(float direction)
    {
        if (direction == 0f)
            direction = facingDirection;

        dashDirection = direction;
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1f - _slowPercentage);
        jumpForce = jumpForce * (1f - _slowPercentage);
        dashSpeed = dashSpeed * (1f - _slowPercentage);
        anim.speed = anim.speed * (1f - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(PlayerStateId.CatchSword);
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();


    public override void Die()
    {
        base.Die();
        StateMachine.ChangeState(PlayerStateId.Dead);
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
