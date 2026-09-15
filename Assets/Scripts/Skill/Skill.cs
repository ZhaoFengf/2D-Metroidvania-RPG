using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float coolDownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;

        //CheckUnlock();
        Invoke(nameof(CheckUnlock), 0.1f);
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }


    protected virtual void CheckUnlock()
    {

    }
    //protected Player player { get; private set; }

    //public void Initialize(Player owner)
    //{
    //    player = owner;
    //    CheckUnlock();
    //}

    public virtual bool TryUseSkill()
    {
        if(coolDownTimer < 0)
        {
            UseSkill();
            coolDownTimer = cooldown;
            return true;
        }
        player.fx.CreatePopupText("cooldown");
        return false;
    }

    public virtual void UseSkill()
    {
        //use skill
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform, float _findRadius = 25)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, _findRadius);
        float closestDistance = Mathf.Infinity;
        Transform cloestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    cloestEnemy = hit.transform;
                }

            }
        }
        //return cloestEnemy; 换为下面这个，避免找不到敌人时返回空，导致报错
        return cloestEnemy != null ? cloestEnemy : _checkTransform;
    }
}
