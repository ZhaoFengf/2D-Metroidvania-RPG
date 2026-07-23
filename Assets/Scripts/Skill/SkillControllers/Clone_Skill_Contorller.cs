using System.Collections;
using UnityEngine;

public class Clone_Skill_Contorller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorFadeSpeed = 1f;

    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.5f;
    private int faceDirection = 1;

    private bool canDuplicateClone;
    private float chanceToDuplicate = 50f;

    [Space]
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float cloestEnemyCheckRadius = 25f;
    [SerializeField ] private Transform cloestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        StartCoroutine(FaceClosestTarget());

    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer <= 0f)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - colorFadeSpeed * Time.deltaTime);

            if(sr.color.a <= 0f)
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, bool _canDuplicateClone, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1,3));
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        player = _player;
        attackMultiplier = _attackMultiplier;

        //FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer -= .1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //hit.GetComponent<Enemy>().DamageEffect();
                //player.stat.DoDamage(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDodamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);
                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }

                if (canDuplicateClone)
                {
                    if(Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.8f * faceDirection, 0));
                    }
                }
            }
        }
    }

    private IEnumerator FaceClosestTarget()
    {
        yield return null;

        FindClosestEnemy();

        if(cloestEnemy != null)
        {

            if (transform.position.x > cloestEnemy.position.x)
            {
                faceDirection = -1;
                transform.Rotate(0, 180, 0);
            }

        }
    }

    private void  FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cloestEnemyCheckRadius, whatIsEnemy);
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                cloestEnemy = hit.transform;
            }
        }
    }

}
