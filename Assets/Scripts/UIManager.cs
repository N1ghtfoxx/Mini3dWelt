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

    [Header("Schl�ssel Anzeige")]
    public Transform keyDisplayParent;           // Wo die Schl�ssel-Icons hingeh�ren
    public GameObject keyIconPrefab;             // Vorlage f�r Schl�ssel-Symbol

    [Header("Einstellungen")]
    public float messageDuration = 3f;           // Wie lange Nachrichten angezeigt werden (3 Sekunden)

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
        }
        else
        {
            Destroy(gameObject);  // L�sche doppelte UIManager
        }
    }

    void Start()
    {
    // Zu Beginn alles verstecken/auf Null setzen
        // HideInteractionText();
        UpdateScore(0);
        UpdateKeyCount(0);
        UpdateItemCount(0);
        HideMessage();
        UpdateKeyCount(888); // Sollte "Schl�ssel: 888" anzeigen
    }

    // INTERAKTIONS-TEXT (zeigt "[E] Schl�ssel aufheben")
    public void ShowInteractionPrompt(string text)
    {
        interactionText.text = text;           // Setze den Text
        interactionText.gameObject.SetActive(true);  // Mache sichtbar
    }

    public void HideInteractionPrompt()
    {
        interactionText.gameObject.SetActive(false);  // Verstecke Text
    }

/*    // Alternative Namen f�r die gleichen Funktionen:
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

    // SCHL�SSEL-ANZEIGE
    public void UpdateKeyDisplay(List<KeyType> keys)
    {
        Debug.Log($"=== UpdateKeyDisplay aufgerufen ===");
        Debug.Log($"Anzahl Schl�ssel: {keys.Count}");

        // L�sche alle alten Schl�ssel-Icons (falls vorhanden)
        foreach (Transform child in keyDisplayParent)
        {
            Destroy(child.gameObject);
        }

        // Aktualisiere den Text-Z�hler
        UpdateKeyCount(keys.Count);

        // Optional: Debug-Ausgabe der vorhandenen Schl�ssel
        if (keys.Count > 0)
        {
            string keyNames = "";
            foreach (KeyType key in keys)
            {
                keyNames += key.ToString() + " ";
            }
            Debug.Log($"Vorhandene Schl�ssel: {keyNames}");
        }
        else
        {
            Debug.Log("Keine Schl�ssel mehr vorhanden");
        }
    }
    

    /*   public void AddKeyIcon(KeyType keyType)
       {
           // Erstelle ein neues Schl�ssel-Icon
           GameObject keyIcon = Instantiate(keyIconPrefab, keyDisplayParent);
           keyIcon.name = keyType.ToString();  // Setze den Namen des Icons
           // Optional: Hier k�nntest du das Icon anpassen (z.B. Sprite setzen)
           // keyIcon.GetComponent<Image>().sprite = ...;
       }
    */

    // SCHL�SSEL-Z�HLER
    public void UpdateKeyCount(int count)
    {
        keyCountText.text = $"Schl�ssel: {count}";  
    }

    // GEGENST�NDE-ANZEIGE  
    public void UpdateItemCount(int count)
    {
        itemCountText.text = $"Gegenst�nde: {count}";  
    }

/*    // HILFSFUNKTIONEN
    public void ShowGameOverScreen()
    {
        ShowMessage("Gl�ckwunsch! Du hast alle Gegenst�nde gefunden!");
        // Hier k�nntest du ein Game Over Men� anzeigen
    }
*/

    public void ResetUI()
    {
        UpdateScore(0);
        UpdateKeyCount(0);
        UpdateItemCount(0);
//        HideInteractionText();
        HideMessage();
    }
}