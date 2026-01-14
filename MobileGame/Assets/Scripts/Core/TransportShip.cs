using UnityEngine;
using System.Collections;

public class TransportShip : MonoBehaviour
{
    [Header("References")]
    public ShipStats baseStats;
    public Asteroid targetAsteroid;
    public Transform homeBase;

    [Header("Live Stats (Read Only)")]
    public float currentSpeed;
    public float currentCapacity;

    private float currentLoad;
    private bool isWorking = false;

    [HideInInspector] public float currentLoadDuration;

    // Wird vom UpgradeManager initialisiert (oder Fallback im Start)
    private void Start()
    {
        if (currentSpeed == 0) currentSpeed = baseStats.speed;
        if (currentCapacity == 0) currentCapacity = baseStats.capacity;
        if (currentLoadDuration == 0) currentLoadDuration = baseStats.loadDuration;
    }

    private void Update()
    {
        // Wenn nicht gearbeitet wird UND Ressourcen da sind -> Losfliegen
        if (!isWorking && targetAsteroid != null && targetAsteroid.pendingResources > 0)
        {
            StartCoroutine(TransportLoop());
        }
    }

    private IEnumerator TransportLoop()
    {
        isWorking = true;

        // 1. Hinfliegen
        yield return StartCoroutine(MoveTo(targetAsteroid.transform.position));

        // 2. Beladen (Visuelles Warten)
        yield return new WaitForSeconds(currentLoadDuration);

        // Logik: Wie viel können wir nehmen?
        // Entweder alles was da ist, ODER maximal unsere Kapazität
        float amountToLoad = System.Math.Min(targetAsteroid.pendingResources, currentCapacity);

        // Ressourcen vom Asteroiden abziehen
        targetAsteroid.pendingResources -= amountToLoad;
        currentLoad = amountToLoad;

        // 3. Zurückfliegen
        yield return StartCoroutine(MoveTo(homeBase.position));

        // 4. Entladen
        ResourceManager.Instance.AddResources(currentLoad);
        currentLoad = 0;

        isWorking = false;
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        // Schiff in Flugrichtung drehen
        transform.LookAt(destination);

        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, currentSpeed * Time.deltaTime);
            yield return null;
        }
    }
}