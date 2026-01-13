using UnityEngine;

public abstract class MinerBase : MonoBehaviour
{
    public MiningStats stats;
    public Asteroid targetAsteroid;

    protected void PerformMining()
    {
        if (targetAsteroid == null) return;

        bool isCrit = Random.value <= stats.critRate;
        float damage = isCrit ? stats.baseDamage * stats.critMultiplier : stats.baseDamage;

        targetAsteroid.TakeDamage(damage);
    }
}