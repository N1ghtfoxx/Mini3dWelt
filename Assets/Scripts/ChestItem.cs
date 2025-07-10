using UnityEngine;

[System.Serializable]
public class ChestItem
{
    public ItemType itemType;
    public string itemName;
    public int pointValue;

    public ChestItem(ItemType type, string name, int value)
    {
        itemType = type;
        itemName = name;
        pointValue = value;
    }


}

public enum ChestItemType
{
    Gem,      // Edelsteine
}

