using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UpgradeUIElement : MonoBehaviour
{
    [Header("UI Fields")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public Button buyButton;

    private UpgradeData data;
    private int currentLevel;

    public void Setup(UpgradeData upgradeData, int level)
    {
        data = upgradeData;
        currentLevel = level;
        RefreshText();
    }

    void Update()
    {
        if (data == null) return;

        double cost = data.GetCost(currentLevel);

        if (ResourceManager.Instance != null)
        {
            buyButton.interactable = ResourceManager.Instance.currentResources >= cost;
        }
    }

    public void RefreshText()
    {
        if (data == null) return;
        double cost = data.GetCost(currentLevel);

        titleText.text = data.upgradeName;
        levelText.text = $"Lvl {currentLevel}";
        costText.text = $"{cost:F0} $";
    }

    public void OnBuyClicked()
    {
        Debug.Log("Button wurde geklickt!");

        double cost = data.GetCost(currentLevel);
        if (ResourceManager.Instance.currentResources >= cost)
        {
            ResourceManager.Instance.currentResources -= cost;
            currentLevel++;

            RefreshText(); // Texte aktualisieren (neuer Preis, neues Level)

            // Logik an Manager senden
            UpgradeManager.Instance.OnUpgradePurchased(data, currentLevel);
        }
    }
}