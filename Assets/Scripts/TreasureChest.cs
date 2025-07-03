using UnityEngine;
using System.Collections.Generic;

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

    void Start()
    {
        inventoryManager = Object.FindFirstObjectByType<InventoryManager>();

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager nicht gefunden!");
        }

        UpdateChestVisual();
    }

    public bool CanInteract(PlayerInteraction player)
    {
        return !isOpened;
    }

    public string GetInteractionText(PlayerInteraction player)
    {
        return isOpened ? "" : "[E] Truhe öffnen";
    }

    public void Interact(PlayerInteraction player)
    {
        if (isOpened) return;
        OpenChest();
    }

    private void OpenChest()
    {
        isOpened = true;

        UpdateChestVisual();

        CollectItems();

        UIManager.Instance.ShowMessage($"Truhe geöffnet! {chestItems.Count} Gegenstände gefunden!");
    }

    private void UpdateChestVisual()
    {
        if (closedChestModel != null)
            closedChestModel.SetActive(!isOpened);

        if (openedChestModel != null)
            openedChestModel.SetActive(isOpened);
    }

    private void CollectItems()
    {
        if (inventoryManager == null) return;

        int totalValue = 0;

        foreach (ChestItem item in chestItems)
        {
            inventoryManager.AddItem(item.itemType, item.itemName, item.pointValue);
            totalValue += item.pointValue;
        }

        if (totalValue > 0)
        {
            UIManager.Instance.AddScore(totalValue);
        }
    }

/*    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
*/

    [ContextMenu("Reset Chest")]
    public void ResetChest()
    {
        isOpened = false;
        UpdateChestVisual();
    }
}