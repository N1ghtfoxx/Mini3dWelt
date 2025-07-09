using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton Pattern - Es gibt nur einen UIManager im ganzen Spiel
    public static UIManager Instance;

    [Header("UI Elemente")]
    public TextMeshProUGUI interactionText;      // Text: "[E] Schlüssel aufheben"
    public TextMeshProUGUI scoreText;            // Text: "Punkte: 150"
    public TextMeshProUGUI messageText;          // Text: "Schlüssel gefunden!"
    public TextMeshProUGUI keyCountText;         // Text: "Schlüssel: 3"
    public TextMeshProUGUI itemCountText;        // Text: "Gegenstände: 7"


    [Header("Schlüssel Bilder Upper Panel")]
    public GameObject silverKeyUpperObject;            // Bild für Silber-Schlüssel  
    public GameObject goldKeyUpperObject;              // Bild für Gold-Schlüssel
    public GameObject masterKeyUpperObject;            // Bild für Master-Schlüssel

/*
    [Header("Bilder Inventory Panel")]
    public GameObject silverKeyInventory;            // Bild für Silber-Schlüssel  
    public GameObject goldKeyInventory;              // Bild für Gold-Schlüssel
    public GameObject masterKeyInventory;            // Bild für Master-Schlüssel
    public GameObject gemsInventory;
    public GameObject foodInventory;
    public GameObject weaponsInventory;
*/

    [Header("Einstellungen")]
    public float messageDuration = 3f;           // Wie lange Nachrichten angezeigt werden (3 Sekunden)

    public GameObject Inventory;
    public GameObject ItemPrefab;  // Prefab für Items im Inventar
    public GameObject FoodInventoryParent;  // Parent-Objekt für Essen im Inventar

    // Private Variablen
    private int currentScore = 0;                // Aktuelle Punkte
    private Coroutine messageCoroutine;          // Für zeitgesteuerte Nachrichten


    void Awake()
    {
        // Singleton Setup - Es gibt nur einen UIManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Bleibt beim Szenen-Wechsel erhalten
 /*           silverKeyInventory.SetActive(false);  // Verstecke Silber-Schlüssel Bild
            goldKeyInventory.SetActive(false);  // Verstecke Silber-Schlüssel Bild
            masterKeyInventory.SetActive(false);  // Verstecke Silber-Schlüssel Bild
            foodInventory.SetActive(false);  // Verstecke Food Bild
            weaponsInventory.SetActive(false);  // Verstecke Weapons Bild
 */
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
    private void HideAllKeyImages()
    {
        // Methode 2: Über einzelne Referenzen
        if (silverKeyUpperObject != null) silverKeyUpperObject.SetActive(false);
        if (goldKeyUpperObject != null) goldKeyUpperObject.SetActive(false);
        if (masterKeyUpperObject != null) masterKeyUpperObject.SetActive(false);
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
        HideAllKeyImages();

        // Dann nur die Bilder für vorhandene Schlüssel anzeigen
        foreach (KeyType key in keys)
        {
            ShowKeyImage(key);
        }
    }

    public void UpdateFoodDisplay(List<ItemType> food)
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
    private void ShowKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchlüssel:
                if (silverKeyUpperObject != null)
                {
                    silverKeyUpperObject.SetActive(true);
                    Debug.Log("Silber-Schlüssel Bild angezeigt");
                }
                break;

            case KeyType.GoldSchlüssel:
                if (goldKeyUpperObject != null)
                {
                    goldKeyUpperObject.SetActive(true);
                    Debug.Log("Gold-Schlüssel Bild angezeigt");
                }
                break;

            case KeyType.MasterSchlüssel:
                if (masterKeyUpperObject != null)
                {
                    masterKeyUpperObject.SetActive(true);
                    Debug.Log("Master-Schlüssel Bild angezeigt");
                }
                break;
        }
    }

    // Verstecke das Bild für einen bestimmten Schlüssel-Typ
    private void HideKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchlüssel:
                if (silverKeyUpperObject != null)
                {
                    silverKeyUpperObject.SetActive(false);
                    Debug.Log("Silber-Schlüssel Bild versteckt");
                }
                break;

            case KeyType.GoldSchlüssel:
                if (goldKeyUpperObject != null)
                {
                    goldKeyUpperObject.SetActive(false);
                    Debug.Log("Gold-Schlüssel Bild versteckt");
                }
                break;

            case KeyType.MasterSchlüssel:
                if (masterKeyUpperObject != null)
                {
                    masterKeyUpperObject.SetActive(false);
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
    public void AddScore(int points)
    {
        currentScore += points;  // Addiere Punkte dazu
        UpdateScore(currentScore);  // Aktualisiere Anzeige
    }

    public void UpdateScore(int newScore)
    {
        currentScore = newScore;
        scoreText.text = currentScore.ToString();  // "Punkte: 150"
    }

    public int GetCurrentScore()
    {
        return currentScore;  // Gib aktuelle Punkte zurück
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
    public void UpdateItemCount(int count)
    {
        itemCountText.text = $"Gegenstände: {count}";  
    }

    public void ResetUI()
    {
        UpdateScore(0);
//        UpdateKeyCount(0);
        UpdateItemCount(0);
//        HideInteractionText();
        HideMessage();
        HideAllKeyImages();  // NEU: Alle Schlüssel-Bilder verstecken

    }
}