using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlot : UIToolTip,IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;
    
    public bool unlocked;
    private Image skillImage;

    [SerializeField] private UISkillTreeSlot[] shouldBeLocked;
    [SerializeField] private UISkillTreeSlot[] shouleBeUnlocked;


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {   
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        if(unlocked)
            skillImage.color = Color.white;
    }

    //实现前置锁和分支锁
    public void UnlockSkillSlot()
    {
        for(int i =0; i< shouleBeUnlocked.Length; i++)
        {
            if (!shouleBeUnlocked[i].unlocked)
            {
                Debug.Log("cannot unlock skill");
                return;
            }
        }

        for(int i=0; i<shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked)
            {
                Debug.Log("cannnot unlock skill");
                return;
            }
        }

        if (unlocked)
        {
            Debug.Log("already unlocked");
            return;
        }

        if (PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
        {
            Debug.Log("no enough JiNeng Dian");
            return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName, skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else 
            _data.skillTree.Add(skillName, unlocked);
    }
}
