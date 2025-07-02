using System;
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
                UIManager.Instance.ShowInteractionPrompt(interactable.GetInteractionText());
                this.interactable = interactable;
            }
        }
        else
        {
            UIManager.Instance.HideInteractionPrompt();
            this.interactable = null; // Kein Interagierbares Objekt gefunden
        }
    }

    //void ShowInteractionPrompt(string text)
    //{
    //    if (interactionText != null)
    //    {
    //        interactionText.text = text;
    //        interactionText.gameObject.SetActive(true);
    //    }
    //}

    //void HideInteractionPrompt()
    //{
    //    if (interactionText != null)
    //    {
    //        interactionText.gameObject.SetActive(false);
    //    }
    //}
}

public interface IInteractable
{
    void Interact(PlayerInteraction player);
    string GetInteractionText();
    bool CanInteract(PlayerInteraction player);
}
