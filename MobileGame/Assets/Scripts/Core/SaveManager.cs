using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;

    private void Awake()
    {
        Instance = this;
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void Save(double currentMoney, Dictionary<string, int> levels)
    {
        SaveData data = new SaveData();
        data.money = currentMoney;
        data.lastSaveTime = System.DateTime.Now.ToBinary().ToString();

        // Dictionary in Listen umwandeln für JSON
        foreach (var pair in levels)
        {
            data.upgradeKeys.Add(pair.Key);
            data.upgradeValues.Add(pair.Value);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public SaveData Load()
    {
        if (!File.Exists(savePath)) return null;

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void DeleteSaveFile()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Savegame erfolgreich gelöscht!");
        }
    }
}