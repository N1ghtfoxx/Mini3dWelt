using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// summary:
// This script handles player interactions with objects in the game world.
// It allows the player to interact with objects within a specified range and displays interaction prompts.
// It uses a raycast to detect interactable objects and provides feedback through the UI.
// It also defines an interface for interactable objects, allowing for flexible interaction behavior.
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public IInteractable interactable;
    private Camera playerCamera;
    public TextMeshProUGUI interactionText;

    // summary:
    // This method is called when the script instance is being loaded
    // It initializes the playerCamera variable to the main camera in the scene.
    private void Awake()
    {
        playerCamera = Camera.main; // Setze die Kamera auf die Hauptkamera
    }

    // summary:
    // This method is called once per frame
    // It checks for interactable objects and handles player input for interaction.
    // If the player presses the interaction key (E), it calls the Interact method on the interactable object.
    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && interactable != null)
        {
            interactable.Interact(this);
        }
    }

    // summary:
    // This method performs a raycast from the center of the screen to check for interactable objects
    // If an interactable object is found, it displays the interaction prompt in the UI.
    // If no interactable object is found, it hides the interaction prompt.
    // It also updates the interactable variable to the found interactable object or null if none is found.
    void CheckForInteractable()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract(this))
            {
                // UI-Prompt anzeigen
                UIManager.Instance.ShowInteractionPrompt(interactable.GetInteractionText(this));
                this.interactable = interactable;
                return;
            }
        }

            UIManager.Instance.HideInteractionPrompt();
            this.interactable = null; // Kein Interagierbares Objekt gefunden    
    }
}

// summary:
// This interface defines the methods that any interactable object must implement
// It includes methods for interaction, getting interaction text, checking if interaction is possible,
// handling save game loading, and starting the interactable object.
// It allows for flexible interaction behavior across different types of interactable objects.
public interface IInteractable
{
    void Interact(PlayerInteraction player);
    string GetInteractionText(PlayerInteraction player);
    bool CanInteract(PlayerInteraction player);
    IEnumerator onSaveGameLoaded();
    void Start();
}