using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;
    [SerializeField] private List<UICraftSlot> craftSlots;
    void Start()
    {
        AssignCraftSlots();
    }

    private void AssignCraftSlots()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UICraftSlot>());
        }
    }

    public void SetUpCraftList()
    {
        for(int i=0; i< craftSlots.Count; i++)
        {
            Destroy(craftSlots[i].gameObject);
        }

        craftSlots = new List<UICraftSlot>();

        for(int i=0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UICraftSlot>().SetUpCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetUpCraftList();
    }
}
