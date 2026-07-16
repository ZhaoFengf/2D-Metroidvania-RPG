using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1f;
    [Space]
    [SerializeField] private bool canAttack = true;

    //[SerializeField] private bool createCloneStart;
    //[SerializeField] private bool createCloneOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Header("Clone Duplication")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate = 50f;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;


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

        newClone.GetComponent<Clone_Skill_Contorller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform, 25), canDuplicateClone, chanceToDuplicate, player);
    }


    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1f * player.facingDirection, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.5f);
        CreateClone(_transform, _offset); //这里后面可能得修改，现在生成的clone在敌人背后但朝向没有翻转
    }
}
