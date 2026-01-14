using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    public void ResetProgress()
    {
        // 1. Savegame löschen
        SaveManager.Instance.DeleteSaveFile();

        // 2. Szene neu laden (Das setzt alles im RAM zurück)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Kleiner Cheat für Developer
    public void AddMoneyCheat()
    {
        ResourceManager.Instance.AddResources(1000);
    }
}