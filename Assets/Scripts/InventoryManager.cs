using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Inventory")]
    public List<KeyType> keys = new List<KeyType>();
    public List<string> food = new List<string>();
    private bool gameLoaded;

    public void AddKey(KeyType keyType)
    {
        keys.Add(keyType);
        UIManager.Instance.UpdateKeyDisplay(keys);
        Debug.Log($"{keyType} hinzugefügt. Aktuelle Schlüssel: {keys.Count}");
    }

    public void AddFood()
    {
        food.Add("Schinken");
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
            UpdateKeys();
            Debug.Log($"{keyType} verwendet. Verbleibende Schlüssel: {keys.Count}");
        }
        else
        {
            Debug.LogWarning($"{keyType} nicht im Inventar.");
        }
    }

    public void UpdateKeys()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKeyDisplay(keys);
            Debug.Log("Schlüssel-UI aktualisiert.");
        }
    }

    public void UseFood()
    {
        if (HasFood())
        {
            food.Remove("Schinken");
            UIManager.Instance.UpdateFoodDisplay(food);
            Debug.Log("Essen verwendet.");
        }
        else
        {
            Debug.LogWarning("Kein Essen im Inventar.");
        }
    }

    public int GetKeyCount(KeyType keyType)
    {
        return keys.Count(key => key == keyType);
    }

    public int GetTotalKeyCount()
    {
        return keys.Count;
    }

    //public void AddItem(ItemType type, string name, int value)
    //{
    //    UIManager.Instance.AddScore(value);
    //}

    public int GetTotalScore()
    {
        return SaveSystem.Instance.currentSaveData.scoreData;
    }

    public void DebugInventory()
    {
        Debug.Log("=== INVENTAR ===");
        Debug.Log($"Schlüssel gesamt: {keys.Count}");
        foreach (KeyType key in keys)
        {
            Debug.Log($"- {key}");
        }
        Debug.Log($"Items gesamt: {SaveSystem.Instance.currentSaveData.scoreData}");
        Debug.Log($"Gesamtpunktzahl: {GetTotalScore()}");
    }

    public void LoadInventoryFromSaveData(InventoryData data)
    {
        Debug.Log("=== LADE INVENTAR ===");

        ClearAllInventories();

        // Schlüssel laden
        LoadKeys(data.collectedKeys);

        // Essen laden
        LoadFood(data.collectedFood);

        // Score laden und UI aktualisieren
        //LoadCurrentScore(SaveSystem.Instance.currentSaveData.scoreData);

        UpdateUIAndLog();

        UIManager.Instance.UpdateScore();
    }

    private void ClearAllInventories()
    {
        keys.Clear();
        food.Clear();
        Debug.Log("Inventar geleert vor dem Laden.");
    }

    private void LoadKeys(CollectedKeyData[] keyDataArray)
    {
        if (keyDataArray == null)
        {
            Debug.Log("Keine Schlüssel-Daten zum Laden vorhanden.");
            return;
        }

        Debug.Log($"Lade {keyDataArray.Length} Schlüssel-Einträge...");

        foreach (var keyData in keyDataArray.Where(k => k?.isCollected == true))
        {
            if (Enum.TryParse(keyData.keyName, out KeyType keyType))
            {
                keys.Add(keyType);
                Debug.Log($"Schlüssel {keyData.keyName} geladen");
            }
            else
            {
                Debug.LogWarning($"Unbekannter Schlüsselname beim Laden: {keyData.keyName}");
            }
        }

        // WICHTIG: UI nach dem Laden aktualisieren
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKeyDisplay(keys);
            Debug.Log($"Schlüssel-UI aktualisiert mit {keys.Count} Schlüsseln");
        }
    }

    private void LoadFood(CollectedFoodData[] foodDataArray)
    {
        if (foodDataArray == null)
        {
            Debug.Log("Keine Essen-Daten zum Laden vorhanden.");
            return;
        }

        Debug.Log($"Lade {foodDataArray.Length} Essen-Einträge...");

        foreach (var foodData in foodDataArray.Where(f => f?.isCollected == true))
        {
            food.Add(foodData.foodName);
            Debug.Log($"Essen {foodData.foodName} geladen");
        }

        // WICHTIG: UI nach dem Laden aktualisieren
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateFoodDisplay(food);
            Debug.Log($"Essen-UI aktualisiert mit {food.Count} Essen-Items");
        }
    }

    //private void LoadCurrentScore(int score)
    //{
    //    if (UIManager.Instance != null)
    //    {
    //        UIManager.Instance.UpdateScore(score);
    //        Debug.Log($"Score UI aktualisiert: {score}");
    //    }
    //}

    private void UpdateUIAndLog()
    {
        Debug.Log("=== INVENTAR NACH DEM LADEN ===");
        Debug.Log($"Schlüssel gesamt: {keys.Count}");
        foreach (KeyType key in keys)
        {
            Debug.Log($"- {key}");
        }
        Debug.Log($"Essen gesamt: {food.Count}");
        Debug.Log($"Gesamtpunktzahl: {GetTotalScore()}");

        // Finale UI-Aktualisierung zur Sicherheit
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKeyDisplay(keys);
            UIManager.Instance.UpdateFoodDisplay(food);
            UIManager.Instance.UpdateScore();
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
}