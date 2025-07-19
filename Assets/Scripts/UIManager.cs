using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using System.Linq;

public class UIManager : MonoBehaviour
{
    // Singleton Pattern - only one instance of UIManager
    public static UIManager Instance;

    [Header("UI Elemente")]
    // summary:
    // UI element references for the game UI
    public TextMeshProUGUI interactionText;      // Text: "[E] xy aufheben"
    public TextMeshProUGUI scoreTextUpperPanel;  // Text:  Punktestand im Upper Panel
    public TextMeshProUGUI scoreTextInventory;   // Text:  Punktestand im Inventory
    public TextMeshProUGUI messageText;          // Text: "xy eigesammelt!"
    public TextMeshProUGUI keyCountText;         // Text: "Schlüssel: 3"
    public TextMeshProUGUI itemCountText;        // Text: "Gegenstände: 7"


    [Header("Schlüssel Bilder Upper Panel")]
    // summary:
    // GameObjects for key images in the upper panel
    public GameObject silverKeyUpperObject;              
    public GameObject goldKeyUpperObject;              
    public GameObject masterKeyUpperObject;            


    [Header("Bilder Inventory Panel")]
    // summary:
    // GameObjects for key images in the inventory panel
    public GameObject silverKeyInventory;             
    public GameObject goldKeyInventory;              
    public GameObject masterKeyInventory;            

    [Header("Einstellungen")]
    // summary:
    // Settings for the UI
    public float messageDuration = 3f;  // Wie lange Nachrichten angezeigt werden (3 Sekunden)

    public GameObject Inventory;
    public GameObject ItemPrefab;  // Prefab für Items im Inventar
    public GameObject FoodInventoryParent;  // Parent-Objekt für Essen im Inventar

    // Private Variablen
    private Coroutine messageCoroutine; // Für zeitgesteuerte Nachrichten


