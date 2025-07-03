using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public KeyType requiredKey;
    public string doorName = "Kerkertür";
    public bool isLocked = true;
    public bool consumeKey = false;
    public Transform hingeTransform; // Optional: Transform für Tür-Animation oder Positionierung

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
            UIManager.Instance.ShowMessage($"{doorName} geöffnet!");
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
            hingeTransform.localRotation = Quaternion.Euler(0, 90, 0); // Beispiel: Tür um 90 Grad öffnen
            /*         doorAnimator.SetBool("isOpen", true);
                       AudioSource.PlayClipAtPoint(openSound, transform.position);
            */
            isOpen = true;
            Debug.Log($"{doorName} wurde geöffnet.");
        }
    }

    public string GetInteractionText(PlayerInteraction player)
    {
        if (isOpen) return "";
        if (isLocked && !player.GetComponent<InventoryManager>().HasKey(requiredKey)) return $"{requiredKey} (benötigt)";
        return $"[E] {doorName} öffnen";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpen;
    }
}
