using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory")]
    public List<KeyType> keys = new List<KeyType>();
    public List<ItemType> food = new List<ItemType>(); // Hier können verschiedene Nahrungsmittel-Typen gespeichert werden
    public List<CollectibleData> items = new List<CollectibleData>();

    public void AddKey(KeyType keyType)
    {
        keys.Add(keyType);
        UIManager.Instance.UpdateKeyDisplay(keys);
        Debug.Log($"{keyType} hinzugefügt. Aktuelle Schlüssel: {keys.Count}");
    }

    public void AddFood()
    {  
        food.Add(ItemType.Food);
        UIManager.Instance.UpdateFoodDisplay(food);
        Debug.Log("Essen zum Inventar hinzugefügt.");
    }

    public bool HasKey(KeyType keyType)
    {
        return keys.Contains(keyType);
    }

    public bool HasFood()
    {
        return food.Count > 0;
    }


    public void UseKey(KeyType keyType)
    {
        if (HasKey(keyType))
        {
            keys.Remove(keyType);
            UIManager.Instance.UpdateKeyDisplay(keys);
            Debug.Log($"{keyType} verwendet. Verbleibende Schlüssel: {keys.Count}");
        }
        else
        {
            Debug.LogWarning($"{keyType} nicht im Inventar.");
        }
    }

    public void UseFood()
    {
        if (HasFood())
        {
            /// TODO: Taste für Essen verwenden
            food.Remove(ItemType.Food); // Entfernt ein beliebiges Nahrungsmittel
            UIManager.Instance.UpdateFoodDisplay(food);
            Debug.Log("Essen verwendet.");
        }
        else
        {
            Debug.LogWarning("Kein Essen im Inventar.");
        }
    }

    // Anzahl eines bestimmten Schlüssel-Typs
    public int GetKeyCount(KeyType keyType)
    {
        return keys.Count(key => key == keyType);
    }
    
    // Gesamte Schlüssel-Anzahl
    public int GetTotalKeyCount()
    {
        return keys.Count;
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

    // Debug-Ausgabe für Inventar
    public void DebugInventory()
    {
        Debug.Log("=== INVENTAR ===");
        Debug.Log($"Schlüssel gesamt: {keys.Count}");
        foreach (KeyType key in keys)
        {
            Debug.Log($"- {key}");
        }
        Debug.Log($"Items gesamt: {items.Count}");
        Debug.Log($"Gesamtpunktzahl: {GetTotalScore()}");
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
        type = t; 
        name = n; 
        pointValue = v;
    }
}