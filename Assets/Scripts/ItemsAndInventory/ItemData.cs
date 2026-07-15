using UnityEngine;
using System.Text;

public enum ItemType
{
    Material,
    Equipment
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string ItemName;
    public Sprite Icon; //视频中将其重命名为了itemIcon

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }
}
