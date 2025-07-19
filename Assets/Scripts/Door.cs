using System.Collections;
using System.Linq;
using UnityEngine;

// summary:
// This script handles the interaction with doors in the game.
// It allows players to open doors if they have the required key, updates the door state based on save data,
// and provides interaction text for the player.
public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public KeyType requiredKey;
    public string doorName = "Kerkertür";
    public bool isLocked = true;
    public bool consumeKey = false;
    public Transform hingeTransform; // Optional: Transform für Tür-Animation oder Positionierung

    // This section is commented out, but it can be used for sound effects when the door is opened or locked.
    /*    [Header("Sound")]
        public AudioClip openSound;
        public AudioClip lockedSound;
    */

    private bool isOpen = false;

    // summary:
    // Interact method is called when the player interacts with the door.
    // It checks if the player has the required key, unlocks the door, and opens it if possible.
    public void Interact(PlayerInteraction player)
    {
        InventoryManager inventory = player.GetComponent<InventoryManager>();

        if (inventory != null && inventory.HasKey(requiredKey))
        {
          
            inventory.UseKey(requiredKey);
            UIManager.Instance.ShowMessage($"{doorName} entsperrt! ({requiredKey} verbraucht)");                     
            isLocked = false;
            OpenDoor();
        }
        else
        {
            UIManager.Instance.ShowMessage($"{doorName} ist verschlossen! Du brauchst einen ({requiredKey}!)");
            /* 
            AudioSource.PlayClipAtPoint(lockedSound, transform.position); 
            */
        }
    }

    // summary:
    // OpenDoor method opens the door if it is not already open.
    // It sets the hinge transform to a specific rotation and marks the door as opened in the save system.
    void OpenDoor()
    {
        if (!isOpen)
        {
            hingeTransform.localRotation = Quaternion.Euler(0, 90, 0); // Beispiel: Tür um 90 Grad öffnen
            /*         
            AudioSource.PlayClipAtPoint(openSound, transform.position);
            */
            isOpen = true;
            Debug.Log($"{doorName} wurde geöffnet.");

            SaveSystem.Instance.MarkDoorAsOpened(gameObject.name, requiredKey);
        }
    }

    // summary:
    // GetInteractionText returns the text displayed when the player can interact with the door.
    public string GetInteractionText(PlayerInteraction player)
    {
        if (isOpen) return "";
        if (isLocked && !player.GetComponent<InventoryManager>().HasKey(requiredKey)) return $"{requiredKey} (zum Öffnen benötigt)";
        return $"[E] {doorName} öffnen";
    }

    // summary:
    // CanInteract checks if the player can interact with the door.
    // It returns true if the door is not open, allowing interaction.
    // If the door is open, interaction is not allowed.
    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpen;
    }

    // summary:
    // onSaveGameLoaded is a coroutine that waits until the save system is ready and checks if the door has been opened.
    // If the door has been opened, it sets the door state accordingly.
    // If the door is open, it sets the hinge transform to the open position.
    // If the door is locked, it remains in the closed position.
    public IEnumerator onSaveGameLoaded()
    {
        while(SaveSystem.Instance == null || SaveSystem.Instance.currentSaveData.playerData.position[0] == 0f)
            yield return null;
        //gameLoaded = true;
        var doorData = SaveSystem.Instance.currentSaveData.openedDoor.FirstOrDefault(d => d.doorName == gameObject.name);
        if (doorData != null)
        {
            isOpen = doorData.isOpened;
            if (isOpen)
            {
                hingeTransform.localRotation = Quaternion.Euler(0, 90, 0);
            }
        }   
    }

    // summary:
    // Start method is called when the script instance is being loaded.
    // It starts the coroutine to check the save game data and update the door state.
    public void Start()
    {
        StartCoroutine(onSaveGameLoaded());
    }
}