using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public double money;
    public List<string> upgradeKeys = new List<string>();
    public List<int> upgradeValues = new List<int>();
    public string lastSaveTime;
}