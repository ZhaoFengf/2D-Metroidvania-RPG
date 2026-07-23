using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int maxItemsToDrop;
    [SerializeField] private ItemData[] itemPool;
    private List<ItemData> possibleDrop = new List<ItemData>();
    [SerializeField] private GameObject dropPrefab;
    //[SerializeField] private ItemData item;

    public virtual void GenerateDrop()
    {
        if(itemPool.Length == 0)
        {
            Debug.LogWarning("Item pool is empty. No items to drop.");
            return;
        }
        foreach(ItemData item in itemPool)
        {
            if(item != null && Random.Range(0f, 100f) < item.dropChance)
            {
                possibleDrop.Add(item);
            }
        }
        for(int i =0; i < maxItemsToDrop; i++)
        {
            if(possibleDrop.Count > 0)
            {
                int randomIndex = Random.Range(0, possibleDrop.Count);
                ItemData itemToDrop = possibleDrop[randomIndex];
                DropItem(itemToDrop);
                possibleDrop.Remove(itemToDrop);
            }
        }


        //for(int i=0; i < itemPool.Length; i++)
        //{
        //    if(Random.Range(0,100)< itemPool[i].dropChance)
        //    {
        //        possibleDrop.Add(itemPool[i]);
        //    }
        //}

        //int itemsToDrop = Mathf.Min(maxItemsToDrop, possibleDrop.Count);

        //for (int i=0; i< itemsToDrop; i++)
        //{
        //    ItemData randomItem = possibleDrop[Random.Range(0, possibleDrop.Count)];

        //    possibleDrop.Remove(randomItem);
        //    DropItem(randomItem);
        //}
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(10, 12));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
