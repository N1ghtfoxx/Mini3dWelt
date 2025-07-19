using UnityEngine;

// summary:
// This script defines a Collectible class that represents items in the game world.
// It includes properties for item type, name, point value, and icon.
// It also provides methods for interaction, displaying interaction text, and checking if interaction is possible.
public class Collectible : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemType itemType;
    public string itemName;
    public int pointValue = 10;
    public Sprite itemIcon;

    // summary:
    // This method is called when the player interacts with the collectible item.
    // It checks if the player can interact with the item, adds it to the player's inventory,
    // updates the UI with a message, and destroys the collectible object.
    public void Interact(PlayerInteraction player)
    {
        if (CanInteract(player))
        {
            // Zum Inventar hinzufügen
            InventoryManager inventory = player.GetComponent<InventoryManager>();

            // UI Updates
            //UIManager.Instance.AddScore(pointValue);
            UIManager.Instance.ShowMessage($"{itemName} gefunden! +{pointValue} Punkte");

            Destroy(gameObject);
        }
    }

    // summary:
    // This method returns the text that will be displayed when the player can interact with the item.
    public string GetInteractionText()
    {
        return $"[E] {itemName} aufheben";
    }

    // summary:
    // This method checks if the player can interact with the collectible item.
    public bool CanInteract(PlayerInteraction player)
    {
        return true;
    }
}

// summary:
// This enum defines the different types of collectible items in the game.
public enum ItemType
{
    Gem,      // Edelsteine
    Weapon,   // Waffen
    Food,     // Nahrung
}