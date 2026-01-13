using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

    public UpgradeData laserDamageUpgrade;
    public int laserLevel = 0;
    public AutoMiner autoMiner;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        LoadGame();
    }

    private void LoadGame()
    {
        SaveData data = SaveManager.Instance.Load();
        if (data != null)
        {
            ResourceManager.Instance.currentResources = data.money;

            // Listen zurück ins Dictionary laden
            upgradeLevels.Clear();
            for (int i = 0; i < data.upgradeKeys.Count; i++)
            {
                upgradeLevels[data.upgradeKeys[i]] = data.upgradeValues[i];
            }

            // TODO: durch alle Upgrades loopen und ApplyUpgrades() rufen
        }
    }

    public void BuyLaserUpgrade()
    {
        double cost = laserDamageUpgrade.GetCost(laserLevel);

        if (ResourceManager.Instance.currentResources >= cost)
        {
            ResourceManager.Instance.currentResources -= cost;
            laserLevel++;

            // Level auch im Dictionary aktuell halten!
            upgradeLevels[laserDamageUpgrade.upgradeName] = laserLevel;

            ApplyUpgrades();

            SaveManager.Instance.Save(ResourceManager.Instance.currentResources, upgradeLevels);
        }
    }

    private void ApplyUpgrades()
    {
        // Wir berechnen den neuen Schaden: Basis + (Level * Bonus)
        autoMiner.stats.baseDamage += laserDamageUpgrade.bonusPerLevel;
        Debug.Log($"Upgrade gekauft! Level: {laserLevel}, Neuer Schaden: {autoMiner.stats.baseDamage}");
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveManager.Instance.Save(ResourceManager.Instance.currentResources, upgradeLevels);
        }
    }

    public void OnUpgradePurchased(UpgradeData data, int newLevel)
    {
        // Level im Dictionary aktualisieren
        upgradeLevels[data.upgradeName] = newLevel;

        // Logik anwenden (Beispiel für Laser)
        if (data.upgradeName == "Laser Power")
        {
            autoMiner.stats.baseDamage = data.baseValue + (newLevel * data.bonusPerLevel);
        }

        // Speichern!
        SaveManager.Instance.Save(ResourceManager.Instance.currentResources, upgradeLevels);
    }

    public int GetLevelForUpgrade(UpgradeData data)
    {
        if (upgradeLevels.ContainsKey(data.upgradeName))
        {
            return upgradeLevels[data.upgradeName];
        }
        return 0;
    }
}