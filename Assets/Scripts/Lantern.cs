using UnityEngine;

// summary:
// This script defines a Lantern class that manages the state of a lantern in the game.
public class Lantern : MonoBehaviour
{
    [Header("Lantern Settings")]
    public bool isOn = false;

    [Header("Lantern Visuals")]
    public GameObject onModel;   // Lit lantern
    public GameObject offModel;  // Unlit lantern

    // summary:
    // Start method initializes the lantern's visual state based on the current time of day.
    // It checks if the DayNightCycle instance is available and sets the lantern state accordingly.
    void Start()
    {
        UpdateLanternVisual();

        if (DayNightCycle.Instance != null)
        {
            var dayNightCycle = DayNightCycle.Instance as DayNightCycle;
            bool shouldBeOn = dayNightCycle != null && dayNightCycle.IsNight();
            SetLanternState(shouldBeOn);
        }
    }

    // summary:
    // SetLanternState method allows external scripts to turn the lantern on or off.
    // It updates the lantern's visual state based on the provided boolean value.
    public void SetLanternState(bool turnOn)
    {
        isOn = turnOn;
        UpdateLanternVisual();
    }

    // summary:
    // UpdateLanternVisual method updates the lantern's visual representation based on its current state.
    private void UpdateLanternVisual()
    {
        if (onModel != null)
            onModel.SetActive(isOn);         // Leuchtendes Modell aktivieren

        if (offModel != null)
            offModel.SetActive(!isOn);       // Dunkles Modell deaktivieren
    }

    // summary:
    // ResetLantern method is a context menu action that resets the lantern to its off state.
    [ContextMenu("Reset Lantern")]
    public void ResetLantern()
    {
        isOn = false;
        UpdateLanternVisual();
    }
}