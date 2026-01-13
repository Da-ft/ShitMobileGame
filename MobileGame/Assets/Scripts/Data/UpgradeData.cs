using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "SpaceMiner/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public double baseCost = 10;
    public double costMultiplier = 1.15;

    [Header("Effect Settings")]
    public float baseValue = 10f;
    public float bonusPerLevel = 5f;

    public double GetCost(int level)
    {
        // Formel: BaseCost * (Multiplier ^ Level)
        return baseCost * Mathf.Pow((float)costMultiplier, level);
    }
}