    void Awake()
    {
        // Singleton Setup - to ensure only one UIManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Bleibt beim Szenen-Wechsel erhalten
        }
        else
        {
            Destroy(gameObject);  // Lösche doppelte UIManager
        }
    }

    // summary:
    // Initialize UI elements when the game starts
    // This is called once at the start of the game
    void Start()
    {
        ResetUI();
    }

    // summary:
    // This method hides all key images in the upper panel
    // This is called when the inventory is closed or reset
    public void HideAllKeyUpperObjects()
    {
        if (silverKeyUpperObject != null) silverKeyUpperObject.SetActive(false);
        if (goldKeyUpperObject != null) goldKeyUpperObject.SetActive(false);
        if (masterKeyUpperObject != null) masterKeyUpperObject.SetActive(false);
    }

    // summary:
    // This method hides all key images in the inventory
    // This is called when the inventory is closed or reset
    public void HideAllKeysInventory()
    {   
        if (silverKeyInventory != null) silverKeyInventory.SetActive(false);
        if (goldKeyInventory != null) goldKeyInventory.SetActive(false);
        if (masterKeyInventory != null) masterKeyInventory.SetActive(false);
    }

    // summary:
    // Update is called once per frame
    // This method checks for user input to toggle the inventory visibility
    private void Update()
    {
        // Check if the player presses the "I" key to toggle inventory visibility
        // If Inventory GameObject is not assigned, log a warning
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Inventory != null)
            {
                Inventory.SetActive(!Inventory.activeSelf);
                Debug.Log($"Inventory Sichtbarkeit: {Inventory.activeSelf}");
            }
            else
            {
                Debug.LogWarning("Inventory GameObject nicht zugewiesen!");
            }
        }
    }

    // summary:
    // Update the key display in the UI based on the provided list of keys
    // This method is called when the player collects or uses keys
    // It hides all key images first, then shows only the keys that are currently collected
    public void UpdateKeyDisplay(List<KeyType> keys)
    {
        Debug.Log($"=== UpdateKeyDisplay aufgerufen ===");
        Debug.Log($"Anzahl Schlüssel: {keys.Count}");

        // Erst alle Bilder verstecken
        HideAllKeyUpperObjects();
        HideAllKeysInventory();

        // Dann nur die Bilder für vorhandene Schlüssel anzeigen
        foreach (KeyType key in keys)
        {
            ShowKeyImage(key);
        }
    }

    // summary:
    // Update the food display in the inventory UI
    // This method is called when the player collects food items
    // It clears the previous food items and instantiates new ones based on the provided list
    public void UpdateFoodDisplay(List<string> food)
    {
        // 1. Alle bisherigen Food-Objekte im Parent löschen
        foreach (Transform child in FoodInventoryParent.transform)
        {
            Destroy(child.gameObject);
        }

        // 2. Für jedes Food-Item ein neues Prefab instanziieren
        foreach (var item in food)
        {
            GameObject newItem = Instantiate(ItemPrefab, FoodInventoryParent.transform);
        }
    }

    // summary:
    // Show the image for a specific key type in the upper panel and inventory
    // This method is called when the player collects a key
    // It activates the corresponding GameObject for the key image
    public void ShowKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchlüssel:
                if (silverKeyUpperObject && silverKeyInventory != null)
                {
                    silverKeyUpperObject.SetActive(true);
                    silverKeyInventory.SetActive(true);  
                    Debug.Log("Silber-Schlüssel Bild angezeigt");
                }
                break;

            case KeyType.GoldSchlüssel:
                if (goldKeyUpperObject && goldKeyInventory != null)
                {
                    goldKeyUpperObject.SetActive(true);
                    goldKeyInventory.SetActive(true);  
                    Debug.Log("Gold-Schlüssel Bild angezeigt");
                }
                break;

            case KeyType.MasterSchlüssel:
                if (masterKeyUpperObject && masterKeyInventory != null)
                {
                    masterKeyUpperObject.SetActive(true);
                    masterKeyInventory.SetActive(true);  
                    Debug.Log("Master-Schlüssel Bild angezeigt");
                }
                break;
        }
    }

    // summary:
    // Hide the image for a specific key type in the upper panel and inventory
    // This method is called when the player uses a key or it is no longer needed
    // It deactivates the corresponding GameObject for the key image
    public void HideKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchlüssel:
                if (silverKeyUpperObject && silverKeyInventory != null)
                {
                    silverKeyUpperObject.SetActive(false);
                    silverKeyInventory.SetActive(false);  
                    Debug.Log("Silber-Schlüssel Bild versteckt");
                }
                break;

            case KeyType.GoldSchlüssel:
                if (goldKeyUpperObject && goldKeyInventory != null)
                {
                    goldKeyUpperObject.SetActive(false);
                    goldKeyInventory.SetActive(false);  
                    Debug.Log("Gold-Schlüssel Bild versteckt");
                }
                break;

            case KeyType.MasterSchlüssel:
                if (masterKeyUpperObject && masterKeyInventory != null)
                {
                    masterKeyUpperObject.SetActive(false);
                    masterKeyInventory.SetActive(false);  
                    Debug.Log("Master-Schlüssel Bild versteckt");
                }
                break;
        }
    }

    // summary:
    // Show an interaction prompt with the specified text
    // This method is called when the player can interact with an object
    // It sets the interaction text and makes it visible
    public void ShowInteractionPrompt(string text)
    {
        interactionText.text = text;           // Setze den Text
        interactionText.gameObject.SetActive(true);  // Mache sichtbar
    }

    // summary:
    // Hide the interaction prompt text
    // This method is called when the player can no longer interact with an object
    // It deactivates the interaction text GameObject
    // This is useful to clear the prompt when the player moves away from the interactable object
    // or when the interaction is completed
    public void HideInteractionPrompt()
    {
        interactionText.gameObject.SetActive(false);  // Verstecke Text
    }

    // PUNKTE-SYSTEM
    // summary:
    // Update the score display in the UI
    // This method is called when the player's score changes
    // It updates the score text in both the upper panel and the inventory
    public void UpdateScore()
    {
        var currentScore = SaveSystem.Instance.currentSaveData.scoreData;
        scoreTextUpperPanel.text = currentScore.ToString();  
        scoreTextInventory.text = currentScore.ToString();  
    }

    // NACHRICHTEN-SYSTEM (temporäre Meldungen)
    // summary:
    // Show a message in the UI for a short duration
    // This method is called to display temporary messages like "Item collected!"
    public void ShowMessage(string message)
    {
        
        messageText.text = message;           // Setze Nachricht
        messageText.gameObject.SetActive(true);  // Mache sichtbar

        HideInteractionText();  // Verstecke Interaktionstext

        // Stoppe alte Nachricht falls eine läuft
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        // Starte Timer um Nachricht nach 3 Sekunden zu verstecken
        messageCoroutine = StartCoroutine(HideMessageAfterDelay());
    }

    // summary:
    // Coroutine to hide the message after a delay
    // This is used to automatically hide the message after a specified duration
    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);  // Warte 3 Sekunden
        HideMessage();  // Verstecke Nachricht
    }

    // summary:
    // Hide the message text in the UI
    // This method is called to clear the message from the UI
    public void HideMessage()
    {
        messageText.gameObject.SetActive(false);  // Verstecke Nachricht
    }

    // summary:
    // Hide the interaction text in the UI
    // This method is called to clear the interaction prompt when it's no longer needed
    // This is useful to clear the prompt when the player moves away from the interactable object
    // or when the interaction is completed
    public void HideInteractionText()
    { 
        interactionText.gameObject.SetActive(false);  // Verstecke Interaktionstext
    }

    // GEGENSTÄNDE-ANZEIGE
    // summary:
    // Update the item count display in the UI
    // This method is called to update the number of items collected
    // It sets the item count text to show the current count
    // If no count is provided, it defaults to 0
    public void UpdateItemCount(int count = 0)
    {
        itemCountText.text = $"Gegenstände: {count}";  
    }

    // summary:
    // Reset the UI to its initial state
    // This method is called to clear all UI elements and reset them to their default values
    // It updates the score, item count, hides messages, and clears key images
    // This is useful when starting a new game or resetting the current game state
    // It ensures that the UI is in a clean state before the player starts interacting with the game
    public void ResetUI()
    {
        UpdateScore();
        UpdateItemCount();
        HideMessage();
        HideAllKeyUpperObjects();  
        HideAllKeysInventory();  
    }
}