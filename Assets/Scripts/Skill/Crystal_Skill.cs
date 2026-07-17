using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//似乎有点bug，就是水晶不能自动追踪时候会直接炸开，这个后续得修改一下
public class Crystal_Skill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration = 2.5f;
    private GameObject currentCrystal;

    [Header("Crystal simple")]
    [SerializeField] private UISkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal mirage")]
    [SerializeField] private UISkillTreeSlot unlockCloneInsteadButton;
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private UISkillTreeSlot unlockExplosiveButton;
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private UISkillTreeSlot unlockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private UISkillTreeSlot unlockMultiCrystalButton;
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCoolDown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalleft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);

        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);

        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);

        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);

        unlockMultiCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiSatckCrystal);
    }

    #region Unlock skill

    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
            cloneInsteadOfCrystal = true;
    }
    
    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
            canExplode = true;
    }
    
    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }
    
    private void UnlockMultiSatckCrystal()
    {
        if (unlockMultiCrystalButton.unlocked)
            canUseMultiStack = true;
    }
    #endregion




    public override void UseSkill()
    {
        base.UseSkill();

        if (canUseMultiCrystal())
            return;


        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            //这里我们换成可以在一定范围内移动到敌人身上，否则可以交换位置。但是移动后原地爆炸的还没解决
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }

        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetUpCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform, 25), player);

        //CurentCrystalChooseRandomTarget();
    }

    public void CurentCrystalChooseRandomTarget() =>  currentCrystal.GetComponent<Crystal_Skill_Controller>()?.ChooseRandomEnemy();

    private bool canUseMultiCrystal()
    {
        if (canUseMultiStack)
        {
            if(crystalleft.Count > 0)
            {
                if(crystalleft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0;
                GameObject crystalToSpawn = crystalleft[crystalleft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                crystalleft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetUpCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform, 25), player);
            
                if(crystalleft.Count <= 0)
                {
                    cooldown = multiStackCoolDown;
                    RefilCrystal();
                }
                return true;
            }
        }
        return false;
    }

    private void RefilCrystal()
    {
        int amountToRefill = amountOfStacks - crystalleft.Count;

        for (int i = 0; i < amountToRefill; i++)
        {
            crystalleft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (coolDownTimer > 0)
            return;

        coolDownTimer = multiStackCoolDown;
        RefilCrystal();
    }
}
