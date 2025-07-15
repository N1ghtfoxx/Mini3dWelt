using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    //private bool gameLoaded;

    public void Start()
    {
        inventoryManager = Object.FindFirstObjectByType<InventoryManager>();

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager nicht gefunden!");
        }

        UpdateChestVisual();

        StartCoroutine(onSaveGameLoaded());
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

        SaveSystem.Instance.MarkChestAsOpened(gameObject.name);
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
        string collectedItems = "";

        foreach (ChestItem item in chestItems)
        {
            //inventoryManager.AddItem(item.itemType, item.itemName, item.pointValue);
            totalValue += item.pointValue;
            collectedItems += $"{item.itemName} ";
        }

        chestItems.Clear(); // Leere die Truhe nach dem Sammeln
        UIManager.Instance.ShowMessage($"Truhe geöffnet! {collectedItems} gefunden!");
        SaveSystem.Instance.AddScore(totalValue);

    }

    /*    private void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    */

    public IEnumerator onSaveGameLoaded()
    {
        while (SaveSystem.Instance == null)
            yield return null;
        //gameLoaded = true;
        var blah = SaveSystem.Instance.currentSaveData.chestData.FirstOrDefault(c => c.chestId == gameObject.name);
        if (blah != null)
        {
            isOpened = blah.isOpened;

            if (isOpened)
            {
                UpdateChestVisual();
            }
        }
    }

    [ContextMenu("Reset Chest")]
    public void ResetChest()
    {
        isOpened = false;
        UpdateChestVisual();
    }
}