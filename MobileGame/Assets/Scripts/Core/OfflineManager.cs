using System;
using UnityEngine;

public class OfflineManager : MonoBehaviour
{
    public void CalculateOfflineEarnings(SaveData data)
    {
        if (string.IsNullOrEmpty(data.lastSaveTime)) return;

        // 1. Zeitdifferenz berechnen
        long temp = Convert.ToInt64(data.lastSaveTime);
        DateTime lastTime = DateTime.FromBinary(temp);
        TimeSpan difference = DateTime.Now - lastTime;
        float secondsOffline = (float)difference.TotalSeconds;

        // 2. Ertrag pro Sekunde ermitteln
        float damagePerSecond = CalculateCurrentDPS();
        double earned = secondsOffline * damagePerSecond;

        // 3. Gutschrift
        if (earned > 0)
        {
            ResourceManager.Instance.AddResources(earned);
            Debug.Log($"Du warst {secondsOffline:F0}s weg und hast {earned:F0} $ verdient!");
            // TODO: Popup Window for earnings!
        }
    }

    private float CalculateCurrentDPS()
    {
        // Logik: Schaden / Intervall (z.B. 10 Schaden alle 2 Sek = 5 DPS)
        var stats = UpgradeManager.Instance.autoMiner.stats;
        return stats.baseDamage / stats.attackInterval;
    }
}