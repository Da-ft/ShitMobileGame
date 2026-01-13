using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance; // Singleton

    public double currentResources;

    private void Awake()
    {
        Instance = this;
    }

    public void AddResources(double amount)
    {
        currentResources += amount;
        Debug.Log($"Bank-Kontostand: {currentResources}");
    }
}