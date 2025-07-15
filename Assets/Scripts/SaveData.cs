using System.Collections.Generic;
using UnityEngine;

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

[System.Serializable]
public class TotalScoreData
{
    public int totalScore;
}

[System.Serializable]
public class CollectedKeyData
{
    public string keyName;
    public bool isCollected;
}


[System.Serializable]
public class OpenedDoorData
{
    public string doorName;
    public bool isOpened;
}

[System.Serializable]
public class TreasureChestData
{
    public string chestId;
    public bool isOpened;
}

[System.Serializable]
public class CollectedItemData
{
    public string itemName;
    public bool isCollected;
}

[System.Serializable]
public class CollectedFoodData
{
    public string foodName;
    public bool isCollected;
}

[System.Serializable]
public class InventoryData
{
    public CollectedKeyData[] collectedKeys;
    public CollectedItemData[] collectedItems;
    public OpenedDoorData[] openedDoors;
    public CollectedFoodData[] collectedFood;

}
