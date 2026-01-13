using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public double pendingResources; // Ressourcen, die auf Abholung warten

    public void TakeDamage(float amount)
    {
        pendingResources += amount;
        // TODO: Spawn floatingText(amount)
    }
}