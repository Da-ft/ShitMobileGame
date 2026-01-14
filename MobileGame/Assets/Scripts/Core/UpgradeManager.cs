using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

    [Header("Mining Setup")]
    public GameObject minerPrefab;
    public Transform[] minerSlots;
    public List<AutoMiner> activeMiners = new List<AutoMiner>();
    public MiningStats defaultMiningStats;

    [Header("Mining Data")]
    public UpgradeData buyLaserUpgrade;
    public UpgradeData laserDamageUpgrade;
    public UpgradeData laserSpeedUpgrade;
    public UpgradeData laserCritUpgrade;

    [Header("Logistics Setup")]
    public GameObject shipPrefab;
    public ShipStats defaultShipStats;
    public Transform shipSpawnPoint;
    public List<TransportShip> activeShips = new List<TransportShip>();

    [Header("Logistics Data")]
    public UpgradeData buyShipUpgrade;
    public UpgradeData shipSpeedUpgrade;
    public UpgradeData shipCapacityUpgrade;
    public UpgradeData shipLoadSpeedUpgrade;

    [Header("Globals")]
    public Asteroid targetAsteroid;
    public Transform homeBase;

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
        activeMiners.Clear();
        activeShips.Clear();

        SaveData data = SaveManager.Instance.Load();
        if (data != null)
        {
            ResourceManager.Instance.currentResources = data.money;
            upgradeLevels.Clear();
            for (int i = 0; i < data.upgradeKeys.Count; i++)
                upgradeLevels[data.upgradeKeys[i]] = data.upgradeValues[i];
        }

        RecalculateAllStats();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveManager.Instance.Save(ResourceManager.Instance.currentResources, upgradeLevels);
    }

    public void OnUpgradePurchased(UpgradeData data, int newLevel)
    {
        upgradeLevels[data.upgradeName] = newLevel;

        // Statt nur SingleStat aufzurufen, rufe zur Sicherheit die spezifischen Update-Methoden auf, die ALLES abdecken, verhindert "vergessene" Stats

        if (data.upgradeName.Contains("Laser") || data.upgradeName.Contains("Miner"))
        {
            // Erst Menge checken (falls es ein Kauf-Upgrade war)
            UpdateMinerCount();
            // Dann Stats auf ALLE anwenden
            foreach (var miner in activeMiners) ApplyMinerStats(miner);
        }
        else if (data.upgradeName.Contains("Ship"))
        {
            UpdateShipCount();
            foreach (var ship in activeShips) ApplyShipStats(ship);
        }

        SaveManager.Instance.Save(ResourceManager.Instance.currentResources, upgradeLevels);
    }

    public int GetLevelForUpgrade(UpgradeData data)
    {
        if (data == null) return 0;
        if (upgradeLevels.ContainsKey(data.upgradeName)) return upgradeLevels[data.upgradeName];
        return 0;
    }

    // Geht durch alle gespeicherten Upgrades und wendet sie an
    private void RecalculateAllStats()
    {
        // 1. Sicherheits-Inits für Stats / Startbedingungen setzen
        // --Laser--
        InitializeIfMissing(laserDamageUpgrade, 0);
        InitializeIfMissing(laserSpeedUpgrade, 0);
        InitializeIfMissing(laserCritUpgrade, 0);

        // --Ship--
        InitializeIfMissing(buyShipUpgrade, 1);
        InitializeIfMissing(shipSpeedUpgrade, 0);
        InitializeIfMissing(shipCapacityUpgrade, 0);
        InitializeIfMissing(shipLoadSpeedUpgrade, 0);

        // 2. Erst spawnen...
        UpdateMinerCount();
        UpdateShipCount();

        // 3. ...dann Werte updaten
        foreach (var miner in activeMiners) ApplyMinerStats(miner);
        foreach (var ship in activeShips) ApplyShipStats(ship);
    }

    private void InitializeIfMissing(UpgradeData data, int defaultLevel)
    {
        if (data != null && !upgradeLevels.ContainsKey(data.upgradeName))
        {
            upgradeLevels[data.upgradeName] = defaultLevel;
        }
    }

    private void UpdateMinerCount()
    {
        int targetCount = GetLevelForUpgrade(buyLaserUpgrade);

        while (activeMiners.Count < targetCount)
        {
            int slotIndex = activeMiners.Count % minerSlots.Length;
            Transform slot = minerSlots[slotIndex];

            GameObject go = Instantiate(minerPrefab, slot.position, slot.rotation, slot);
            AutoMiner miner = go.GetComponent<AutoMiner>();

            miner.targetAsteroid = targetAsteroid;
            miner.stats = defaultMiningStats;

            // aktuelle Stats draufrechnen!
            ApplyMinerStats(miner);

            activeMiners.Add(miner);
        }
    }

    private void UpdateShipCount()
    {
        int targetCount = GetLevelForUpgrade(buyShipUpgrade);

        while (activeShips.Count < targetCount)
        {
            GameObject go = Instantiate(shipPrefab, shipSpawnPoint.position, Quaternion.identity);
            TransportShip ship = go.GetComponent<TransportShip>();

            ship.targetAsteroid = targetAsteroid;
            ship.homeBase = homeBase;
            ship.baseStats = defaultShipStats;

            // aktuelle Stats draufrechnen!
            ApplyShipStats(ship);

            activeShips.Add(ship);
        }
    }

    private void ApplyMinerStats(AutoMiner miner)
    {
        // 1. Levels holen
        int dmgLvl = GetLevelForUpgrade(laserDamageUpgrade);
        int spdLvl = GetLevelForUpgrade(laserSpeedUpgrade);
        int critLvl = GetLevelForUpgrade(laserCritUpgrade);

        // 2. Werte berechnen
        float dmg = laserDamageUpgrade.baseValue + (dmgLvl * laserDamageUpgrade.bonusPerLevel);

        // Speed (Darf nicht < 0.1 sein)
        float spd = Mathf.Max(0.1f, laserSpeedUpgrade.baseValue - (spdLvl * laserSpeedUpgrade.bonusPerLevel));

        float crit = laserCritUpgrade.baseValue + (critLvl * laserCritUpgrade.bonusPerLevel);

        // 3. Anwenden
        miner.currentDamage = dmg;
        miner.currentAttackInterval = spd;
        miner.currentCritRate = Mathf.Clamp01(crit);
    }

    private void ApplyShipStats(TransportShip ship)
    {
        int spdLvl = GetLevelForUpgrade(shipSpeedUpgrade);
        int capLvl = GetLevelForUpgrade(shipCapacityUpgrade);
        int loadLvl = GetLevelForUpgrade(shipLoadSpeedUpgrade);

        float speed = shipSpeedUpgrade.baseValue + (spdLvl * shipSpeedUpgrade.bonusPerLevel);
        float cap = shipCapacityUpgrade.baseValue + (capLvl * shipCapacityUpgrade.bonusPerLevel);
        float load = Mathf.Max(0.1f, shipLoadSpeedUpgrade.baseValue - (loadLvl * shipLoadSpeedUpgrade.bonusPerLevel));

        ship.currentSpeed = speed;
        ship.currentCapacity = cap;
        ship.currentLoadDuration = load;
    }
}
