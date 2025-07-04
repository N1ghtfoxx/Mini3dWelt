using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Zeit-Einstellungen")]
    [SerializeField] private float tagDauer = 240f; // Wie lange ein Tag dauert (in Sekunden)
    [SerializeField] private float aktuelleZeit = 12f; // Startzeit (12 = Mittag)

    [Header("Sonne")]
    [SerializeField] private Light sonne; // Dein Hauptlicht (Directional Light)
    [SerializeField] private Gradient sonnenfarbe; // Farbe der Sonne über den Tag
    [SerializeField] private AnimationCurve sonnenintensitaet; // Wie hell die Sonne ist

    [Header("Mond")]
    [SerializeField] private Light mond; // Zweites Licht für Mondschein
    [SerializeField] private Color mondfarbe = Color.white; // Mondfarbe (bläulich-weiß)
    [SerializeField] private AnimationCurve mondintensitaet; // Wie hell der Mond ist

    [Header("Himmel & Ambiente")]
    [SerializeField] private Material himmelMaterial; // Skybox Material
    [SerializeField] private Gradient himmelfarbe; // Himmelfarbe über den Tag
    [SerializeField] private Color ambientTagfarbe = Color.white; // Umgebungslicht am Tag
    [SerializeField] private Color ambientNachtfarbe = Color.blue; // Umgebungslicht in der Nacht
    [SerializeField] private AnimationCurve ambientIntensitaet; // Stärke des Umgebungslichts

    [Header("Nebel")]
    [SerializeField] private bool nebelAktiv = true;
    [SerializeField] private Gradient nebelfarbe;
    [SerializeField] private AnimationCurve nebeldichte;

    [Header("Laternen-Einstellungen")]
    [SerializeField] private float laternenAnZeit = 19f; // 19:00 Uhr - Laternen gehen an
    [SerializeField] private float laternenAusZeit = 6f; // 6:00 Uhr - Laternen gehen aus

    private LanternManager laternenManager;
    private bool laternenAn = false;

    // ...

    void Start()
    {
        // LaternenManager suchen
        laternenManager = FindAnyObjectByType<LanternManager>();
        if (laternenManager == null)
        {
            Debug.LogWarning("LaternenManager nicht gefunden!");
        }

        // Mond-Licht konfigurieren falls vorhanden
        if (mond != null)
        {
            mond.color = mondfarbe;
            mond.type = LightType.Directional;
        }

        // Nebel aktivieren falls gewünscht
        if (nebelAktiv)
        {
            RenderSettings.fog = true;
        }

        // Initiale Einstellungen
        UpdateLaternen();
    }

    void Update()
    {
        // Zeit läuft weiter
        aktuelleZeit += (24f / tagDauer) * Time.deltaTime;

        // Tag zurücksetzen wenn 24 Stunden erreicht
        if (aktuelleZeit >= 24f)
        {
            aktuelleZeit = 0f;
        }

        // Alles aktualisieren
        UpdateSonne();
        UpdateMond();
        UpdateHimmel();
        UpdateAmbientLight();
        UpdateNebel();
        UpdateLaternen();
    }

    void UpdateSonne()
    {
        if (sonne == null) return;

        // Sonnenposition berechnen
        float sonnenWinkel = (aktuelleZeit / 24f) * 360f - 90f;
        sonne.transform.rotation = Quaternion.Euler(sonnenWinkel, 30f, 0f);

        // Sonnenintensität und -farbe
        float normalisiertZeit = aktuelleZeit / 24f;
        sonne.intensity = sonnenintensitaet.Evaluate(normalisiertZeit);
        sonne.color = sonnenfarbe.Evaluate(normalisiertZeit);

        // Sonne komplett ausschalten wenn sie unter dem Horizont ist
        if (sonnenWinkel < -90f || sonnenWinkel > 90f)
        {
            sonne.intensity = 0f;
        }
    }

    void UpdateMond()
    {
        if (mond == null) return;

        // Mond ist gegenüber der Sonne (180° versetzt)
        float mondWinkel = (aktuelleZeit / 24f) * 360f - 90f + 180f;
        mond.transform.rotation = Quaternion.Euler(mondWinkel, 30f, 0f);

        // Mondintensität
        float normalisiertZeit = aktuelleZeit / 24f;
        mond.intensity = mondintensitaet.Evaluate(normalisiertZeit);
        mond.color = mondfarbe;

        // Mond nur sichtbar wenn er über dem Horizont ist
        if (mondWinkel < -90f || mondWinkel > 90f)
        {
            mond.intensity = 0f;
        }
    }

    void UpdateHimmel()
    {
        if (himmelMaterial == null) return;

        float normalisiertZeit = aktuelleZeit / 24f;
        Color himmelsfarbe = himmelfarbe.Evaluate(normalisiertZeit);

        // Verschiedene Skybox-Eigenschaften setzen
        if (himmelMaterial.HasProperty("_Tint"))
        {
            himmelMaterial.SetColor("_Tint", himmelsfarbe);
        }

        // Für Procedural Skybox
        if (himmelMaterial.HasProperty("_SkyTint"))
        {
            himmelMaterial.SetColor("_SkyTint", himmelsfarbe);
        }

        // Für 6-Sided Skybox
        if (himmelMaterial.HasProperty("_TintColor"))
        {
            himmelMaterial.SetColor("_TintColor", himmelsfarbe);
        }
    }

    void UpdateAmbientLight()
    {
        float normalisiertZeit = aktuelleZeit / 24f;

        // Umgebungslichtfarbe zwischen Tag und Nacht interpolieren
        Color ambientFarbe = Color.Lerp(ambientNachtfarbe, ambientTagfarbe,
            ambientIntensitaet.Evaluate(normalisiertZeit));

        // Umgebungslicht setzen
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = ambientFarbe;

        // Oder für Gradient/Skybox Ambient:
        // RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        // RenderSettings.ambientSkyColor = ambientFarbe;
        // RenderSettings.ambientEquatorColor = ambientFarbe * 0.5f;
        // RenderSettings.ambientGroundColor = ambientFarbe * 0.2f;
    }

    void UpdateNebel()
    {
        if (!nebelAktiv) return;

        float normalisiertZeit = aktuelleZeit / 24f;

        // Nebelfarbe
        if (nebelfarbe != null)
        {
            RenderSettings.fogColor = nebelfarbe.Evaluate(normalisiertZeit);
        }

        // Nebeldichte
        if (nebeldichte != null)
        {
            RenderSettings.fogDensity = nebeldichte.Evaluate(normalisiertZeit);
        }

        // Nebel-Modus (exponentiell sieht meist besser aus)
        RenderSettings.fogMode = FogMode.ExponentialSquared;
    }

    void UpdateLaternen()
    {
        if (laternenManager == null) return;

        bool solltenLaternenAn = SolltenLaternenAnSein();

        if (solltenLaternenAn != laternenAn)
        {
            laternenAn = solltenLaternenAn;
            laternenManager.SetLaternen(laternenAn);
        }
    }

    bool SolltenLaternenAnSein()
    {
        // Laternen sind an zwischen laternenAnZeit und laternenAusZeit
        if (laternenAnZeit > laternenAusZeit) // z.B. 19:00 bis 6:00
        {
            return aktuelleZeit >= laternenAnZeit || aktuelleZeit <= laternenAusZeit;
        }
        else
        {
            return aktuelleZeit >= laternenAnZeit && aktuelleZeit <= laternenAusZeit;
        }
    }

    // Hilfsmethoden
    public float GetAktuelleZeit()
    {
        return aktuelleZeit;
    }

    public void SetZeit(float neueZeit)
    {
        aktuelleZeit = Mathf.Clamp(neueZeit, 0f, 24f);
    }

    public string GetZeitString()
    {
        int stunden = Mathf.FloorToInt(aktuelleZeit);
        int minuten = Mathf.FloorToInt((aktuelleZeit - stunden) * 60f);
        return string.Format("{0:00}:{1:00}", stunden, minuten);
    }

    public bool IstNacht()
    {
        return laternenAn;
    }

    public bool IstTag()
    {
        return aktuelleZeit >= 6f && aktuelleZeit <= 18f;
    }

    public string GetTageszeit()
    {
        if (aktuelleZeit >= 5f && aktuelleZeit < 12f) return "Morgen";
        if (aktuelleZeit >= 12f && aktuelleZeit < 17f) return "Mittag";
        if (aktuelleZeit >= 17f && aktuelleZeit < 21f) return "Abend";
        return "Nacht";
    }
}