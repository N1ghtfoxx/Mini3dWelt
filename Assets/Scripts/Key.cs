using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using System.Collections;

// summary:
// This script defines a Key class that implements the IInteractable interface.
public class Key : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    public KeyType keyType;

    // audio could be used for pickup sound effects
    //    public AudioClip pickupSound;

    // summary:
    // Interact method is called when the player interacts with the key.
    // It adds the key to the player's inventory, updates the UI, and destroys the key object.
    public void Interact(PlayerInteraction player)
    {
        if (CanInteract(player))
        {
            // Zum Inventar hinzufügen
            player.GetComponent<InventoryManager>().AddKey(keyType);

            // UI Update
            UIManager.Instance.ShowMessage($"{keyType.ToString()} aufgehoben!");

            SaveSystem.Instance.MarkKeyAsCollected(gameObject.name);

            // Effekte
            // audio could be played here
            // AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Objekt wird aus der Szene entfernt
            Destroy(gameObject);

        }
    }

    // summary:
    // GetInteractionText method returns the text displayed when the player can interact with the key.
    public string GetInteractionText(PlayerInteraction player)
    {
        return $"[E] {keyType.ToString()} aufheben";
    }

    // summary:
    // CanInteract method checks if the player can interact with the key.
    public bool CanInteract(PlayerInteraction player)
    {
        return true; // Schlüssel können immer aufgehoben werden
    }

    // summary:
    // onSaveGameLoaded method is a coroutine that waits until the save system is ready and checks if the key has already been collected.
    // If the key has been collected, it destroys the key object.
    public IEnumerator onSaveGameLoaded()
    {
        while (SaveSystem.Instance == null || SaveSystem.Instance.currentSaveData.playerData.position[0] == 0f)
            yield return null;
        //gameLoaded = true;
        var keyData = SaveSystem.Instance.currentSaveData.collectedInWorldKeys.FirstOrDefault(k => k.keyName == gameObject.name);

        if (keyData != null)
            {
            if (keyData.isCollected)
            {
                // Wenn der Schlüssel bereits gesammelt wurde, wird das Objekt zerstört
                Destroy(gameObject);
            }
        }
    }

    // summary:
    // Start method is called when the script instance is being loaded.
    // It starts the coroutine to check if the key has already been collected.
    public void Start()
    {
        StartCoroutine(onSaveGameLoaded());
    }
}

// summary:
// KeyType enum defines the different types of keys available in the game.
public enum KeyType
{
    BronzeSchlüssel,       // Für...
    SilberSchlüssel,       // Für... 
    GoldSchlüssel,         // Für ...
    MasterSchlüssel        // Für Ausgang
}