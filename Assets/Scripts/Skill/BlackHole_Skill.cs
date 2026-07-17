using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHole_Skill : Skill
{
    [SerializeField] private UISkillTreeSlot blackHoleUnlockButton;
    public bool blackHoleUnlocked { get; private set; }

    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCoolDown;
    [SerializeField] private float blackHoleDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private int maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    BlackHole_Skill_Controller currentBlackHole;


    private void UnlockBlackHole()
    {
        if (blackHoleUnlocked)
            return;

        if (blackHoleUnlockButton.unlocked)
        {
            blackHoleUnlocked = true;
        }
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackHole = newBlackHole.GetComponent<BlackHole_Skill_Controller>();

        currentBlackHole.SetUpBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCoolDown, blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();

        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (!currentBlackHole)
            return false;
        if (currentBlackHole.PlayerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }
        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
