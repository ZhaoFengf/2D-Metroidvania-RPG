using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1f;
    [Space]
    [Header("Clone attack")]
    [SerializeField] private UISkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneMultiplier; //克隆倍增器
    [SerializeField] private bool canAttack;

    [Header("Aggressive clone")]
    [SerializeField] private UISkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect { get; private set; }

    [Header("Multiple clone")]
    [SerializeField] private UISkillTreeSlot multipleUnlockButton;
    [SerializeField] private float multipleCloneMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate = 50f;

    [Header("Crystal instead of clone")]
    [SerializeField] private UISkillTreeSlot crystalInsteadUnlockButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttcak);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressive);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);

    }

    #region Unlock 

    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockCloneAttcak();
        UnlockAggressive();
        UnlockMultiClone();
        UnlockCrystalInstead();
    }

    private void UnlockCloneAttcak()
    {
        if (canAttack)
            return;

        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneMultiplier;
        }
    }

    private void UnlockAggressive()
    {
        if (canApplyOnHitEffect)
            return;

        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (canDuplicateClone)
            return;

        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultiplier = multipleCloneMultiplier;
        }
    }

    private void UnlockCrystalInstead()
    {
        if (crystalInsteadOfClone)
            return;

        if (crystalInsteadUnlockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }
    #endregion


    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        //这里需要计算的是偏移后的位置，不然朝向可能不对
        newClone.transform.position = _clonePosition.position + _offset;

        newClone.GetComponent<Clone_Skill_Contorller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform, 25), canDuplicateClone, chanceToDuplicate, player, attackMultiplier);
    }


    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        //if (canCreateCloneOnCounterAttack)
        StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(1f * player.facingDirection, 0)));
    }

    private IEnumerator CloneDelayCorotine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.5f);
        CreateClone(_transform, _offset); //这里后面可能得修改，现在生成的clone在敌人背后但朝向没有翻转
    }
}
