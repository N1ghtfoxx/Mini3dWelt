using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.Rendering.DebugUI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    //summary:
    //Singleton pattern for InventoryManager
    // This class manages the player's inventory, including keys and food items.
    // It provides methods to add, remove, and check for keys and food, as well as to load inventory data from saved game states.
    // It also updates the UI to reflect changes in the inventory.
    // It uses a singleton pattern to ensure that only one instance of InventoryManager exists in the game.
    // It also provides methods to debug the inventory and log its contents.
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
    public List<KeyType> keys = new List<KeyType>(); // List to hold keys of different types
    public List<string> food = new List<string>(); // List to hold food items collected by the player
    private bool gameLoaded; // Flag to check if the game has been loaded

    //summary:
    // Adds a key of the specified type to the inventory and updates the UI.
    // This method is used to add keys when the player collects them in the game.
    // It takes a KeyType enum as a parameter to specify the type of key being added.
    // It also logs the addition of the key to the console for debugging purposes.
    public void AddKey(KeyType keyType)
    {
        keys.Add(keyType);
        UIManager.Instance.UpdateKeyDisplay(keys);
        Debug.Log($"{keyType} hinzugefügt. Aktuelle Schlüssel: {keys.Count}");
    }

    //summary:
    // Adds a food item to the inventory and updates the UI.
    // This method is used to add food items when the player collects them in the game.
    // It adds a hardcoded food item "Schinken" to the inventory.
    // It also logs the addition of the food item to the console for debugging purposes.
    public void AddFood()
    {
        food.Add("Schinken");
        UIManager.Instance.UpdateFoodDisplay(food);
        Debug.Log("Essen zum Inventar hinzugefügt.");
    }

    //summary:
    // Checks if the inventory contains a key of the specified type.
    // This method is used to determine if the player has a specific key in their inventory.
    // It takes a KeyType enum as a parameter to specify the type of key being checked.
    // It returns true if the key is found, otherwise false.
    public bool HasKey(KeyType keyType)
    {
        return keys.Contains(keyType);
    }

    //summary:
    // Checks if the inventory contains any food items.
    // This method is used to determine if the player has any food items in their inventory.
    // It returns true if there is at least one food item, otherwise false.
    public bool HasFood()
    {
        return food.Count > 0;
    }

    //summary:
    // Uses a key of the specified type from the inventory.
    // This method is used when the player uses a key to unlock something in the game.
    // It checks if the key exists in the inventory, removes it if found, and updates the UI.
    // If the key is not found, it logs a warning message.
    // It also logs the remaining number of keys after using one.
    // It takes a KeyType enum as a parameter to specify the type of key being used.
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

    //summary:
    // Updates the UI to reflect the current state of the keys in the inventory.
    // This method is called whenever the keys in the inventory change, such as when a key is added or used.
    // It updates the key display in the UIManager to show the current keys.
    public void UpdateKeys()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKeyDisplay(keys);
            Debug.Log("Schlüssel-UI aktualisiert.");
        }
    }

    // UseFood is commented out because it is not currently used in the game
    /*
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
    */

    //summary:
    // Gets the count of keys of a specific type in the inventory.
    // This method is used to count how many keys of a specific type the player has in their inventory.
    // It takes a KeyType enum as a parameter to specify the type of key being counted.
    // It returns the count of keys of that type.
    public int GetKeyCount(KeyType keyType)
    {
        return keys.Count(key => key == keyType);
    }

    //summary:
    // Gets the total number of keys in the inventory.
    // This method is used to get the total count of all keys in the player's inventory.
    // It returns the count of keys in the keys list.
    // This is useful for displaying the total number of keys in the UI or for debugging purposes.
    public int GetTotalKeyCount()
    {
        return keys.Count;
    }

    //summary:
    // This method retrieves the total score from the current save data managed by the SaveSystem.
    // It returns the score value, which is used to track the player's progress in the game.
    public int GetTotalScore()
    {
        return SaveSystem.Instance.currentSaveData.scoreData;
    }

    //summary:
    // This method is used for debugging purposes to log the current state of the inventory.
    // It logs the total number of keys, the types of keys, the total number of food items, and the total score.
    // It is useful for developers to see the contents of the inventory during development or testing.
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

    //summary:
    // This method loads the inventory data from the provided InventoryData object.
    // It clears the current inventory, loads keys and food items from the data, and updates the UI accordingly.
    // It is called when the game is loaded or when the inventory needs to be restored from saved data.
    // It also logs the loading process and the final state of the inventory for debugging purposes.
    // It expects the InventoryData object to contain arrays of collected keys and food items.
    public void LoadInventoryFromSaveData(InventoryData data)
    {
        Debug.Log("=== LADE INVENTAR ===");

        ClearAllInventories();

        // load keys
        LoadKeys(data.collectedKeys);

        // load food
        LoadFood(data.collectedFood);

        // Score laden und UI aktualisieren
        //LoadCurrentScore(SaveSystem.Instance.currentSaveData.scoreData);

        UpdateUIAndLog();

        UIManager.Instance.UpdateScore();
    }

    //summary:
    // This method clears all keys and food items from the inventory.
    // It is used to reset the inventory before loading new data, ensuring that no old items remain.
    // It logs a message to indicate that the inventory has been cleared.
    private void ClearAllInventories()
    {
        keys.Clear();
        food.Clear();
        Debug.Log("Inventar geleert vor dem Laden.");
    }

    //summary:
    // This method loads keys and food items from the provided arrays of collected key and food data.
    // It iterates through the arrays, checks if each item is collected, and adds them to the respective lists in the inventory.
    // It also updates the UI to reflect the loaded items and logs the loading process for debugging purposes.
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

        // IMPORTANT: update UI after loading
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKeyDisplay(keys);
            Debug.Log($"Schlüssel-UI aktualisiert mit {keys.Count} Schlüsseln");
        }
    }

    //summary:
    // This method loads food items from the provided array of collected food data.
    // It iterates through the array, checks if each food item is collected, and adds them to the food list in the inventory.
    // It also updates the UI to reflect the loaded food items and logs the loading process for debugging purposes.
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

    //summary:
    // This method updates the UI and logs the current state of the inventory after loading.
    // It logs the total number of keys, the types of keys, the total number of food items, and the total score.
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

        // final update of UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateKeyDisplay(keys);
            UIManager.Instance.UpdateFoodDisplay(food);
            UIManager.Instance.UpdateScore();
        }
    }

    //summary:
    // This class represents the data structure for a collectible item in the game.
    // It contains the type of the item, its name, and its point value.
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