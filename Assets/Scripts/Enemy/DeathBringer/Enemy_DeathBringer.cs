using System.Collections;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    # region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }

    #endregion

    public bool bossFightBegin;

    [Header("Spell cast details")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpells;
    public float spellCooldown;
    public float lastTimeCast;
    [SerializeField] private float spellStateCooldown = 5f;

    [Header("Teleport details")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport = 25;
    public float defaultChanceToTeleport = 25;


    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);

        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);

        deadState = new DeathBringerDeadState(this, stateMachine, "Idle", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "SpellCast", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }


    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;

        float xOffset = Random.Range(-player.facingDirection, player.facingDirection * 2);

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + 2.5f);

        if (player.rb.velocity.x == 0)
            spellPosition.x = player.transform.position.x;

        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<DeadBringerSpellController>().SetupSpell(stat);
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 2.5f, arena.bounds.max.x - 2.5f);
        float y = Random.Range(arena.bounds.min.y + 2.5f, arena.bounds.max.y - 2.5f);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));
        
        if(!GroundBelow() || SomethingisAround())
        {
            FindPosition();
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 50, groundLayer);
    private bool SomethingisAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0f, Vector2.zero, 0f, groundLayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }

    public bool canTeleport()
    {
        float randomValue = Random.Range(0f, 100f);

        if(randomValue <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
        }
        return randomValue <= chanceToTeleport;
    }

    public bool canDoSpellCast()
    {
        if(Time.time >= lastTimeCast + spellStateCooldown)
        {
            //lastTimeCast = Time.time;
            return true;
        }
        return false;
    }
}
