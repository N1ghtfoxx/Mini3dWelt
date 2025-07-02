using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public KeyType requiredKey;
    public string doorName = "Kerkertür";
    public bool isLocked = true;

/*    [Header("Animation")]
    public Animator doorAnimator;
    public AudioClip openSound;
    public AudioClip lockedSound;*/

    private bool isOpen = false;

    public void Interact(PlayerInteraction player)
    {
        InventoryManager inventory = player.GetComponent<InventoryManager>();

        if (!isLocked || !isOpen)
        {
            OpenDoor();
        }
        else if (inventory.HasKey(requiredKey))
        {
            // Schlüssel verbrauchen (optional)
            // inventory.UseKey(requiredKey);

            isLocked = false;
            OpenDoor();
            UIManager.Instance.ShowMessage($"{doorName} geöffnet!");
        }
        else
        {
//          AudioSource.PlayClipAtPoint(lockedSound, transform.position);
            UIManager.Instance.ShowMessage($"Du brauchst einen {requiredKey}!");
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
 /*         doorAnimator.SetBool("isOpen", true);
            AudioSource.PlayClipAtPoint(openSound, transform.position);
 */
            isOpen = true;
        }
    }

    public string GetInteractionText()
    {
        if (isOpen) return "";
        if (isLocked) return $"{requiredKey} (benötigt)";
        return $"[E] {doorName} öffnen";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpen;
    }
}
