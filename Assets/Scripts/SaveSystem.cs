using System.Collections.Generic;
using System.IO;
using System.Linq; // Am Anfang der Datei einfügen
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
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeSaveData()
    {
        currentSaveData = new GameSaveData
        {
            playerData = new PlayerData(),
            chestData = new TreasureChestData[0], // Initialize as an empty array instead of a List
            collectedInWorldItems = new CollectedItemData[0],
            collectedInWorldKeys = new CollectedKeyData[0],
            collectedFoodInWorld = new CollectedFoodData[0],
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

            // Aktuelle Spielzeit aktualisieren

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

                // Spieler-Position und -Rotation laden
                LoadPlayerData();

                Debug.Log("Spiel geladen: " + savePath);
                return true;
            }
            else
            {
                Debug.Log("Keine Speicherdatei gefunden.");
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

    void LoadInventoryData()
    {
        ///TODO: Implementieren Sie die Logik zum Laden der Inventardaten
        // Hier können Sie die Logik zum Laden der Inventardaten implementieren
        // z.B. UI-Elemente aktualisieren, um die gesammelten Gegenstände anzuzeigen
    }

    // Schlüssel-Funktionen
    public void MarkKeyAsCollected(string keyName)
    {
        var existingKey = currentSaveData.collectedInWorldKeys.FirstOrDefault(k => k.keyName == keyName);
        if (existingKey == null)
        {
            currentSaveData.collectedInWorldKeys = currentSaveData.collectedInWorldKeys.Append(new CollectedKeyData
            {
                keyName = keyName,
                isCollected = true
            }).ToArray();
        }
        else
        {
            existingKey.isCollected = true;
        }
        // Zum Inventar hinzufügen
        if (!currentSaveData.inventoryData.collectedKeys.Contains(existingKey))
        {
            currentSaveData.inventoryData.collectedKeys = currentSaveData.inventoryData.collectedKeys.Append(existingKey).ToArray();
        }

        SaveGame();
    }

    // Tür-Funktionen
    public void MarkDoorAsOpened(string doorName)
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
    public void MarkItemAsCollected(string itemName)
    {
        var existingItem = currentSaveData.collectedInWorldItems.FirstOrDefault(i => i.itemName == itemName);
        if (existingItem == null)
        {
            currentSaveData.collectedInWorldItems = currentSaveData.collectedInWorldItems.Append(existingItem = new CollectedItemData
            {
                itemName = itemName,
                isCollected = true
            }).ToArray();
        }
        else
        {
            existingItem.isCollected = true;
        }

        // Zum Inventar hinzufügen
        if (!currentSaveData.inventoryData.collectedItems.Contains(existingItem))
        {
           currentSaveData.inventoryData.collectedItems = currentSaveData.inventoryData.collectedItems.Append(existingItem).ToArray();
        }
        SaveGame();
    }

    // Food-Funktionen
    public void MarkFoodAsCollected(string foodName)
    {
        var existingFood = currentSaveData.collectedFoodInWorld.FirstOrDefault(f => f.foodName == foodName);
        if (existingFood == null)
        {
            currentSaveData.collectedFoodInWorld = currentSaveData.collectedFoodInWorld.Append(new CollectedFoodData
            {
                foodName = foodName,
                isCollected = true
            }).ToArray();
            existingFood = currentSaveData.collectedFoodInWorld.Last(); // Referenz auf das neu erstellte Element
        }
        else
        {
            existingFood.isCollected = true;
        }
        // Zum Inventar hinzufügen
        if (!currentSaveData.inventoryData.collectedFood.Contains(existingFood))
        {
            currentSaveData.inventoryData.collectedFood = currentSaveData.inventoryData.collectedFood.Append(existingFood).ToArray();
        }
        SaveGame();
    }

    public void MarkItemAsUsed(string itemName)
    {
        var existingItem = currentSaveData.inventoryData.collectedItems.FirstOrDefault(i => i.itemName == itemName);
        if (existingItem == null)
        {
            Debug.LogWarning("Item nicht im Inventar gefunden: " + itemName);
            return;
        }
        existingItem.isCollected = true;
        SaveGame();
    }

/*
    // Spieler-Statistiken
    public void UpdatePlayerStats(int level, int experience, int health, int maxHealth)
    {
        currentSaveData.playerData.level = level;
        currentSaveData.playerData.experience = experience;
        currentSaveData.playerData.health = health;
        currentSaveData.playerData.maxHealth = maxHealth;
    }
*/

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
}