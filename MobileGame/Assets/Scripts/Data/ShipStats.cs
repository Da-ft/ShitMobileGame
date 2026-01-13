using UnityEngine;

[CreateAssetMenu(fileName = "NewShipStats", menuName = "SpaceMiner/ShipStats")]
public class ShipStats : ScriptableObject
{
    public float speed = 5f;
    public double capacity = 100;
    public float loadDuration = 2f; // Zeit zum Beladen am Asteroiden
}