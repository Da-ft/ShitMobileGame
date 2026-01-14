using UnityEngine;

public class AutoMiner : MinerBase
{
    private float timer;

    [HideInInspector] public float currentAttackInterval; // <-- NEU

    // Wir überschreiben Start, um auch den Interval zu initialisieren
    protected override void Start()
    {
        base.Start(); // Wichtig, damit MinerBase.Start läuft
        if (currentAttackInterval == 0) currentAttackInterval = stats.attackInterval;
    }

    void Update()
    {
        if (stats == null) return;

        timer += Time.deltaTime;
        // Hier nutzen wir jetzt currentAttackInterval
        if (timer >= currentAttackInterval)
        {
            PerformMining();
            timer = 0;
        }
    }
}