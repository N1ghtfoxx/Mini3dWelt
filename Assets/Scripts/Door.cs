using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public KeyType requiredKey;
    public string doorName = "Kerkert�r";
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
            // Schl�ssel verbrauchen (optional)
            // inventory.UseKey(requiredKey);

            isLocked = false;
            OpenDoor();
            UIManager.Instance.ShowMessage($"{doorName} ge�ffnet!");
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
        if (isLocked) return $"{requiredKey} (ben�tigt)";
        return $"[E] {doorName} �ffnen";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpen;
    }
}
