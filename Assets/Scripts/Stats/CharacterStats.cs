using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,

    damage,
    critChance,
    critPower,

    health,
    armor,
    evasion,
    magicResistance,

    fireDamage,
    iceDamage,
    lightningDamage,
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; //力量
    public Stat agility; //敏捷
    public Stat intelligence; //智力
    public Stat vitality; //体力

    [Header("Offsensive stats")]
    public Stat damage;
    public Stat critChance; //暴击率
    public Stat critPower;  //暴击伤害


    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor; // 护甲
    public Stat evasion; //闪避
    public Stat magicResistance; //魔抗

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;


    public bool isIgnited; //燃烧
    public bool isChilled; //减少20%护甲
    public bool isShocked; //减少20%命中率


    [SerializeField] private float alimentDuration = 2f;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCoolDown = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();

        ////装备武器
        //damage.AddModifier(4);
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited)
            ApplyIgniteDamage();

    }

    public void MakeVulnerableFor(float _duration)=> StartCoroutine(VulnerableCortuine(_duration));

    private IEnumerator VulnerableCortuine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);

        DoMagicDamage(_targetStats);//这个可能会导致双重判定，即物理和法术攻击均判定一次，假如其有俩属性
    }

    #region Magic Damage and Alilments
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0) return;
        bool flowControl = AttemptToApplyAliments(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
        if (!flowControl)
        {
            return;
        }

    }

    private bool AttemptToApplyAliments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool canAppplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canAppplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canAppplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canAppplyIgnite && !canAppplyChill && !canAppplyShock)
        {
            if (Random.value < .5f && _fireDamage > 0)
            {
                canAppplyIgnite = true;
                _targetStats.ApplyAilments(canAppplyIgnite, canAppplyChill, canAppplyShock);
                return false;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canAppplyChill = true;
                _targetStats.ApplyAilments(canAppplyIgnite, canAppplyChill, canAppplyShock);
                return false;
            }

            if (Random.value < .5f && _lightningDamage > 0)
            {
                canAppplyShock = true;
                _targetStats.ApplyAilments(canAppplyIgnite, canAppplyChill, canAppplyShock);
                return false;
            }
        }

        if (canAppplyIgnite)
            _targetStats.SetUpIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canAppplyShock)
            _targetStats.SetUpShockDamage(Mathf.RoundToInt(_lightningDamage * .2f));

        _targetStats.ApplyAilments(canAppplyIgnite, canAppplyChill, canAppplyShock);
        return true;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        //if (isIgnited || isChilled || isShocked)
        //    return;
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = alimentDuration;

            fx.IgniteFxFor(alimentDuration);
        }
        if(_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = alimentDuration;

            float slowPercentage = .5f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, alimentDuration);

            fx.ChillFxFor(alimentDuration);
        }
        if(_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)//这里后续得修改，关于敌人攻击玩家时的处理
                    return;
                HitNearestEnemyShockStrike();
            }
        }

        //isChilled = _chill;
        //isShocked = _shock;
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            //Debug.Log("ignite damage" + igniteDamage);
            //currentHealth -= igniteDamage;
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCoolDown;
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = alimentDuration;

        fx.ShockFxFor(alimentDuration);
    }

    private void HitNearestEnemyShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform cloestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    cloestEnemy = hit.transform;
                }

            }
            if (cloestEnemy == null)
                cloestEnemy = transform;
        }

        if (cloestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrikeController>().SetUp(shockDamage, cloestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetUpIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetUpShockDamage(int _damage) => shockDamage = _damage;
    #endregion


    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible) //无敌
            return;

        DecreaseHealthBy(_damage);


        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
            Die();

    }

    public virtual void IncreaseHealthBy(int _healAmount)
    {
        currentHealth += _healAmount;
        //currentHealth = Mathf.Clamp(currentHealth, 0, GetMaxHealthValue());
        if(currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.2f);

        currentHealth -= _damage;

        if(onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
        //throw new NotImplementedException();
    }

    public void KillEntity()
    {
        if(!isDead)
            Die();
    }

    public void MakeInvencible(bool _invencibe)
    {
        isInvincible = _invencibe;
    }


    #region Stat Calulation
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.2f); //降低20%护甲
        else 
            totalDamage -= _targetStats.armor.GetValue(); //护甲减伤

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);

        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;//降低闪避率

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }

    protected bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) < totalCritChance)
        {
            //Debug.Log("crit");
            return true;
        }
        return false;
    }

    protected int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue())/100f; //可以修改暴击伤害的计算方式
        //Debug.Log("totalCritPower: " + totalCritPower);
        float totalCritDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(totalCritDamage);
    }

    public int GetMaxHealthValue()
    {
        int totalMaxHealth = maxHealth.GetValue() + vitality.GetValue() * 5;
        return totalMaxHealth;
    }
    #endregion


    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.intelligence) return intelligence;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.armor) return armor;
        else if (_statType == StatType.evasion) return evasion;
        else if (_statType == StatType.magicResistance) return magicResistance;
        else if (_statType == StatType.fireDamage) return fireDamage;
        else if (_statType == StatType.iceDamage) return iceDamage;
        else if (_statType == StatType.lightningDamage) return lightningDamage;

        return null;
    }
}
