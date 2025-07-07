using UnityEngine;

public class Lantern : MonoBehaviour
{
    [Header("Lantern Settings")]
    public bool isOn = false;

    [Header("Lantern Visuals")]
    public GameObject onModel;   // Lit lantern
    public GameObject offModel;  // Unlit lantern

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

    public void SetLanternState(bool turnOn)
    {
        isOn = turnOn;
        UpdateLanternVisual();
    }

    private void UpdateLanternVisual()
    {
        if (onModel != null)
            onModel.SetActive(isOn);         // Leuchtendes Modell aktivieren

        if (offModel != null)
            offModel.SetActive(!isOn);       // Dunkles Modell deaktivieren
    }

    [ContextMenu("Reset Lantern")]
    public void ResetLantern()
    {
        isOn = false;
        UpdateLanternVisual();
    }
}