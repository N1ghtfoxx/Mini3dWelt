using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    public KeyType keyType;
    public AudioClip pickupSound;

    public void Interact(PlayerInteraction player)
    {
        if (CanInteract(player))
        {
            // Zum Inventar hinzufügen
            player.GetComponent<InventoryManager>().AddKey(keyType);

            // Effekte
//          AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);

            // UI Update
            UIManager.Instance.ShowMessage($"{keyType.ToString()} aufgehoben!");
        }
    }

    public string GetInteractionText()
    {
        return $"[E] {keyType.ToString()} aufheben";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return true; // Schlüssel können immer aufgehoben werden
    }
}

public enum KeyType
{
//    Bronze,       // Für...
//   Silber,        // Für... 
    Gold,           // Für ...
//   Master         // Für Ausgang
}
