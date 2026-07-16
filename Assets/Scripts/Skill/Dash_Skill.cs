using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private UISkillTreeSlot dashUnlockedButton;

    [Header("Clone on dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UISkillTreeSlot cloneOnDashUnlockedButton;

    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UISkillTreeSlot cloneOnArrivalUnlockedButton;


    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
    }

    private void UnlockDash()
    {
        if(dashUnlockedButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        //if(cloneOnDashUnlockedButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        //if(cloneOnArrivalUnlockedButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
}
