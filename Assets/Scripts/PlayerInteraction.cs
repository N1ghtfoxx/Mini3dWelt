using UnityEngine;
using System;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public IInteractable interactable;
    private Camera playerCamera;

    private void Awake()
    {
        playerCamera = Camera.main; // Setze die Kamera auf die Hauptkamera
    }

    void Update()
    {
        CheckForInteractable();

        if (Input.GetKeyDown(KeyCode.E) && interactable != null)
        {
            interactable.Interact(this);
        }
    }


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
                ShowInteractionPrompt(interactable.GetInteractionText());
                this.interactable = interactable;
            }
        }
        else
        {
            HideInteractionPrompt();
            this.interactable = null; // Kein Interagierbares Objekt gefunden
        }
    }
    // Füge diese Methoden in die Klasse PlayerInteraction ein

    void ShowInteractionPrompt(string text)
    {
        // TODO: Hier UI-Code einfügen, um den Interaktions-Prompt mit dem Text anzuzeigen
        Debug.Log("Interaktion möglich: " + text);
    }

    void HideInteractionPrompt()
    {
        // TODO: Hier UI-Code einfügen, um den Interaktions-Prompt auszublenden
        Debug.Log("Interaktions-Prompt ausgeblendet");
    }
}

public interface IInteractable
{
    void Interact(PlayerInteraction player);
    string GetInteractionText();
    bool CanInteract(PlayerInteraction player);
}
