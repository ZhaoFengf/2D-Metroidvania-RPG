using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
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
    }

    //private void OnEnable()
    //{
    //    UpdateSlot(item);
    //}


    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;
        Inventory.instance.CanCraft(craftData, craftData.craftingMaterial);
    }
}
