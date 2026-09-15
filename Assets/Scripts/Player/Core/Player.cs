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
    public PlayerStateTransitionPolicy TransitionPolicy { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject Sword { get; private set; }
    public Player_FX fx { get; private set; }
    #endregion

    [Header("Attack details")]
    public Vector2 [] attackMovement;
    public float counterAttackDuration = .2f;
    

    public bool IsBusy { get; private set; }
    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float baseJumpForce = 10f;

    public float MoveSpeed { get; private set; }
    public float JumpForce { get; private set; }

    public float swordReturnImpact = 8f;
    //public float moveSpeed = 5f;
    //public float jumpForce = 10f;
    //public float swordReturnImpact = 8f;
    //private float defaultMoveSpeed;
    //private float defaultJumpForce;

    [Header("Dash")]
    [SerializeField] private float baseDashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;

    public float DashSpeed { get; private set; }
    public float DashDuration => dashDuration;
    //public float dashSpeed = 15f;
    //public float dashDuration = 0.2f;
    //private float defaultDashSpeed;
    public float DashDirection { get;private set; }

    public PlayerInputSnapshot Input => PlayerInput.Current;
    public PlayerIntent Intent { get; private set; }

    private PlayerIntentResolver intentResolver;

    #region States
    public PlayerStateMachine StateMachine { get; private set; }
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
        TransitionPolicy = new PlayerStateTransitionPolicy();

        intentResolver = new PlayerIntentResolver(Camera.main);

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

        MoveSpeed = baseMoveSpeed;
        JumpForce = baseJumpForce;
        DashSpeed = baseDashSpeed;

    }

    protected override void Update()
    {
        if(Time.timeScale == 0f)
            return;

        PlayerInput.UpdateInput();

        Intent = intentResolver.Resolve(PlayerInput.Current);


        base.Update();

        HandleSystemInput(); //当前是用于检测是否退出游戏

        Ability.HandleInput();

        StateMachine.Update();
    }

    //public void RequestState(PlayerStateId stateId)
    //{
    //    StateMachine.ChangeState(stateId);
    //}
    //public bool RequestState(PlayerStateId stateId)
    //{
    //    PlayerState currentState = StateMachine.CurrentState;

    //    if (currentState == null) return false;

    //    if (!TransitionPolicy.CanTransition(currentState.Id, stateId))
    //    {
    //        return false;
    //    }

    //    StateMachine.ChangeState(stateId);
    //    return true;
    //}
    public bool RequestState(PlayerStateId stateId)
    {
        PlayerState currentState = StateMachine.CurrentState;

        if (currentState == null)
        {
            Debug.LogError($"Player: Cannot request state '{stateId}' because " + "the StateMachine has not been initialized.");
            return false;
        }

        if (!TransitionPolicy.CanTransition(currentState.Id, stateId))
        {
            Debug.LogWarning($"Player: Transition rejected. " + $"'{currentState.Id}' -> '{stateId}'.");
            return false;
        }

        return StateMachine.ChangeState(stateId);
    }

    private void HandleSystemInput()
    {
        if (PlayerInput.QuitGame)
        {
            Application.Quit();
        }
    }


 
    public void SetDashDirection(float direction)
    {
        if (direction == 0f)
            direction = facingDirection;

        DashDirection = direction;
    }


    public void SetInvincible(bool value)
    {
        stat.MakeInvencible(value);
    }

    public void CreateDashAfterImage()
    {
        fx.CreateAfterImage();
    }
   
    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        MoveSpeed *= 1f - _slowPercentage;
        JumpForce *= 1f - _slowPercentage;
        DashSpeed *= 1f - _slowPercentage;
        anim.speed = anim.speed * (1f - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        MoveSpeed = baseMoveSpeed;
        JumpForce = baseJumpForce;
        DashSpeed = baseDashSpeed;
    }

    public void AssignNewSword(GameObject _newSword)
    {
        Sword = _newSword;
    }

    public void CatchTheSword()
    {
        //RequestState(PlayerStateId.CatchSword);
        //Destroy(sword);
        GameObject caughtSword = Sword;

        RequestState(PlayerStateId.CatchSword);

        if (caughtSword != null)
        {
            Destroy(caughtSword);
            Sword = null;
        }
    }


    public IEnumerator BusyFor(float _seconds)
    {
        IsBusy = true;
        yield return new WaitForSeconds(_seconds);
        IsBusy = false;
    }

    public void AnimationTrigger() => StateMachine.CurrentState?.AnimationFinishTrigger();


    public override void Die()
    {
        base.Die();
        RequestState(PlayerStateId.Dead);
    }

    protected override void SetupZeroKnockbackPower()
    {
        knockbackPower = new Vector2(0, 0);
    }
}
