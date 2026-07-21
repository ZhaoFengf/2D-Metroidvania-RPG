using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stat { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    [Header("Collision")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask groundLayer;

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackPower;
    [SerializeField] protected float knockBackDuration = 0.07f;
    protected bool isKnocekbacked = false;

    public int knockbackDir { get; private set; }
    public int facingDirection { get; private set; } = 1;
    protected bool isFacingRight = true;

    public System.Action onFlipped;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        fx = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1f;
    }

    public virtual void DamageImpact()
    {
        //fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
    }

    public virtual void SetupKnockbackDir(Transform _damageDir)
    {
        if(_damageDir.position.x > transform.position.x)
            knockbackDir = -1;
        else
            knockbackDir = 1;
    }

    public void SetupKnockbackPower(Vector2 _knockbackPower) => knockbackPower = _knockbackPower;

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocekbacked = true;
        rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);

        yield return new WaitForSeconds(knockBackDuration);
        isKnocekbacked = false;

        SetupZeroKnockbackPower();
    }

    protected virtual void SetupZeroKnockbackPower()
    {

    }

    #region Velocity
    public void SetZeroVelocity()
    {
        if(isKnocekbacked)
            return;
        rb.velocity = Vector2.zero;
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if(isKnocekbacked)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    public virtual bool isGroundedDeteced() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);//0,-1,0
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);//1,0,0
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);

        if(onFlipped != null)
            onFlipped();
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !isFacingRight || _x < 0 && isFacingRight)
            Flip();
    }
    #endregion

    

    public virtual void Die()
    {

    }
}
