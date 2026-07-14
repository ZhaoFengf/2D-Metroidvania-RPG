using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int amountOfItems;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();
    [SerializeField] private GameObject dropPrefab;
    //[SerializeField] private ItemData item;

    public virtual void GenerateDrop()
    {
        for(int i=0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0,100)< possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        int itemsToDrop = Mathf.Min(amountOfItems, dropList.Count);
        //for(int i=0; i<amountOfItems; i++)
        for (int i=0; i< itemsToDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count)];
            
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(10, 12));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
