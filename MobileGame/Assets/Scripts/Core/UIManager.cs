using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI pendingText;

    [Header("Data References")]
    public Asteroid targetAsteroid;

    void Update()
    {
        // Anzeige der Bank-Ressourcen (ResourceManager Singleton)
        if (ResourceManager.Instance != null)
        {
            currencyText.text = $"Bank: {ResourceManager.Instance.currentResources:F0} $";
        }

        // Anzeige der Ressourcen am Asteroiden
        if (targetAsteroid != null)
        {
            pendingText.text = $"Pending: {targetAsteroid.pendingResources:F0}";
        }
    }
}