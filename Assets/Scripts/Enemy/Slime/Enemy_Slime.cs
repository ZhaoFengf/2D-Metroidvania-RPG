using UnityEngine;

public enum SlimeType
{
    big, 
    medium,
    small
}

public class Enemy_Slime : Enemy
{
    [Header("Slime specifiic")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimesToCreate;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreateVelocity;
    [SerializeField] private Vector2 maxCreateVelocity;

    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);

        if (slimeType == SlimeType.small)
            return;

        CreateSlimes(slimesToCreate, slimePrefab);
    }

    private void CreateSlimes(int _amountOfSlimes, GameObject _slimePrefab)
    {
        for(int i =0; i< _amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetupSlime(facingDirection);
        }
    }

    public void SetupSlime(int _facingDir)
    {
        if (_facingDir != facingDirection)
            Flip();

        float xVelocity = Random.Range(minCreateVelocity.x, maxCreateVelocity.x);
        float yVelocity = Random.Range(minCreateVelocity.y, maxCreateVelocity.y);

        isKnocekbacked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * -facingDirection, yVelocity);

        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback() => isKnocekbacked = false;
}

