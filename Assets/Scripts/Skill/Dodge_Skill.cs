using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UISkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage Dodge")]
    [SerializeField] private UISkillTreeSlot unlockMirageDodgeButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    private void UnlockDodge()
    {
        if (dodgeUnlocked)
            return;

        if (unlockDodgeButton.unlocked)
        {
            player.stat.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if (dodgeMirageUnlocked)
            return;

        if (unlockMirageDodgeButton.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDirection, 0));
    }
}
