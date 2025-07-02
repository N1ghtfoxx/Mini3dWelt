using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemType itemType;
    public string itemName;
    public int pointValue = 10;
    public Sprite itemIcon;

    [Header("Effects")]
    public ParticleSystem pickupEffect;
    public AudioClip pickupSound;

    public void Interact(PlayerInteraction player)
    {
        if (CanInteract(player))
        {
            // Zum Inventar hinzufügen
            InventoryManager inventory = player.GetComponent<InventoryManager>();
            inventory.AddItem(itemType, itemName, pointValue);

/*            // Effekte abspielen
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);*/

            // UI Updates
            UIManager.Instance.AddScore(pointValue);
            UIManager.Instance.ShowMessage($"{itemName} gefunden! +{pointValue} Punkte");

            Destroy(gameObject);
        }
    }

    public string GetInteractionText()
    {
        return $"[E] {itemName} aufheben";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return true;
    }
}

public enum ItemType
{
    Gem,      // Edelsteine
    Weapon,   // Waffen
    Treasure, // Schätze
    Artifact  // Besondere Gegenstände
}

