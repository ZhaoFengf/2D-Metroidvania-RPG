using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UISkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UISkillTreeSlot parryRestoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    public bool parryRestoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UISkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (parryRestoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stat.GetMaxHealthValue() * restoreHealthPercentage);
            player.stat.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    private void UnlockParry()
    {
        if(parryUnlockButton.unlocked)
            parryUnlocked = true;
    } 
    private void UnlockParryRestore()
    {
        if(parryRestoreUnlockButton.unlocked)
            parryRestoreUnlocked = true;
    } 

    private void UnlockParryWithMirage()
    {
        if(parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}
