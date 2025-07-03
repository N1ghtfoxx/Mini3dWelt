using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public KeyType requiredKey;
    public string doorName = "Kerkert�r";
    public bool isLocked = true;
    public bool consumeKey = false;
    public Transform hingeTransform; // Optional: Transform f�r T�r-Animation oder Positionierung

    /*    [Header("Animation")]
        public Animator doorAnimator;
        public AudioClip openSound;
        public AudioClip lockedSound;*/

    private bool isOpen = false;

    public void Interact(PlayerInteraction player)
    {
        InventoryManager inventory = player.GetComponent<InventoryManager>();

        if (!isLocked)
        {
            OpenDoor();
            UIManager.Instance.ShowMessage($"{doorName} ge�ffnet!");
        }
        else if (inventory != null && inventory.HasKey(requiredKey))
        {
           if(consumeKey)
            {
                inventory.UseKey(requiredKey);
                UIManager.Instance.ShowMessage($"{doorName} entsperrt! ({requiredKey} verbraucht)");
            }
            else
            {
                UIManager.Instance.ShowMessage($"{doorName} entsperrt!");
            }

           isLocked = false;
           OpenDoor();
        }
        else
        {
            UIManager.Instance.ShowMessage($"{doorName} ist verschlossen! Du brauchst einen ({requiredKey}!)");
            /*            AudioSource.PlayClipAtPoint(lockedSound, transform.position);*/
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            hingeTransform.localRotation = Quaternion.Euler(0, 90, 0); // Beispiel: T�r um 90 Grad �ffnen
            /*         doorAnimator.SetBool("isOpen", true);
                       AudioSource.PlayClipAtPoint(openSound, transform.position);
            */
            isOpen = true;
            Debug.Log($"{doorName} wurde ge�ffnet.");
        }
    }

    public string GetInteractionText(PlayerInteraction player)
    {
        if (isOpen) return "";
        if (isLocked && !player.GetComponent<InventoryManager>().HasKey(requiredKey)) return $"{requiredKey} (ben�tigt)";
        return $"[E] {doorName} �ffnen";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpen;
    }
}
