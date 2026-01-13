using UnityEngine;
using System.Collections.Generic;

public class UpgradeListPopulator : MonoBehaviour
{
    [Header("Settings")]
    public GameObject upgradePrefab;
    public Transform contentParent; 

    [Header("Upgrades to Show")]
    public List<UpgradeData> availableUpgrades;

    void Start()
    {
        PopulateList();
    }

    public void PopulateList()
    {
        // Zuerst alles löschen, falls wir die Liste neu laden
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (UpgradeData data in availableUpgrades)
        {
            // Prefab instanziieren
            GameObject newEntry = Instantiate(upgradePrefab, contentParent);

            // Die UI-Komponente auf dem Prefab füllen
            UpgradeUIElement uiElement = newEntry.GetComponent<UpgradeUIElement>();

            // Wir holen das aktuelle Level aus dem UpgradeManager
            int currentLevel = UpgradeManager.Instance.GetLevelForUpgrade(data);

            uiElement.Setup(data, currentLevel);
        }
    }
}