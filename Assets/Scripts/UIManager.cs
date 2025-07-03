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

    [Header("Schlüssel Anzeige")]
    public Transform keyDisplayParent;           // Wo die Schlüssel-Icons hingehören
    public GameObject keyIconPrefab;             // Vorlage für Schlüssel-Symbol

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
        // Zu Beginn alles verstecken/auf Null setzen
 //       HideInteractionText();
        UpdateScore(0);
        UpdateKeyCount(0);
        UpdateItemCount(0);
        HideMessage();
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
        scoreText.text = $"Punkte: {currentScore}";  // "Punkte: 150"
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

   // SCHLÜSSEL-ANZEIGE
    public void UpdateKeyDisplay(List<KeyType> keys)
    {
        
        // Lösche alle alten Schlüssel-Icons
        foreach (Transform child in keyDisplayParent)
        {
            Destroy(child.gameObject);
        }

        /*       // Erstelle neue Icons für jeden Schlüssel
               foreach (KeyType key in keys)
               {
                   GameObject keyIcon = Instantiate(keyIconPrefab, keyDisplayParent);

                   // Setze richtige Farbe je nach Schlüssel-Typ
                   Image iconImage = keyIcon.GetComponent<Image>();
                   switch (key)
                   {
                       case KeyType.BronzeSchlüssel:
                           iconImage.color = Color.red;  // Bronze-Schlüssel rot
                           break;
                       case KeyType.SilberSchlüssel:
                           iconImage.color = Color.gray;  // Silber-Schlüssel grau
                           break;
                       case KeyType.GoldSchlüssel:
                           iconImage.color = Color.yellow;  // Gold-Schlüssel gelb
                           break;
                       case KeyType.MasterSchlüssel:
                           iconImage.color = Color.green;  // Master-Schlüssel grün
                           break;
                   }

               }
        */

        // Aktualisiere Schlüssel-Zähler
        UpdateKeyCount(keys.Count);
    }

    public void UpdateKeyCount(int count)
    {
        keyCountText.text = $"Schlüssel: {count}";  // "Schlüssel: 3"
    }

    // GEGENSTÄNDE-ANZEIGE  
    public void UpdateItemCount(int count)
    {
        itemCountText.text = $"Gegenstände: {count}";  // "Gegenstände: 7"
    }

    // HILFSFUNKTIONEN
    public void ShowGameOverScreen()
    {
        ShowMessage("Glückwunsch! Du hast alle Gegenstände gefunden!");
        // Hier könntest du ein Game Over Menü anzeigen
    }

    public void ResetUI()
    {
        UpdateScore(0);
        UpdateKeyCount(0);
        UpdateItemCount(0);
//        HideInteractionText();
        HideMessage();
    }
}