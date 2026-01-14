using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserVisuals : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Wie lange der Laserstrahl sichtbar bleibt (in Sekunden)")]
    public float beamDuration = 0.1f;

    [ColorUsage(true, true)]
    public Color normalColor = Color.green;
    [ColorUsage(true, true)]
    public Color critColor = Color.cyan;

    [Header("References")]
    public Transform firePoint;

    private LineRenderer line;
    private Coroutine currentBeamRoutine;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false; // Am Anfang ausschalten

        // Sicherstellen, dass wir im World Space arbeiten, damit die Ziele korrekt sind
        line.useWorldSpace = true;
        line.positionCount = 2;
    }

    // Diese Methode wird vom Miner aufgerufen
    public void FireBeam(Transform target, bool isCrit)
    {
        if (target == null || firePoint == null) return;

        // Falls gerade noch ein alter Strahl läuft, stoppen wir ihn, um Flackern zu vermeiden
        if (currentBeamRoutine != null) StopCoroutine(currentBeamRoutine);

        currentBeamRoutine = StartCoroutine(ShowBeamRoutine(target, isCrit));
    }

    private IEnumerator ShowBeamRoutine(Transform target, bool isCrit)
    {
        // 1. Farbe setzen
        Color baseColor = isCrit ? critColor : normalColor;
        line.startColor = baseColor;
        line.endColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0.5f);

        // 2. Positionen setzen
        line.SetPosition(0, firePoint.position);
        line.SetPosition(1, target.position);

        // 3. Einschalten
        line.enabled = true;

        // 4. Kurz warten
        yield return new WaitForSeconds(beamDuration);

        // 5. Ausschalten
        line.enabled = false;
        currentBeamRoutine = null;
    }
}