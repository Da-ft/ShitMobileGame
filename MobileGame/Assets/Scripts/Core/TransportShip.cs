using UnityEngine;
using System.Collections;

public class TransportShip : MonoBehaviour
{
    public ShipStats stats;
    public Asteroid targetAsteroid;
    public Transform homeBase;

    private double cargo;
    private bool isWorking = false;

    private void Update()
    {
        if (!isWorking && targetAsteroid != null && targetAsteroid.pendingResources > 0)
        {
            StartCoroutine(TransportLoop());
        }
    }

    private IEnumerator TransportLoop()
    {
        isWorking = true;

        // 1. Flug zum Asteroiden
        yield return StartCoroutine(MoveTo(targetAsteroid.transform.position));

        // 2. Beladen
        double amountToLoad = Mathf.Min((float)targetAsteroid.pendingResources, (float)stats.capacity);
        targetAsteroid.pendingResources -= amountToLoad;
        cargo = amountToLoad;
        yield return new WaitForSeconds(stats.loadDuration);

        // 3. Flug zur Basis
        yield return StartCoroutine(MoveTo(homeBase.position));

        // 4. Entladen
        ResourceManager.Instance.AddResources(cargo);
        cargo = 0;

        isWorking = false;
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, stats.speed * Time.deltaTime);
            yield return null;
        }
    }
}