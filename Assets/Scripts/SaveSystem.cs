using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string savePath;
    public GameSaveData currentSaveData;

    public System.Action OnGameSaved;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            InitializeSaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadGame();
        // WICHTIG: Verzögere die UI-Aktualisierung um sicherzustellen, dass alle Manager bereit sind
        StartCoroutine(UpdateUIAfterLoad());
    }

    private System.Collections.IEnumerator UpdateUIAfterLoad()
    {
        // Warte einen Frame, damit alle Manager initialisiert sind
        yield return null;

        // Stelle sicher, dass die UI korrekt aktualisiert wird
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.UpdateKeys();
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScore();
        }
    }

    void InitializeSaveData()
    {
        currentSaveData = new GameSaveData
        {
            playerData = new PlayerData(),
            chestData = new TreasureChestData[0],
            collectedInWorldItems = new CollectedItemData[0],
            collectedInWorldKeys = new CollectedKeyData[0],
            collectedFoodInWorld = new CollectedFoodData[0],
            scoreData = 0,
            inventoryData = new InventoryData(),
            openedDoor = new OpenedDoorData[0],
        };
        currentSaveData.inventoryData.collectedItems = new CollectedItemData[0];
        currentSaveData.inventoryData.collectedKeys = new CollectedKeyData[0];
        currentSaveData.inventoryData.collectedFood = new CollectedFoodData[0];
    }

    public void SaveGame()
    {
        try
        {
            // Spieler-Position und -Rotation speichern
            SavePlayerData();

            // In JSON konvertieren und speichern
            string json = JsonUtility.ToJson(currentSaveData, true);
            File.WriteAllText(savePath, json);

            Debug.Log("Spiel gespeichert: " + savePath);
            OnGameSaved?.Invoke();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Speichern: " + e.Message);
        }
    }

    public bool LoadGame()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                currentSaveData = JsonUtility.FromJson<GameSaveData>(json);

                Debug.Log("=== LADE SPIEL ===");
                Debug.Log($"Geladener Score: {currentSaveData.scoreData}");
                Debug.Log($"Geladene Schlüssel: {currentSaveData.inventoryData.collectedKeys?.Length ?? 0}");
                Debug.Log($"Geladenes Essen: {currentSaveData.inventoryData.collectedFood?.Length ?? 0}");

                // Spieler-Position und -Rotation laden
                LoadPlayerData();

                // WICHTIG: Erst nach dem Laden der Daten das Inventar aktualisieren
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.LoadInventoryFromSaveData(currentSaveData.inventoryData);
                }

                // UI aktualisieren
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.UpdateScore();
                }

                if (currentSaveData.inventoryData.collectedFood.Length > 0)
                {
                    var foodList = currentSaveData.inventoryData.collectedFood
                        .Where(f => f.isCollected)
                        .Select(f => f.foodName)
                        .ToList();
                    UIManager.Instance.UpdateFoodDisplay(foodList);
                }

                Debug.Log("Spiel erfolgreich geladen: " + savePath);
                return true;
            }
            else
            {
                Debug.Log("Keine Speicherdatei gefunden - Neues Spiel wird gestartet.");
                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Fehler beim Laden: " + e.Message);
            return false;
        }
    }

    void SavePlayerData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            Vector3 rot = player.transform.eulerAngles;

            currentSaveData.playerData.position = new float[] { pos.x, pos.y, pos.z };
            currentSaveData.playerData.rotation = new float[] { rot.x, rot.y, rot.z };
        }
    }

    void LoadPlayerData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && currentSaveData.playerData != null)
        {
            Vector3 pos = new Vector3(
                currentSaveData.playerData.position[0],
                currentSaveData.playerData.position[1],
                currentSaveData.playerData.position[2]
            );
            Vector3 rot = new Vector3(
                currentSaveData.playerData.rotation[0],
                currentSaveData.playerData.rotation[1],
                currentSaveData.playerData.rotation[2]
            );

            player.transform.position = pos;
            player.transform.eulerAngles = rot;
        }
    }

    // Schlüssel-Funktionen
    public void MarkKeyAsCollected(string keyName)
    {
        var existingKey = currentSaveData.collectedInWorldKeys.FirstOrDefault(k => k.keyName == keyName);
        if (existingKey == null)
        {
            existingKey = new CollectedKeyData
            {
                keyName = keyName,
                isCollected = true
            };
            currentSaveData.collectedInWorldKeys = currentSaveData.collectedInWorldKeys.Append(existingKey).ToArray();
        }
        else
        {
            existingKey.isCollected = true;
        }

        var itemKey = new CollectedKeyData
        {
            keyName = keyName,
            isCollected = true
        };
        // WICHTIG: Zum Inventar hinzufügen (verwende Any() statt Contains() für bessere Kompatibilität)
        if (!currentSaveData.inventoryData.collectedKeys.Any(k => k.keyName == keyName && k.isCollected))
        {
            currentSaveData.inventoryData.collectedKeys = currentSaveData.inventoryData.collectedKeys.Append(itemKey).ToArray();
            Debug.Log($"Schlüssel '{keyName}' zum Inventar hinzugefügt. Inventar-Schlüssel: {currentSaveData.inventoryData.collectedKeys.Length}");
        }

        SaveGame();
    }

    // Tür-Funktionen
    public void MarkDoorAsOpened(string doorName, KeyType keyType)
    {
        var existingDoor = currentSaveData.openedDoor.FirstOrDefault(d => d.doorName == doorName);
        if (existingDoor == null)
        {
            currentSaveData.openedDoor = currentSaveData.openedDoor.Append(new OpenedDoorData
            {
                doorName = doorName,
                isOpened = true,
            }).ToArray();
        }
        else
        {
            existingDoor.isOpened = true;
        }

        var keyName = keyType.ToString();
        var existingKey = currentSaveData.inventoryData.collectedKeys.FirstOrDefault(k => k.keyName == keyName);
        if (currentSaveData.inventoryData.collectedKeys.Any(k => k.keyName == keyName && k.isCollected))
        {
            existingKey.isCollected = false;
        }

        SaveGame();
    }

    // Truhen-Funktionen
    public void MarkChestAsOpened(string chestName)
    {
        var existingChest = currentSaveData.chestData.FirstOrDefault(c => c.chestId == chestName);
        if (existingChest == null)
        {
            currentSaveData.chestData = currentSaveData.chestData.Append(new TreasureChestData
            {
                chestId = chestName,
                isOpened = true,
            }).ToArray();
        }
        else
        {
            existingChest.isOpened = true;
        }
        SaveGame();
    }

    // Gegenstände-Funktionen
    //public void MarkItemAsCollected(string itemName)
    //{
    //    var existingItem = currentSaveData.collectedInWorldItems.FirstOrDefault(i => i.itemName == itemName);
    //    if (existingItem == null)
    //    {
    //        existingItem = new CollectedItemData
    //        {
    //            itemName = itemName,
    //            isCollected = true
    //        };
    //        currentSaveData.collectedInWorldItems = currentSaveData.collectedInWorldItems.Append(existingItem).ToArray();
    //    }
    //    else
    //    {
    //        existingItem.isCollected = true;
    //    }

    //    // WICHTIG: Zum Inventar hinzufügen (verwende Any() statt Contains() für bessere Kompatibilität)
    //    if (!currentSaveData.inventoryData.collectedItems.Any(i => i.itemName == itemName && i.isCollected))
    //    {
    //        currentSaveData.inventoryData.collectedItems = currentSaveData.inventoryData.collectedItems.Append(existingItem).ToArray();
    //        Debug.Log($"Item '{itemName}' zum Inventar hinzugefügt. Inventar-Items: {currentSaveData.inventoryData.collectedItems.Length}");
    //    }

    //    SaveGame();
    //}

    // Food-Funktionen
    public void MarkFoodAsCollected(string foodName)
    {
        var existingFood = currentSaveData.collectedFoodInWorld.FirstOrDefault(f => f.foodName == foodName);
        if (existingFood == null)
        {
            existingFood = new CollectedFoodData
            {
                foodName = foodName,
                isCollected = true
            };
            currentSaveData.collectedFoodInWorld = currentSaveData.collectedFoodInWorld.Append(existingFood).ToArray();
        }
        else
        {
            existingFood.isCollected = true;
        }
        var itemFood = new CollectedFoodData
        {
            foodName = foodName,
            isCollected = true
        };
        // WICHTIG: Zum Inventar hinzufügen (verwende Any() statt Contains() für bessere Kompatibilität)
        if (!currentSaveData.inventoryData.collectedFood.Any(f => f.foodName == foodName && f.isCollected))
        {
            currentSaveData.inventoryData.collectedFood = currentSaveData.inventoryData.collectedFood.Append(itemFood).ToArray();
            Debug.Log($"Essen '{foodName}' zum Inventar hinzugefügt. Inventar-Essen: {currentSaveData.inventoryData.collectedFood.Length}");
        }

        SaveGame();
    }

    public void AddScore(int score)
    {         
        currentSaveData.scoreData += score;
        UIManager.Instance.UpdateScore();  // UI aktualisieren
        SaveGame();
    }
    //public void MarkItemAsUsed(string itemName)
    //{
    //    var existingItem = currentSaveData.inventoryData.collectedItems.FirstOrDefault(i => i.itemName == itemName);
    //    if (existingItem == null)
    //    {
    //        Debug.LogWarning("Item nicht im Inventar gefunden: " + itemName);
    //        return;
    //    }
    //    existingItem.isCollected = true;
    //    SaveGame();
    //}

    // Getter für aktuelle Daten
    public GameSaveData GetCurrentSaveData()
    {
        return currentSaveData;
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Speicherdatei gelöscht.");
            InitializeSaveData();
        }
    }

    public bool SaveFileExists()
    {
        return File.Exists(savePath);
    }

    // Debug-Methode für Speicherdaten
    public void DebugSaveData()
    {
        Debug.Log("=== SPEICHERDATEN DEBUG ===");
        Debug.Log($"Score: {currentSaveData.scoreData}");
        Debug.Log($"Schlüssel in World: {currentSaveData.collectedInWorldKeys?.Length ?? 0}");
        Debug.Log($"Schlüssel in Inventar: {currentSaveData.inventoryData.collectedKeys?.Length ?? 0}");
        Debug.Log($"Essen in World: {currentSaveData.collectedFoodInWorld?.Length ?? 0}");
        Debug.Log($"Essen in Inventar: {currentSaveData.inventoryData.collectedFood?.Length ?? 0}");
        Debug.Log($"Items in World: {currentSaveData.collectedInWorldItems?.Length ?? 0}");
        Debug.Log($"Items in Inventar: {currentSaveData.inventoryData.collectedItems?.Length ?? 0}");
    }
}