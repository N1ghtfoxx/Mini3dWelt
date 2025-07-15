using System.Collections;
using System.Linq;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable
{
    [Header("Food Settings")]
    public FoodType foodType;
    //private bool gameLoaded;
    //   public AudioClip pickupSound;

    public void Interact(PlayerInteraction player)
    {
        if (CanInteract(player))
        {
            // Zum Inventar hinzufügen
            player.GetComponent<InventoryManager>().AddFood();

            UIManager.Instance.ShowMessage($"{foodType.ToString()} eingesammelt!");

            SaveSystem.Instance.MarkFoodAsCollected(gameObject.name);

            // Effekte
            // AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);

            // UI Update
        }
    }

    public string GetInteractionText(PlayerInteraction player)
    {
        return $"[E] {foodType.ToString()} aufheben";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return true; // Schinken kann immer aufgehoben werden
    }

    public IEnumerator onSaveGameLoaded()
    {
        while (SaveSystem.Instance == null)
            yield return null;
        //gameLoaded = true;
        var foodData = SaveSystem.Instance.currentSaveData.collectedFoodInWorld.FirstOrDefault(f => f.foodName == gameObject.name);
        if (foodData != null)
            {
            if (foodData.isCollected)
            {
                // Wenn das Essen bereits gesammelt wurde, wird das Objekt zerstört
                Destroy(gameObject);
            }
        }
    }

    public void Start()
    {
        StartCoroutine(onSaveGameLoaded());
    }
}

public enum FoodType
{
    Schinken, // Schinken
}
