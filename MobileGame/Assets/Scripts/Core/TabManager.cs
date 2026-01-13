using UnityEngine;

public class TabManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject laserPanel;
    public GameObject shipPanel;
    public GameObject shopPanel;

    private void Start()
    {
        ShowLaser(); // Standardmäßig Laser zeigen
    }

    public void ShowLaser() => SetActivePanel(laserPanel);
    public void ShowShips() => SetActivePanel(shipPanel);
    public void ShowShop() => SetActivePanel(shopPanel);

    private void SetActivePanel(GameObject active)
    {
        laserPanel.SetActive(active == laserPanel);
        shipPanel.SetActive(active == shipPanel);
        shopPanel.SetActive(active == shopPanel);
    }
}