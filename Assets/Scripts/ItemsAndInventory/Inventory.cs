using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UIItemSlot[] inventoryItemSlot;
    private UIItemSlot[] stashItemSlot;
    private UIEqupimentSlot[] equipmentSlot;
    private UIStatSlot[] statSlot;

    [Header("Items cooldown")]
    private float flaskLastUsedTime;
    private float armorLastUsedTime;

    public float flaskCooldown { get; private set; }
    private float armorCooldown;

    [Header("Database")]
    public List<ItemData> itemDatabase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UIItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UIEqupimentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UIStatSlot>();
        AddSatrtingItems();
    }

    private void AddSatrtingItems()
    {
        if(loadedEquipment.Count > 0)
        {
            foreach(ItemData_Equipment item in loadedEquipment)
            {
                EquipItem(item);
            }
        }

        if(loadedItems.Count > 0)
        {
            foreach(InventoryItem item in loadedItems)
            {
                for(int i=0; i< item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }

            return;
        }

        for (int i = 0; i < startingItems.Count; i++)
        {
            if(startingItems[i] != null)
                AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }
        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }
            

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            //AddItem(itemToRemove);
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatUI();
    }

    public void UpdateStatUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();//这里还有很多问题，需要修改
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if(_item.itemType == ItemType.Material)
            AddToStash(_item);


        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem inventoryValue))
        {
            if(inventoryValue.stackSize <= 1)
            {
                inventory.Remove(inventoryValue);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                inventoryValue.RemoveStack();
            }
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    public bool CanAddItem() //这里有问题，就是当背包满了之后也不能添加相同的装备
    {
        if(inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("not enough set");
            return false;
        }
        return true;
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requireMaterials)
    {
        foreach(var requiredItem in _requireMaterials)
        {
            if(stashDictionary.TryGetValue(requiredItem.data, out InventoryItem stashItem))
            {
                if(stashItem.stackSize < requiredItem.stackSize)
                {
                    Debug.Log("not enough material");
                    return false;
                }
            }
            else
            {
                Debug.Log("not enough material");
                return false;
            }
        }

        foreach(var requiredMaterial in _requireMaterials)
        {
            for(int i = 0; i < requiredMaterial.stackSize; i++)
            {
                RemoveItem(requiredMaterial.data);
            }
        }

        AddItem(_itemToCraft);

        return true;
        /* 上面新的是修复代码，仅供参考学习
        //新增在满了之后不能制造
        if (!CanAddItem())
            return false;

        List<InventoryItem> materialsToMove = new List<InventoryItem>();

        for(int i =0; i< _requireMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requireMaterials[i].data, out InventoryItem statValue))
            {
                if(statValue.stackSize < _requireMaterials[i].stackSize)
                {
                    Debug.Log("not enough material");
                    return false;
                }
                else
                {
                    //materialsToMove.Add(statValue);
                    materialsToMove.Add(_requireMaterials[i]);
                }
            }
            else
            {
                Debug.Log("not enough material");
                return false;
            }
        }
        foreach(var material in materialsToMove)
        {
            for (int i = 0; i < material.stackSize; i++)
            {
                RemoveItem(material.data);
            }
        }

        AddItem(_itemToCraft);
        return true;
        */
    }

    public List<InventoryItem> GetEquipmentList() => equipment;
    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                equipedItem = item.Key;
        }
        return equipedItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);
        if(currentFlask != null)
        {
            bool canUseFlask = Time.time > flaskLastUsedTime + flaskCooldown;
            if (canUseFlask)
            {
                flaskCooldown = currentFlask.itemCooldown;
                currentFlask.Effect(null);
                flaskLastUsedTime = Time.time;
            }
            else
            {
                Debug.Log("flask on cooldown");
            }
        }
    }

    public bool canUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (currentArmor == null)
            return false;

        if(Time.time > armorLastUsedTime + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            armorLastUsedTime = Time.time;
            return true;
        }
        Debug.Log("armor cooldown");
        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach(var item in itemDatabase)//GetItemDataBase()
            {
                if(item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach(string equipmentId in _data.equipment)
        {
            foreach(var item in itemDatabase)//GetItemDataBase()
            {
                if(item != null && item.itemId == equipmentId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }

        Debug.Log("inventory load");
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipment.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach(KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        //由于数据在上述已经有保存，这里理论上只要有id就行
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipment.Add(pair.Key.itemId);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("fill up item database")]
    private void FillUpItemDatabase()
    {
        itemDatabase = new List<ItemData>(GetItemDataBase());
    }

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items"});
        //string[] assetNames = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Data/Equipment"});

        foreach(string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDatabase.Add(itemData);
        }
        return itemDatabase;
    }
#endif
}
