using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//显示item的装备基本信息
/*
 由于tooltip的显示位置与右上角的装备有所重叠，可能会闪动，目前采用添加canvas group的方式，取消勾选BlocksRaycasts 
 */
public class UIItemToolTip : UIToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private float defaultFontSize = 38f;

    public void ShowToolTip(ItemData_Equipment _item)
    {
        itemNameText.text = _item.ItemName;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescription.text = _item.GetDescription();

        if (itemNameText.text.Length > 12)
            itemNameText.fontSize = defaultFontSize * .8f;
        else             
            itemNameText.fontSize = defaultFontSize;

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
