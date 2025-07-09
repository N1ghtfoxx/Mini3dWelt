using UnityEngine;

public class Food : MonoBehaviour, IInteractable
{
    [Header("Food Settings")]
    public FoodType foodType;
 //   public AudioClip pickupSound;

    public void Interact(PlayerInteraction player)
    {
        if (CanInteract(player))
        {
            // Zum Inventar hinzufügen
            player.GetComponent<InventoryManager>().AddFood();

            // Effekte
            // AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);

            // UI Update
            UIManager.Instance.ShowMessage($"{foodType.ToString()} eingesammelt!");
        }
    }

    public string GetInteractionText(PlayerInteraction player)
    {
        return $"[E] {foodType.ToString()} aufheben";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return true; // Schlüssel können immer aufgehoben werden
    }
}

public enum FoodType
{
    Schinken, // Schinken
}
