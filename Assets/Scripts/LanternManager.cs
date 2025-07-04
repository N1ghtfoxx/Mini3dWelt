using UnityEngine;

public class LanternManager : MonoBehaviour
{
    [Header("Laternen Prefabs")]
    [SerializeField] private GameObject laterneAnPrefab; // Dein Prefab für Laterne AN
    [SerializeField] private GameObject laterneAusPrefab; // Dein Prefab für Laterne AUS

    [Header("Laternen Positionen")]
    [SerializeField] private Transform[] laternenPositionen; // Wo die Laternen stehen sollen

    private GameObject[] aktuelleLaternen; // Die aktuell gespawnten Laternen
    private bool laternenAktiviert = false; // Sind die Laternen gerade an?

    void Start()
    {
        // Beim Start alle Laternen ausgeschaltet spawnen
        SpawnLaternen(false);
    }

    public void SetLaternen(bool anschalten)
    {
        // Nur ändern wenn sich der Status ändert
        if (anschalten != laternenAktiviert)
        {
            laternenAktiviert = anschalten;
            SpawnLaternen(anschalten);
        }
    }

    void SpawnLaternen(bool anschalten)
    {
        // Alte Laternen löschen
        if (aktuelleLaternen != null)
        {
            for (int i = 0; i < aktuelleLaternen.Length; i++)
            {
                if (aktuelleLaternen[i] != null)
                {
                    DestroyImmediate(aktuelleLaternen[i]);
                }
            }
        }

        // Neue Laternen spawnen
        if (laternenPositionen != null && laternenPositionen.Length > 0)
        {
            aktuelleLaternen = new GameObject[laternenPositionen.Length];

            // Welches Prefab soll verwendet werden?
            GameObject prefabZuVerwenden = anschalten ? laterneAnPrefab : laterneAusPrefab;

            if (prefabZuVerwenden != null)
            {
                // Für jede Position eine Laterne spawnen
                for (int i = 0; i < laternenPositionen.Length; i++)
                {
                    if (laternenPositionen[i] != null)
                    {
                        aktuelleLaternen[i] = Instantiate(prefabZuVerwenden,
                            laternenPositionen[i].position,
                            laternenPositionen[i].rotation);
                        aktuelleLaternen[i].transform.SetParent(laternenPositionen[i]);
                    }
                }
            }
        }
    }

    // Methode um neue Laternen-Positionen hinzuzufügen
    public void AddLaternenPosition(Transform position)
    {
        if (laternenPositionen == null)
        {
            laternenPositionen = new Transform[1];
            laternenPositionen[0] = position;
        }
        else
        {
            System.Array.Resize(ref laternenPositionen, laternenPositionen.Length + 1);
            laternenPositionen[laternenPositionen.Length - 1] = position;
        }

        // Laternen neu spawnen
        SpawnLaternen(laternenAktiviert);
    }
}