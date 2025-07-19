using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//summary:
// This script manages a treasure chest in a game.
// It allows players to interact with the chest, open it, and collect items.
public class TreasureChest : MonoBehaviour, IInteractable
{
    [Header("Chest Settings")]
    public bool isOpened = false;

    [Header("Chest Contents")]
    public List<ChestItem> chestItems = new List<ChestItem>();

    [Header("Chest Visuals")]
    public GameObject closedChestModel;   // Geschlossene Truhe
    public GameObject openedChestModel;   // Geöffnete Truhe

    private InventoryManager inventoryManager;

    // summary:
    // Initializes the chest and sets up the inventory manager.
    public void Start()
    {
        inventoryManager = Object.FindFirstObjectByType<InventoryManager>();

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager nicht gefunden!");
        }

        UpdateChestVisual();    // update the visual state of the chest

        StartCoroutine(onSaveGameLoaded()); // Wait for the save system to load before checking chest state
    }

    // summary:
    // Checks if the player can interact with the chest.
    // Returns true if the chest is not opened.
    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpened;
    }

    // summary:
    // Returns the interaction text for the player.
    // If the chest is opened, returns an empty string.
    public string GetInteractionText(PlayerInteraction player)
    {
        return isOpened ? "" : "[E] Truhe öffnen";
    }

    // summary:
    // Handles the player's interaction with the chest.
    // If the chest is already opened, does nothing.
    public void Interact(PlayerInteraction player)
    {
        if (isOpened) return;
        OpenChest();
    }

    // summary:
    // Opens the chest, updates its visual state, collects items, and saves the state.
    private void OpenChest()
    {
        isOpened = true;

        UpdateChestVisual();

        CollectItems();

        SaveSystem.Instance.MarkChestAsOpened(gameObject.name);
    }

    // summary:
    // Updates the visual representation of the chest based on its state (opened or closed).
    private void UpdateChestVisual()
    {
        if (closedChestModel != null)
            closedChestModel.SetActive(!isOpened);

        if (openedChestModel != null)
            openedChestModel.SetActive(isOpened);
    }

    // summary:
    // Collects items from the chest and adds them to the player's inventory.
    // Displays a message with the collected items and updates the score.
    private void CollectItems()
    {
        if (inventoryManager == null) return;

        int totalValue = 0;
        string collectedItems = "";

        foreach (ChestItem item in chestItems)
        {
            totalValue += item.pointValue;
            collectedItems += $"{item.itemName} ";
        }

        chestItems.Clear(); // Leere die Truhe nach dem Sammeln
        UIManager.Instance.ShowMessage($"Truhe geöffnet! {collectedItems} gefunden!");
        SaveSystem.Instance.AddScore(totalValue);

    }

    // summary:
    // This method is commented out, but it was intended to play a sound when the chest is opened.
    /*    private void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    */

    // summary:
    // Coroutine that waits for the save system to load before checking the chest state.
    public IEnumerator onSaveGameLoaded()
    {
        while (SaveSystem.Instance == null || SaveSystem.Instance.currentSaveData.playerData.position[0] == 0f)
            yield return null;
        //gameLoaded = true;
        var chest = SaveSystem.Instance.currentSaveData.chestData.FirstOrDefault(c => c.chestId == gameObject.name);
        if (chest != null)
        {
            isOpened = chest.isOpened;

            if (isOpened)
            {
                UpdateChestVisual();
            }
        }
    }

    [ContextMenu("Reset Chest")]

    // summary:
    // Resets the chest to its initial state (closed).
    public void ResetChest()
    {
        isOpened = false;
        UpdateChestVisual();
    }
}