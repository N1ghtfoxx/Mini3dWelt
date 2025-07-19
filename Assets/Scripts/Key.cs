using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour, IInteractable
{
    [Header("Key Settings")]
    public KeyType keyType;
    //private bool gameLoaded; 

//    public AudioClip pickupSound;

    public void Interact(PlayerInteraction player)
    {
        if (CanInteract(player))
        {
            // Zum Inventar hinzuf�gen
            player.GetComponent<InventoryManager>().AddKey(keyType);

            // UI Update
            UIManager.Instance.ShowMessage($"{keyType.ToString()} aufgehoben!");

            SaveSystem.Instance.MarkKeyAsCollected(gameObject.name);

            // Effekte
            // AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Objekt wird aus der Szene entfernt
            Destroy(gameObject);

        }
    }

    public string GetInteractionText(PlayerInteraction player)
    {
        return $"[E] {keyType.ToString()} aufheben";
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return true; // Schl�ssel k�nnen immer aufgehoben werden
    }

    public IEnumerator onSaveGameLoaded()
    {
        while (SaveSystem.Instance == null || SaveSystem.Instance.currentSaveData.playerData.position[0] == 0f)
            yield return null;
        //gameLoaded = true;
        var keyData = SaveSystem.Instance.currentSaveData.collectedInWorldKeys.FirstOrDefault(k => k.keyName == gameObject.name);

        if (keyData != null)
            {
            if (keyData.isCollected)
            {
                // Wenn der Schl�ssel bereits gesammelt wurde, wird das Objekt zerst�rt
                Destroy(gameObject);
            }
        }
    }

    public void Start()
    {
        StartCoroutine(onSaveGameLoaded());
    }
}

public enum KeyType
{
    BronzeSchl�ssel,       // F�r...
    SilberSchl�ssel,       // F�r... 
    GoldSchl�ssel,         // F�r ...
    MasterSchl�ssel        // F�r Ausgang
}
