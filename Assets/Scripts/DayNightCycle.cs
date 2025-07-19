using UnityEngine;

// summary:
// This script manages a day-night cycle in Unity, controlling the sun and moon's position,
// lighting, ambient light, fog, and skybox colors based on the time of day.
// It allows for customization of day length, celestial object properties, and various visual effects.
// It also includes methods to set specific times of day and toggle lanterns based on the time state.
public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float dayLengthInMinutes = 10f;
    [SerializeField] private float currentTime = 0.5f; // 0 = midnight, 0.5 = noon

    [Header("Celestial Objects")]
    [SerializeField] private Light sunLight;
    [SerializeField] private Light moonLight;
    [SerializeField] private Transform sunTransform;
    [SerializeField] private Transform moonTransform;

    [Header("Lighting")]
    [SerializeField] private AnimationCurve lightIntensityCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private float maxSunIntensity = 1.2f;
    [SerializeField] private float maxMoonIntensity = 0.3f;

    [Header("Colors")]
    [SerializeField] private Color sunColorMorning = new Color(1f, 0.8f, 0.6f);
    [SerializeField] private Color sunColorNoon = Color.white;
    [SerializeField] private Color sunColorEvening = new Color(1f, 0.5f, 0.3f);
    [SerializeField] private Color moonColor = new Color(0.8f, 0.9f, 1f);

    [Header("Ambient Light")]
    [SerializeField] private Color ambientDayColor = new Color(0.5f, 0.7f, 1f);
    [SerializeField] private Color ambientNightColor = new Color(0.2f, 0.3f, 0.5f);
    [SerializeField] private float ambientIntensity = 0.1f;

    [Header("Fog")]
    [SerializeField] private bool enableFog = true;
    [SerializeField] private Color fogDayColor = new Color(0.8f, 0.9f, 1f);
    [SerializeField] private Color fogNightColor = new Color(0.1f, 0.2f, 0.4f);

    [Header("Skybox")]
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private bool adjustSkybox = true;
    [SerializeField] private float skyboxBlendSpeed = 2f;

    [Header("Skybox Colors")]
    [SerializeField] private Color skyColorMorning = new Color(0.5f, 0.8f, 1f);
    [SerializeField] private Color skyColorNoon = new Color(0.4f, 0.7f, 1f);
    [SerializeField] private Color skyColorEvening = new Color(1f, 0.6f, 0.3f);
    [SerializeField] private Color skyColorNight = new Color(0.1f, 0.2f, 0.4f);

    [Header("Skybox Intensity")]
    [SerializeField] private float skyIntensityDay = 1.2f;
    [SerializeField] private float skyIntensityNight = 0.3f;
    [SerializeField] private AnimationCurve skyIntensityCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Debug")]
    [SerializeField] private bool timePaused = false;
    [SerializeField] private bool debugMode = false;

    private float timeSpeed;
    private Color currentSkyboxColor;
    private bool lastStateWasDay = true;

    public static object Instance { get; internal set; }

    // summary:
    // Initializes the day-night cycle settings and starts the cycle.
    // This method sets the time speed based on the day length, updates the initial lighting,
    // and configures the fog and skybox if enabled.
    void Start()
    {
        timeSpeed = 1f / (dayLengthInMinutes * 60f);
        UpdateLighting();

        if (enableFog)
            RenderSettings.fog = true;

        if (adjustSkybox && skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
            currentSkyboxColor = skyColorNoon;
        }
    }

    // summary:
    // Updates the day-night cycle each frame.
    // This method increments the current time based on the time speed,
    // updates the lighting, checks for day/night transitions, and toggles lanterns accordingly.
    // It also logs the current time in debug mode.
    void Update()
    {
        if (!timePaused)
        {
            currentTime += Time.deltaTime * timeSpeed;
            if (currentTime >= 1f) currentTime = 0f;
        }

        UpdateLighting();

        bool isNowDay = IsDay();
        if (isNowDay != lastStateWasDay)
        {
            lastStateWasDay = isNowDay;
            ToggleAllLanterns(!isNowDay); // true = night, turn lanterns on
        }

        if (debugMode)
        {
            Debug.Log($"Current Time: {GetReadableTime()} - Normalized Time: {currentTime:F2}");
        }
    }

    // summary:
    // Updates the lighting based on the current time.
    // This method calculates the sun and moon angles, adjusts their rotations,
    // sets the intensity and color of the sun and moon lights, updates ambient light,
    // fog settings, and skybox properties if enabled.
    // It also determines whether it is day or night based on the sun's position.
    void UpdateLighting()
    {
        float sunAngle = currentTime * 360f - 90f;
        float moonAngle = sunAngle + 180f;

        if (sunTransform != null)
            sunTransform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        if (moonTransform != null)
            moonTransform.rotation = Quaternion.Euler(moonAngle, 170f, 0f);

        bool isDay = sunAngle >= 0f && sunAngle <= 180f;

        if (sunLight != null)
        {
            sunLight.enabled = isDay;
            if (isDay)
            {
                float sunHeight = Mathf.Clamp01(Mathf.Sin(sunAngle * Mathf.Deg2Rad));
                sunLight.intensity = lightIntensityCurve.Evaluate(sunHeight) * maxSunIntensity;
                sunLight.color = CalculateSunColor(currentTime);
            }
        }

        if (moonLight != null)
        {
            moonLight.enabled = !isDay;
            if (!isDay)
            {
                float moonHeight = Mathf.Clamp01(Mathf.Sin(moonAngle * Mathf.Deg2Rad));
                moonLight.intensity = lightIntensityCurve.Evaluate(moonHeight) * maxMoonIntensity;
                moonLight.color = moonColor;
            }
        }

        UpdateAmbientLight();
        if (enableFog) UpdateFog();
        if (adjustSkybox && skyboxMaterial != null) UpdateSkybox();
    }

    // summary:
    // Calculates the color of the sun based on the current time.
    // This method interpolates between morning, noon, and evening colors based on the time of day.
    Color CalculateSunColor(float time)
    {
        if (time < 0.3f)
        {
            float t = Mathf.InverseLerp(0.2f, 0.3f, time);
            return Color.Lerp(sunColorMorning, sunColorNoon, t);
        }
        else if (time < 0.7f)
        {
            return sunColorNoon;
        }
        else if (time < 0.8f)
        {
            float t = Mathf.InverseLerp(0.7f, 0.8f, time);
            return Color.Lerp(sunColorNoon, sunColorEvening, t);
        }
        else
        {
            return sunColorEvening;
        }
    }

    // summary:
    // Updates the ambient light color based on the current time.
    void UpdateAmbientLight()
    {
        float dayFactor = 0f;

        if (currentTime > 0.2f && currentTime < 0.8f)
            dayFactor = 1f;
        else if (currentTime >= 0.1f && currentTime <= 0.2f)
            dayFactor = Mathf.InverseLerp(0.1f, 0.2f, currentTime);
        else if (currentTime >= 0.8f && currentTime <= 0.9f)
            dayFactor = 1f - Mathf.InverseLerp(0.8f, 0.9f, currentTime);

        Color ambientColor = Color.Lerp(ambientNightColor, ambientDayColor, dayFactor);
        RenderSettings.ambientLight = ambientColor * ambientIntensity;
    }

    // summary:
    // Updates the fog settings based on the current time.
    void UpdateFog()
    {
        float dayFactor = 0f;

        if (currentTime > 0.2f && currentTime < 0.8f)
            dayFactor = 1f;
        else if (currentTime >= 0.1f && currentTime <= 0.2f)
            dayFactor = Mathf.InverseLerp(0.1f, 0.2f, currentTime);
        else if (currentTime >= 0.8f && currentTime <= 0.9f)
            dayFactor = 1f - Mathf.InverseLerp(0.8f, 0.9f, currentTime);

        Color fogColor = Color.Lerp(fogNightColor, fogDayColor, dayFactor);
        RenderSettings.fogColor = fogColor;
    }

    // summary:
    // Updates the skybox material properties based on the current time.
    void UpdateSkybox()
    {
        Color targetColor = CalculateSkyColor(currentTime);
        currentSkyboxColor = Color.Lerp(currentSkyboxColor, targetColor, Time.deltaTime * skyboxBlendSpeed);

        float intensity = CalculateSkyIntensity(currentTime);

        if (skyboxMaterial.HasProperty("_Tint"))
            skyboxMaterial.SetColor("_Tint", currentSkyboxColor);

        if (skyboxMaterial.HasProperty("_Exposure"))
            skyboxMaterial.SetFloat("_Exposure", intensity);

        if (skyboxMaterial.HasProperty("_SkyTint"))
            skyboxMaterial.SetColor("_SkyTint", currentSkyboxColor);

        if (skyboxMaterial.HasProperty("_AtmosphereThickness"))
        {
            float atmosphere = Mathf.Lerp(1.8f, 1.0f, intensity);
            skyboxMaterial.SetFloat("_AtmosphereThickness", atmosphere);
        }
    }

    // summary:
    // Calculates the sky color based on the current time.
    // This method interpolates between different sky colors for night, morning, noon, evening, and night again.
    Color CalculateSkyColor(float time)
    {
        if (time < 0.2f)
            return skyColorNight;
        else if (time < 0.3f)
            return Color.Lerp(skyColorNight, skyColorMorning, Mathf.InverseLerp(0.2f, 0.3f, time));
        else if (time < 0.4f)
            return Color.Lerp(skyColorMorning, skyColorNoon, Mathf.InverseLerp(0.3f, 0.4f, time));
        else if (time < 0.6f)
            return skyColorNoon;
        else if (time < 0.7f)
            return skyColorNoon;
        else if (time < 0.8f)
            return Color.Lerp(skyColorNoon, skyColorEvening, Mathf.InverseLerp(0.7f, 0.8f, time));
        else if (time < 0.9f)
            return Color.Lerp(skyColorEvening, skyColorNight, Mathf.InverseLerp(0.8f, 0.9f, time));
        else
            return skyColorNight;
    }

    // summary:
    // Calculates the sky intensity based on the current time.
    // This method uses an animation curve to determine the intensity during different times of day.
    // It returns a value that blends between day and night intensities.
    float CalculateSkyIntensity(float time)
    {
        float dayFactor = 0f;

        if (time > 0.25f && time < 0.75f)
            dayFactor = 1f;
        else if (time >= 0.2f && time <= 0.25f)
            dayFactor = skyIntensityCurve.Evaluate(Mathf.InverseLerp(0.2f, 0.25f, time));
        else if (time >= 0.75f && time <= 0.8f)
            dayFactor = skyIntensityCurve.Evaluate(1f - Mathf.InverseLerp(0.75f, 0.8f, time));

        return Mathf.Lerp(skyIntensityNight, skyIntensityDay, dayFactor);
    }

    // summary:
    // Returns a human-readable string representation of the current time in HH:MM format.
    public string GetReadableTime()
    {
        float hours = currentTime * 24f;
        int hour = Mathf.FloorToInt(hours);
        int minutes = Mathf.FloorToInt((hours - hour) * 60f);
        return $"{hour:D2}:{minutes:D2}";
    }

    // summary:
    // Sets the current time of day based on a normalized value between 0 and 1.
    // This method clamps the new time value to ensure it stays within the valid range.
    public void SetTime(float newTime)
    {
        currentTime = Mathf.Clamp01(newTime);
        UpdateLighting();
    }

    // summary:
    // Sets the current time of day based on hour and minute values.
    public void SetTime(int hour, int minute)
    {
        float time = (hour + minute / 60f) / 24f;
        SetTime(time);
    }

    public float GetCurrentTime() => currentTime; // Returns the current time as a normalized value (0 to 1).
    public bool IsDay() => currentTime > 0.2f && currentTime < 0.8f; // Returns true if it is daytime (between 20% and 80% of the day cycle).
    public bool IsNight() => !IsDay(); // Returns true if it is nighttime (not daytime).

    [ContextMenu("Set Sunrise")]
    void SetSunrise() => SetTime(0.25f);    // Sets the time to sunrise (25% of the day cycle).

    [ContextMenu("Set Noon")]
    void SetNoon() => SetTime(0.5f);    // Sets the time to noon (50% of the day cycle).

    [ContextMenu("Set Sunset")]
    void SetSunset() => SetTime(0.75f); // Sets the time to sunset (75% of the day cycle).

    [ContextMenu("Set Midnight")]
    void SetMidnight() => SetTime(0f);  // Sets the time to midnight (0% of the day cycle).

    // summary:
    // Toggles all lanterns in the scene based on the specified state.
    // This method finds all Lantern objects in the scene and sets their state to either on or off.
    void ToggleAllLanterns(bool turnOn)
    {
        Lantern[] allLanterns = Object.FindObjectsByType<Lantern>(FindObjectsSortMode.None);
        foreach (Lantern lantern in allLanterns)
        {
            lantern.SetLanternState(turnOn);
        }
    }
}