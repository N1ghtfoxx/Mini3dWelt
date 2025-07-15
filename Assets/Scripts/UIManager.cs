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
    public TextMeshProUGUI keyCountText;         // Text: "Schl�ssel: 3"
    public TextMeshProUGUI itemCountText;        // Text: "Gegenst�nde: 7"


    [Header("Schl�ssel Bilder Upper Panel")]
    public GameObject silverKeyUpperObject;            // Bild f�r Silber-Schl�ssel  
    public GameObject goldKeyUpperObject;              // Bild f�r Gold-Schl�ssel
    public GameObject masterKeyUpperObject;            // Bild f�r Master-Schl�ssel


    [Header("Bilder Inventory Panel")]
    public GameObject silverKeyInventory;            // Bild f�r Silber-Schl�ssel  
    public GameObject goldKeyInventory;              // Bild f�r Gold-Schl�ssel
    public GameObject masterKeyInventory;            // Bild f�r Master-Schl�ssel

    [Header("Einstellungen")]
    public float messageDuration = 3f;           // Wie lange Nachrichten angezeigt werden (3 Sekunden)

    public GameObject Inventory;
    public GameObject ItemPrefab;  // Prefab f�r Items im Inventar
    public GameObject FoodInventoryParent;  // Parent-Objekt f�r Essen im Inventar

    // Private Variablen
    /*private int currentScore = 0;  */              // Aktuelle Punkte
    private Coroutine messageCoroutine;          // F�r zeitgesteuerte Nachrichten


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
            Destroy(gameObject);  // L�sche doppelte UIManager
        }
    }

    void Start()
    {
        ResetUI();
    }

    // NEU: Verstecke alle Schl�ssel-Bilder
    public void HideAllKeyUpperObjects()
    {
        // Methode 2: �ber einzelne Referenzen
        if (silverKeyUpperObject != null) silverKeyUpperObject.SetActive(false);
        if (goldKeyUpperObject != null) goldKeyUpperObject.SetActive(false);
        if (masterKeyUpperObject != null) masterKeyUpperObject.SetActive(false);
    }

    public void HideAllKeysInventory()
    {   // Methode 2: �ber einzelne Referenzen
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

    // Hauptfunktion - Zeige/Verstecke Schl�ssel-Bilder basierend auf Inventar
    public void UpdateKeyDisplay(List<KeyType> keys)
    {
        Debug.Log($"=== UpdateKeyDisplay aufgerufen ===");
        Debug.Log($"Anzahl Schl�ssel: {keys.Count}");

        // Erst alle Bilder verstecken
        HideAllKeyUpperObjects();
        HideAllKeysInventory();

        // Dann nur die Bilder f�r vorhandene Schl�ssel anzeigen
        foreach (KeyType key in keys)
        {
            ShowKeyImage(key);
        }
    }

    public void UpdateFoodDisplay(List<string> food)
    {
        // 1. Alle bisherigen Food-Objekte im Parent l�schen
        foreach (Transform child in FoodInventoryParent.transform)
        {
            Destroy(child.gameObject);
        }

        // 2. F�r jedes Food-Item ein neues Prefab instanziieren
        foreach (var item in food)
        {
            GameObject newItem = Instantiate(ItemPrefab, FoodInventoryParent.transform);
            // Optional: Passe das Aussehen/Text des Prefabs an, z.B. Name oder Icon
            // Beispiel: newItem.GetComponentInChildren<TextMeshProUGUI>().text = item.ToString();
        }
    }

    // Zeige das Bild f�r einen bestimmten Schl�ssel-Typ
    public void ShowKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchl�ssel:
                if (silverKeyUpperObject && silverKeyInventory != null)
                {
                    silverKeyUpperObject.SetActive(true);
                    silverKeyInventory.SetActive(true);  // NEU: Silber-Schl�ssel auch im Inventory anzeigen
                    Debug.Log("Silber-Schl�ssel Bild angezeigt");
                }
                break;

            case KeyType.GoldSchl�ssel:
                if (goldKeyUpperObject && goldKeyInventory != null)
                {
                    goldKeyUpperObject.SetActive(true);
                    goldKeyInventory.SetActive(true);  // NEU: Gold-Schl�ssel auch im Inventory anzeigen
                    Debug.Log("Gold-Schl�ssel Bild angezeigt");
                }
                break;

            case KeyType.MasterSchl�ssel:
                if (masterKeyUpperObject && masterKeyInventory != null)
                {
                    masterKeyUpperObject.SetActive(true);
                    masterKeyInventory.SetActive(true);  // NEU: Master-Schl�ssel auch im Inventory anzeigen
                    Debug.Log("Master-Schl�ssel Bild angezeigt");
                }
                break;
        }
    }

    // Verstecke das Bild f�r einen bestimmten Schl�ssel-Typ
    public void HideKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchl�ssel:
                if (silverKeyUpperObject && silverKeyInventory != null)
                {
                    silverKeyUpperObject.SetActive(false);
                    silverKeyInventory.SetActive(false);  // NEU: Silber-Schl�ssel auch im Inventory verstecken
                    Debug.Log("Silber-Schl�ssel Bild versteckt");
                }
                break;

            case KeyType.GoldSchl�ssel:
                if (goldKeyUpperObject && goldKeyInventory != null)
                {
                    goldKeyUpperObject.SetActive(false);
                    goldKeyInventory.SetActive(false);  // NEU: Gold-Schl�ssel auch im Inventory verstecken
                    Debug.Log("Gold-Schl�ssel Bild versteckt");
                }
                break;

            case KeyType.MasterSchl�ssel:
                if (masterKeyUpperObject && masterKeyInventory != null)
                {
                    masterKeyUpperObject.SetActive(false);
                    masterKeyInventory.SetActive(false);  // NEU: Master-Schl�ssel auch im Inventory verstecken
                    Debug.Log("Master-Schl�ssel Bild versteckt");
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

    // NACHRICHTEN-SYSTEM (tempor�re Meldungen)
    public void ShowMessage(string message)
    {
        
        messageText.text = message;           // Setze Nachricht
        messageText.gameObject.SetActive(true);  // Mache sichtbar

        HideInteractionText();  // Verstecke Interaktionstext

        // Stoppe alte Nachricht falls eine l�uft
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

    // GEGENST�NDE-ANZEIGE  
    public void UpdateItemCount(int count = 0)
    {
        itemCountText.text = $"Gegenst�nde: {count}";  
    }

    public void ResetUI()
    {
        UpdateScore();
        UpdateItemCount();
        HideMessage();
        HideAllKeyUpperObjects();  // Alle Schl�ssel-Bilder im Upper Panel verstecken
        HideAllKeysInventory();  // Alle Schl�ssel-Bilder im Inventory verstecken
        //UpdateFoodDisplay( SaveSystem.Instance.currentSaveData.inventoryData.collectedFood.Where(f => f.isCollected)
        //                .Select(f => f.foodName)
        //                .ToList() );  // Leere Food-Anzeige
    }
}