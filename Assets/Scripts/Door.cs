using System.Collections;
using System.Linq;
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
    //private bool gameLoaded = false;

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
            /*            AudioSource.PlayClipAtPoint(lockedSound, transform.position);*/
        }
    }

    void OpenDoor()
    {
        if (!isOpen)
        {
            hingeTransform.localRotation = Quaternion.Euler(0, 90, 0); // Beispiel: T�r um 90 Grad �ffnen
            /*         
                       AudioSource.PlayClipAtPoint(openSound, transform.position);
            */
            isOpen = true;
            Debug.Log($"{doorName} wurde ge�ffnet.");

            SaveSystem.Instance.MarkDoorAsOpened(gameObject.name, requiredKey);
        }
    }

    public string GetInteractionText(PlayerInteraction player)
    {
        if (isOpen) return "";
        if (isLocked && !player.GetComponent<InventoryManager>().HasKey(requiredKey)) return $"{requiredKey} (zum �ffnen ben�tigt)";
        return $"[E] {doorName} �ffnen";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpen;
    }

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

    public void Start()
    {
        StartCoroutine(onSaveGameLoaded());
    }
}
