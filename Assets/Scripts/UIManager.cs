using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using System.Linq;

public class UIManager : MonoBehaviour
{
    // Singleton Pattern - Es gibt nur einen UIManager im ganzen Spiel
    public static UIManager Instance;

    [Header("UI Elemente")]
    public TextMeshProUGUI interactionText;      // Text: "[E] xy aufheben"
    public TextMeshProUGUI scoreTextUpperPanel;  // Text: "Punkte: xy"
    public TextMeshProUGUI scoreTextInventory;   // Text: "Punkte: xy" im Inventory
    public TextMeshProUGUI messageText;          // Text: "xy eigesammelt!"
    public TextMeshProUGUI keyCountText;         // Text: "Schlüssel: 3"
    public TextMeshProUGUI itemCountText;        // Text: "Gegenstände: 7"


    [Header("Schlüssel Bilder Upper Panel")]
    public GameObject silverKeyUpperObject;            // Bild für Silber-Schlüssel  
    public GameObject goldKeyUpperObject;              // Bild für Gold-Schlüssel
    public GameObject masterKeyUpperObject;            // Bild für Master-Schlüssel


    [Header("Bilder Inventory Panel")]
    public GameObject silverKeyInventory;            // Bild für Silber-Schlüssel  
    public GameObject goldKeyInventory;              // Bild für Gold-Schlüssel
    public GameObject masterKeyInventory;            // Bild für Master-Schlüssel

    [Header("Einstellungen")]
    public float messageDuration = 3f;           // Wie lange Nachrichten angezeigt werden (3 Sekunden)

    public GameObject Inventory;
    public GameObject ItemPrefab;  // Prefab für Items im Inventar
    public GameObject FoodInventoryParent;  // Parent-Objekt für Essen im Inventar

    // Private Variablen
    /*private int currentScore = 0;  */              // Aktuelle Punkte
    private Coroutine messageCoroutine;          // Für zeitgesteuerte Nachrichten


    void Awake()
    {
        // Singleton Setup - Es gibt nur einen UIManager
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

    void Start()
    {
        ResetUI();
    }

    // NEU: Verstecke alle Schlüssel-Bilder
    public void HideAllKeyUpperObjects()
    {
        // Methode 2: Über einzelne Referenzen
        if (silverKeyUpperObject != null) silverKeyUpperObject.SetActive(false);
        if (goldKeyUpperObject != null) goldKeyUpperObject.SetActive(false);
        if (masterKeyUpperObject != null) masterKeyUpperObject.SetActive(false);
    }

    public void HideAllKeysInventory()
    {   // Methode 2: Über einzelne Referenzen
        if (silverKeyInventory != null) silverKeyInventory.SetActive(false);
        if (goldKeyInventory != null) goldKeyInventory.SetActive(false);
        if (masterKeyInventory != null) masterKeyInventory.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            // Toggle Inventory Sichtbarkeit
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

    // Hauptfunktion - Zeige/Verstecke Schlüssel-Bilder basierend auf Inventar
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
            // Optional: Passe das Aussehen/Text des Prefabs an, z.B. Name oder Icon
            // Beispiel: newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.ToString();
        }
    }

    // Zeige das Bild für einen bestimmten Schlüssel-Typ
    public void ShowKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchlüssel:
                if (silverKeyUpperObject && silverKeyInventory != null)
                {
                    silverKeyUpperObject.SetActive(true);
                    silverKeyInventory.SetActive(true);  // NEU: Silber-Schlüssel auch im Inventory anzeigen
                    Debug.Log("Silber-Schlüssel Bild angezeigt");
                }
                break;

            case KeyType.GoldSchlüssel:
                if (goldKeyUpperObject && goldKeyInventory != null)
                {
                    goldKeyUpperObject.SetActive(true);
                    goldKeyInventory.SetActive(true);  // NEU: Gold-Schlüssel auch im Inventory anzeigen
                    Debug.Log("Gold-Schlüssel Bild angezeigt");
                }
                break;

            case KeyType.MasterSchlüssel:
                if (masterKeyUpperObject && masterKeyInventory != null)
                {
                    masterKeyUpperObject.SetActive(true);
                    masterKeyInventory.SetActive(true);  // NEU: Master-Schlüssel auch im Inventory anzeigen
                    Debug.Log("Master-Schlüssel Bild angezeigt");
                }
                break;
        }
    }

    // Verstecke das Bild für einen bestimmten Schlüssel-Typ
    public void HideKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchlüssel:
                if (silverKeyUpperObject && silverKeyInventory != null)
                {
                    silverKeyUpperObject.SetActive(false);
                    silverKeyInventory.SetActive(false);  // NEU: Silber-Schlüssel auch im Inventory verstecken
                    Debug.Log("Silber-Schlüssel Bild versteckt");
                }
                break;

            case KeyType.GoldSchlüssel:
                if (goldKeyUpperObject && goldKeyInventory != null)
                {
                    goldKeyUpperObject.SetActive(false);
                    goldKeyInventory.SetActive(false);  // NEU: Gold-Schlüssel auch im Inventory verstecken
                    Debug.Log("Gold-Schlüssel Bild versteckt");
                }
                break;

            case KeyType.MasterSchlüssel:
                if (masterKeyUpperObject && masterKeyInventory != null)
                {
                    masterKeyUpperObject.SetActive(false);
                    masterKeyInventory.SetActive(false);  // NEU: Master-Schlüssel auch im Inventory verstecken
                    Debug.Log("Master-Schlüssel Bild versteckt");
                }
                break;
        }
    }

    // INTERAKTIONS-TEXT (zeigt "[E]...")
    public void ShowInteractionPrompt(string text)
    {
        interactionText.text = text;           // Setze den Text
        interactionText.gameObject.SetActive(true);  // Mache sichtbar
    }

    public void HideInteractionPrompt()
    {
        interactionText.gameObject.SetActive(false);  // Verstecke Text
    }

    // PUNKTE-SYSTEM
    public void UpdateScore()
    {
        var currentScore = SaveSystem.Instance.currentSaveData.scoreData;
        scoreTextUpperPanel.text = currentScore.ToString();  // "Punktestand" im Upper Panel
        scoreTextInventory.text = currentScore.ToString();  // "Punktestand" im Inventory
    }

    // NACHRICHTEN-SYSTEM (temporäre Meldungen)
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

    // Verstecke Nachricht nach bestimmter Zeit
    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);  // Warte 3 Sekunden
        HideMessage();  // Verstecke Nachricht
    }

    public void HideMessage()
    {
        messageText.gameObject.SetActive(false);  // Verstecke Nachricht
    }

    public void HideInteractionText()
    { 
        interactionText.gameObject.SetActive(false);  // Verstecke Interaktionstext
    }

    // GEGENSTÄNDE-ANZEIGE  
    public void UpdateItemCount(int count = 0)
    {
        itemCountText.text = $"Gegenstände: {count}";  
    }

    public void ResetUI()
    {
        UpdateScore();
        UpdateItemCount();
        HideMessage();
        HideAllKeyUpperObjects();  // Alle Schlüssel-Bilder im Upper Panel verstecken
        HideAllKeysInventory();  // Alle Schlüssel-Bilder im Inventory verstecken
        //UpdateFoodDisplay( SaveSystem.Instance.currentSaveData.inventoryData.collectedFood.Where(f => f.isCollected)
        //                .Select(f => f.foodName)
        //                .ToList() );  // Leere Food-Anzeige
    }
}