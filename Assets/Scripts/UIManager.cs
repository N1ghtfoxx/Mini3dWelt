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

 /*   [Header("Alte Schlüssel Anzeige - kann entfernt werden")]
    public Transform keyDisplayParent;           // Wo die Schlüssel-Icons hingehören
    public GameObject keyIconPrefab;             // Vorlage für Schlüssel-Symbol
 */

    [Header("Schlüssel Bilder - NEU!")]
    public GameObject silverKeyImage;            // Bild für Silber-Schlüssel  
    public GameObject goldKeyImage;              // Bild für Gold-Schlüssel
    public GameObject masterKeyImage;            // Bild für Master-Schlüssel

    [Header("Einstellungen")]
    public float messageDuration = 3f;           // Wie lange Nachrichten angezeigt werden (3 Sekunden)

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
        if (silverKeyImage != null) silverKeyImage.SetActive(false);
        if (goldKeyImage != null) goldKeyImage.SetActive(false);
        if (masterKeyImage != null) masterKeyImage.SetActive(false);
    }

    // NEU: Hauptfunktion - Zeige/Verstecke Schlüssel-Bilder basierend auf Inventar
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

    // NEU: Zeige das Bild für einen bestimmten Schlüssel-Typ
    private void ShowKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchlüssel:
                if (silverKeyImage != null)
                {
                    silverKeyImage.SetActive(true);
                    Debug.Log("Silber-Schlüssel Bild angezeigt");
                }
                break;

            case KeyType.GoldSchlüssel:
                if (goldKeyImage != null)
                {
                    goldKeyImage.SetActive(true);
                    Debug.Log("Gold-Schlüssel Bild angezeigt");
                }
                break;

            case KeyType.MasterSchlüssel:
                if (masterKeyImage != null)
                {
                    masterKeyImage.SetActive(true);
                    Debug.Log("Master-Schlüssel Bild angezeigt");
                }
                break;
        }
    }

    // NEU: Verstecke das Bild für einen bestimmten Schlüssel-Typ
    private void HideKeyImage(KeyType keyType)
    {
        switch (keyType)
        {

            case KeyType.SilberSchlüssel:
                if (silverKeyImage != null)
                {
                    silverKeyImage.SetActive(false);
                    Debug.Log("Silber-Schlüssel Bild versteckt");
                }
                break;

            case KeyType.GoldSchlüssel:
                if (goldKeyImage != null)
                {
                    goldKeyImage.SetActive(false);
                    Debug.Log("Gold-Schlüssel Bild versteckt");
                }
                break;

            case KeyType.MasterSchlüssel:
                if (masterKeyImage != null)
                {
                    masterKeyImage.SetActive(false);
                    Debug.Log("Master-Schlüssel Bild versteckt");
                }
                break;
        }
    }

    // INTERAKTIONS-TEXT (zeigt "[E] Schlüssel aufheben")
    public void ShowInteractionPrompt(string text)
    {
        interactionText.text = text;           // Setze den Text
        interactionText.gameObject.SetActive(true);  // Mache sichtbar
    }

    public void HideInteractionPrompt()
    {
        interactionText.gameObject.SetActive(false);  // Verstecke Text
    }

/*    // Alternative Namen für die gleichen Funktionen:
    public void ShowInteractionText(string text)
    {
        ShowInteractionPrompt(text);
    }

    public void HideInteractionText()
    {
        HideInteractionPrompt();
    }
*/

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

/*    // SCHLÜSSEL-ANZEIGE
    public void UpdateKeyDisplay(List<KeyType> keys)
    {
        Debug.Log($"=== UpdateKeyDisplay aufgerufen ===");
        Debug.Log($"Anzahl Schlüssel: {keys.Count}");

        // Lösche alle alten Schlüssel-Icons (falls vorhanden)
        foreach (Transform child in keyDisplayParent)
        {
            Destroy(child.gameObject);
        }

        // Aktualisiere den Text-Zähler
        UpdateKeyCount(keys.Count);

        // Optional: Debug-Ausgabe der vorhandenen Schlüssel
        if (keys.Count > 0)
        {
            string keyNames = "";
            foreach (KeyType key in keys)
            {
                keyNames += key.ToString() + " ";
            }
            Debug.Log($"Vorhandene Schlüssel: {keyNames}");
        }
        else
        {
            Debug.Log("Keine Schlüssel mehr vorhanden");
        }
    }
*/    

    /*   public void AddKeyIcon(KeyType keyType)
       {
           // Erstelle ein neues Schlüssel-Icon
           GameObject keyIcon = Instantiate(keyIconPrefab, keyDisplayParent);
           keyIcon.name = keyType.ToString();  // Setze den Namen des Icons
           // Optional: Hier könntest du das Icon anpassen (z.B. Sprite setzen)
           // keyIcon.GetComponent<Image>().sprite = ...;
       }
    */

/*    // SCHLÜSSEL-ZÄHLER
    public void UpdateKeyCount(int count)
    {
        keyCountText.text = $"Schlüssel: {count}";  
    }
*/

    // GEGENSTÄNDE-ANZEIGE  
    public void UpdateItemCount(int count)
    {
        itemCountText.text = $"Gegenstände: {count}";  
    }


/*    // HILFSFUNKTIONEN
    public void ShowGameOverScreen()
    {
        ShowMessage("Glückwunsch! Du hast alle Gegenstände gefunden!");
        // Hier könntest du ein Game Over Menü anzeigen
    }
*/

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