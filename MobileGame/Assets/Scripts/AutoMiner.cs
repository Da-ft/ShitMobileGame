using UnityEngine;

public class AutoMiner : MinerBase
{
    private float timer;

    void Update()
    {
        if (stats == null) return;

        timer += Time.deltaTime;
        if (timer >= stats.attackInterval)
        {
            PerformMining();
            timer = 0;
        }
    }
}