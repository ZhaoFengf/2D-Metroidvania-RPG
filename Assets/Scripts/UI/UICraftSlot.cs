using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
    private int defaultFontSize = 24;
    protected override void Start()
    {
        base.Start();
    }

    public void SetUpCraftSlot(ItemData_Equipment _data)
    {
        if (_data == null)
            return;

        item.data = _data;

        itemImage.sprite = _data.Icon;

        itemText.text = _data.ItemName;

        if (itemText.text.Length > 12)
            itemText.fontSize = defaultFontSize * .7f;
        else
            itemText.fontSize = defaultFontSize;
    }

    //private void OnEnable()
    //{
    //    UpdateSlot(item);
    //}


    public override void OnPointerDown(PointerEventData eventData)
    {
        //ItemData_Equipment craftData = item.data as ItemData_Equipment;
        //Inventory.instance.CanCraft(craftData, craftData.craftingMaterial);
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
