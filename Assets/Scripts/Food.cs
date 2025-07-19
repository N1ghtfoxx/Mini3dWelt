using System.Collections;
using System.Linq;
using UnityEngine;

// summary:
// This script handles the interaction with food items in the game.
// It allows players to collect food, updates the inventory, and manages the state of food items based on save data.
public class Food : MonoBehaviour, IInteractable
{
    [Header("Food Settings")]
    public FoodType foodType;

    // summary:
    // Interact method is called when the player interacts with the food item.
    // It checks if the player can interact, adds the food to the inventory, shows a message,
    // marks the food as collected in the save system, and destroys the food object.
    public void Interact(PlayerInteraction player)
    {
        if (CanInteract(player))
        {
            // Zum Inventar hinzufügen
            player.GetComponent<InventoryManager>().AddFood();

            UIManager.Instance.ShowMessage($"{foodType.ToString()} eingesammelt!");

            SaveSystem.Instance.MarkFoodAsCollected(gameObject.name);

            Destroy(gameObject);

            // UI Update
        }
    }

    // summary:
    // GetInteractionText returns the text displayed when the player can interact with the food item.
    // It includes the food type and the action to pick it up.
    public string GetInteractionText(PlayerInteraction player)
    {
        return $"[E] {foodType.ToString()} aufheben";
    }

    // summary:
    // CanInteract checks if the player can interact with the food item.
    // In this case, it always returns true, allowing the player to pick up the food.
    public bool CanInteract(PlayerInteraction player)
    {
        return true; // Schinken kann immer aufgehoben werden
    }

    // summary:
    // onSaveGameLoaded is a coroutine that waits until the save system is ready and checks if the food item has been collected.
    // If the food has been collected, it destroys the game object.
    public IEnumerator onSaveGameLoaded()
    {
        while (SaveSystem.Instance == null || SaveSystem.Instance.currentSaveData.playerData.position[0] == 0f)
            yield return null;
        //gameLoaded = true;
        var foodData = SaveSystem.Instance.currentSaveData.collectedFoodInWorld.FirstOrDefault(f => f.foodName == gameObject.name);
        if (foodData != null)
            {
            if (foodData.isCollected)
            {
                // Wenn das Essen bereits gesammelt wurde, wird das Objekt zerstört
                Destroy(gameObject);
            }
        }
    }

    // summary:
    // Start is called when the script instance is being loaded.
    // It starts the coroutine to check the save data and manage the food item state.
    public void Start()
    {
        StartCoroutine(onSaveGameLoaded());
    }
}

// summary:
// FoodType is an enumeration that defines the types of food available in the game.
// Currently, it includes only "Schinken" (ham).
public enum FoodType
{
    Schinken, // Schinken
}