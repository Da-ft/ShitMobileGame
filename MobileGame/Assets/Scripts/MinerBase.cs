using UnityEngine;

public abstract class MinerBase : MonoBehaviour
{
    public MiningStats stats;
    public Asteroid targetAsteroid;

    [Header("Visuals")]
    public LaserVisuals laserVisuals;

    [HideInInspector] public float currentDamage;
    [HideInInspector] public float currentCritRate;

    protected virtual void Start()
    {
        // Falls der Manager nicht initialisiert hat (Fallback)
        if (currentDamage == 0 && stats != null) currentDamage = stats.baseDamage;
        if (currentCritRate == 0 && stats != null) currentCritRate = stats.critRate;
    }

    protected void PerformMining()
    {
        if (targetAsteroid == null) return;

        // Crit berechnen
        bool isCrit = Random.value <= currentCritRate;
        float finalDamage = isCrit ? currentDamage * stats.critMultiplier : currentDamage;

        // Schaden zufügen
        targetAsteroid.TakeDamage(finalDamage);

        // Visuals triggern
        if (laserVisuals != null)
        {
            laserVisuals.FireBeam(targetAsteroid.transform, isCrit);
        }
    }
}