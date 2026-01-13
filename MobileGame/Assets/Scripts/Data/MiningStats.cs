using UnityEngine;

[CreateAssetMenu(fileName = "NewMiningStats", menuName = "SpaceMiner/MiningStats")]
public class MiningStats : ScriptableObject
{
    public float baseDamage = 10f;
    public float attackInterval = 1f; // Zeit zwischen Laser-Schüssen
    [Range(0, 1)] public float critRate = 0.1f;
    public float critMultiplier = 2f;
}