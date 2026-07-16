using TMPro;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    public InventoryItem item;
    protected UI ui;


    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _item)
    {
        item = _item;

        itemImage.color = Color.white;
        if (item != null)
        {
            itemImage.sprite = item.data.Icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if(item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
            //Debug.Log("equiped new item" + item.data.ItemName);
        }

        ui.itemToolTip.HideToolTip();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        //可以将这一段使用继承封装
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

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
        ui.itemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;

        ui.itemToolTip.HideToolTip();
    }

}

