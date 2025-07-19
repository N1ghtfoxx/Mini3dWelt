using System.Collections.Generic;
using UnityEngine;

// summary:
// This script defines the structure for saving game data.
// It includes player data, treasure chest states, collected items, keys, food, score, and inventory.

//summary:
// This class holds all the data needed to save the game state.
[System.Serializable]
public class GameSaveData
{
    public PlayerData playerData;
    public TreasureChestData[] chestData;
    public CollectedItemData[] collectedInWorldItems;
    public CollectedKeyData[] collectedInWorldKeys;
    public CollectedFoodData[] collectedFoodInWorld;
    public int scoreData;
    public InventoryData inventoryData;
    public OpenedDoorData[] openedDoor;

}

// summary:
// This class holds the player's position and rotation data.
// It can be extended to include more player-related data such as level, experience, health, etc.
[System.Serializable]
public class PlayerData
{
    public float[] position = new float[3];
    public float[] rotation = new float[3];
 //   public int level;
 //   public int experience;
 //   public int health;
 //   public int maxHealth;
}

// summary:
// This class holds the total score data.
[System.Serializable]
public class TotalScoreData
{
    public int totalScore;
}

// summary:
// This class holds the data for keys collected in the game.
[System.Serializable]
public class CollectedKeyData
{
    public string keyName;
    public bool isCollected;
}

// summary:
// This class holds the data for opened doors in the game.
[System.Serializable]
public class OpenedDoorData
{
    public string doorName;
    public bool isOpened;
}

// summary:
// This class holds the data for treasure chests in the game.
[System.Serializable]
public class TreasureChestData
{
    public string chestId;
    public bool isOpened;
}

// summary:
// This class holds the data for items collected in the game world.
[System.Serializable]
public class CollectedItemData
{
    public string itemName;
    public bool isCollected;
}

// summary:
// This class holds the data for food items collected in the game world.
[System.Serializable]
public class CollectedFoodData
{
    public string foodName;
    public bool isCollected;
}

// summary:
// This class holds the inventory data, including collected keys, items, opened doors, and food.
[System.Serializable]
public class InventoryData
{
    public CollectedKeyData[] collectedKeys;
    public CollectedItemData[] collectedItems;
    public OpenedDoorData[] openedDoors;
    public CollectedFoodData[] collectedFood;

}