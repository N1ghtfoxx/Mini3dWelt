using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    public List<KeyType> keys = new List<KeyType>();
    public List<CollectibleData> items = new List<CollectibleData>();

    public void AddKey(KeyType keyType)
    {
        keys.Add(keyType);
        UIManager.Instance.UpdateKeyDisplay(keys);
    }

    public bool HasKey(KeyType keyType)
    {
        return keys.Contains(keyType);
    }

    public void AddItem(ItemType type, string name, int value)
    {
        items.Add(new CollectibleData(type, name, value));
        UIManager.Instance.UpdateItemCount(items.Count);
    }

    public int GetTotalScore()
    {
        return items.Sum(item => item.pointValue);
    }
}

[System.Serializable]
public class CollectibleData
{
    public ItemType type;
    public string name;
    public int pointValue;

    public CollectibleData(ItemType t, string n, int v)
    {
        type = t; name = n; pointValue = v;
    }
}
