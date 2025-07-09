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
    public TextMeshProUGUI interactionText;      // Text: "[E] Schl�ssel aufheben"
    public TextMeshProUGUI scoreText;            // Text: "Punkte: 150"
    public TextMeshProUGUI messageText;          // Text: "Schl�ssel gefunden!"
    public TextMeshProUGUI keyCountText;         // Text: "Schl�ssel: 3"
    public TextMeshProUGUI itemCountText;        // Text: "Gegenst�nde: 7"


    [Header("Schl�ssel Bilder Upper Panel")]
    public GameObject silverKeyUpperObject;            // Bild f�r Silber-Schl�ssel  
    public GameObject goldKeyUpperObject;              // Bild f�r Gold-Schl�ssel
    public GameObject masterKeyUpperObject;            // Bild f�r Master-Schl�ssel

/*
    [Header("Bilder Inventory Panel")]
    public GameObject silverKeyInventory;            // Bild f�r Silber-Schl�ssel  
    public GameObject goldKeyInventory;              // Bild f�r Gold-Schl�ssel
    public GameObject masterKeyInventory;            // Bild f�r Master-Schl�ssel
    public GameObject gemsInventory;
    public GameObject foodInventory;
    public GameObject weaponsInventory;
*/

    [Header("Einstellungen")]
    public float messageDuration = 3f;           // Wie lange Nachrichten angezeigt werden (3 Sekunden)

    public GameObject Inventory;
    public GameObject ItemPrefab;  // Prefab f�r Items im Inventar
    public GameObject FoodInventoryParent;  // Parent-Objekt f�r Essen im Inventar

    // Private Variablen
    private int currentScore = 0;                // Aktuelle Punkte
    private Coroutine messageCoroutine;          // F�r zeitgesteuerte Nachrichten


    void Awake()
    {
        // Singleton Setup - Es gibt nur einen UIManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Bleibt beim Szenen-Wechsel erhalten
 /*           silverKeyInventory.SetActive(false);  // Verstecke Silber-Schl�ssel Bild
            goldKeyInventory.SetActive(false);  // Verstecke Silber-Schl�ssel Bild
            masterKeyInventory.SetActive(false);  // Verstecke Silber-Schl�ssel Bild
            foodInventory.SetActive(false);  // Verstecke Food Bild
            weaponsInventory.SetActive(false);  // Verstecke Weapons Bild
 */
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
    private void HideAllKeyImages()
    {
        // Methode 2: �ber einzelne Referenzen
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

    // Hauptfunktion - Zeige/Verstecke Schl�ssel-Bilder basierend auf Inventar
    public void UpdateKeyDisplay(List<KeyType> keys)
    {
        Debug.Log($"=== UpdateKeyDisplay aufgerufen ===");
        Debug.Log($"Anzahl Schl�ssel: {keys.Count}");

        // Erst alle Bilder verstecken
        HideAllKeyImages();

        // Dann nur die Bilder f�r vorhandene Schl�ssel anzeigen
        foreach (KeyType key in keys)
        {
            ShowKeyImage(key);
        }
    }

    public void UpdateFoodDisplay(List<ItemType> food)
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
    private void ShowKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchl�ssel:
                if (silverKeyUpperObject != null)
                {
                    silverKeyUpperObject.SetActive(true);
                    Debug.Log("Silber-Schl�ssel Bild angezeigt");
                }
                break;

            case KeyType.GoldSchl�ssel:
                if (goldKeyUpperObject != null)
                {
                    goldKeyUpperObject.SetActive(true);
                    Debug.Log("Gold-Schl�ssel Bild angezeigt");
                }
                break;

            case KeyType.MasterSchl�ssel:
                if (masterKeyUpperObject != null)
                {
                    masterKeyUpperObject.SetActive(true);
                    Debug.Log("Master-Schl�ssel Bild angezeigt");
                }
                break;
        }
    }

    // Verstecke das Bild f�r einen bestimmten Schl�ssel-Typ
    private void HideKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchl�ssel:
                if (silverKeyUpperObject != null)
                {
                    silverKeyUpperObject.SetActive(false);
                    Debug.Log("Silber-Schl�ssel Bild versteckt");
                }
                break;

            case KeyType.GoldSchl�ssel:
                if (goldKeyUpperObject != null)
                {
                    goldKeyUpperObject.SetActive(false);
                    Debug.Log("Gold-Schl�ssel Bild versteckt");
                }
                break;

            case KeyType.MasterSchl�ssel:
                if (masterKeyUpperObject != null)
                {
                    masterKeyUpperObject.SetActive(false);
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
        return currentScore;  // Gib aktuelle Punkte zur�ck
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
    public void UpdateItemCount(int count)
    {
        itemCountText.text = $"Gegenst�nde: {count}";  
    }

    public void ResetUI()
    {
        UpdateScore(0);
//        UpdateKeyCount(0);
        UpdateItemCount(0);
//        HideInteractionText();
        HideMessage();
        HideAllKeyImages();  // NEU: Alle Schl�ssel-Bilder verstecken

    }
}