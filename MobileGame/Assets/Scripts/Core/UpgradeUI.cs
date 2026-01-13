using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public TextMeshProUGUI costText;
    public Button upgradeButton;

    void Update()
    {
        double cost = upgradeManager.laserDamageUpgrade.GetCost(upgradeManager.laserLevel);
        costText.text = $"Upgrade Laser: {cost:F0} $";

        // Button deaktivieren, wenn nicht genug Geld
        upgradeButton.interactable = ResourceManager.Instance.currentResources >= cost;
    }
}