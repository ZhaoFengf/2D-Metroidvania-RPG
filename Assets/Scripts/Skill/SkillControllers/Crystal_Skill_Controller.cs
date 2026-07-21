using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    private float crystalExistTimer;

    private bool canExplore;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5f;

    private Transform cloestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    public void SetUpCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _cloestTarget, Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalDuration;
        canExplore = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        cloestTarget = _cloestTarget;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.GetBlackHoleRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if(colliders.Length > 0)
            cloestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            if (cloestTarget == null)
                return;

            if (Vector2.Distance(transform.position, cloestTarget.position) > 25) //假如太远就不移动5f
            {
                //目前是假如直接太远了就不移动，即便后边敌人进入范围
                canMove = false;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, cloestTarget.position, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, cloestTarget.position) < 1.5f)
                {
                    FinishCrystal();
                    canMove = false;
                }
            }
            
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                player.stat.DoMagicDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if(equipedAmulet!= null)//增加饰品的攻击效果
                {
                    equipedAmulet.Effect(hit.transform);
                }
            }
                
            //hit.GetComponent<Enemy>().DamageEffect();
        }
    }

    public void FinishCrystal() //这里先放着，后面可以这样修改，假如是移位，就不爆炸
    {
        if (canExplore)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }

        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
