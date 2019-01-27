using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BuyerDataReader : MonoBehaviour
{
    public ArchetypeData gameData;
    private string gameDataProjectFilePath = "/archetypes.json";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START");
        this.LoadGameData(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<ArchetypeData>(dataAsJson);
        }
        else
        {
            gameData = new ArchetypeData();
            Debug.Log("No load");
        }
        Debug.Log("GameData name '" + gameData.name + "'" + " path " + filePath);
    }

    private void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }
}
