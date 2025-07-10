using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer;
    public IInteractable interactable;
    private Camera playerCamera;
    public TextMeshProUGUI interactionText;

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
                UIManager.Instance.ShowInteractionPrompt(interactable.GetInteractionText(this));
                this.interactable = interactable;
                return;
            }
        }

            UIManager.Instance.HideInteractionPrompt();
            this.interactable = null; // Kein Interagierbares Objekt gefunden    
    }
}

public interface IInteractable
{
    void Interact(PlayerInteraction player);
    string GetInteractionText(PlayerInteraction player);
    bool CanInteract(PlayerInteraction player);
    IEnumerator onSaveGameLoaded();
    void Start();
}
