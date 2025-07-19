using UnityEngine;

// summary:
// This script defines a ChestItem class that represents an item in a chest.
// It includes properties for the item type, name, and point value.
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