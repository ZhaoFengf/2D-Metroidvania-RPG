using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private int skillPrize;
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

        if (PlayerManager.instance.HaveEnoughMoney(skillPrize) == false)
        {
            Debug.Log("no enough JiNeng Dian");
            return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);

        //方便显示,感觉装备那边更加需要，而技能就显示在左下角即可
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 450)
            xOffset = -150;
        else
            xOffset = 150;
        if (mousePosition.y > 200)
            yOffset = -100;
        else
            yOffset = 100;

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
