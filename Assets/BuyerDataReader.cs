using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BuyerDataReader : MonoBehaviour
{
    public GlobalConfigData globalConfigData;

    public WavesConfigData wavesConfigData;
    private string gameDataProjectFilePath = "/archetypes-full.json";
    private string wavesConfigDataFilePath = "/waves.json";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START");
        //this.LoadGameData();
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
            globalConfigData = JsonUtility.FromJson<GlobalConfigData>(dataAsJson);
            Debug.Log(JsonUtility.ToJson(globalConfigData, true));
        }
        else
        {
            globalConfigData = new GlobalConfigData();
        }

        string wavesConfigFilePath = Application.dataPath + wavesConfigDataFilePath;


        if (File.Exists(wavesConfigFilePath))
        {
            string waveJsonData = File.ReadAllText(wavesConfigFilePath);
            wavesConfigData = JsonUtility.FromJson<WavesConfigData>(waveJsonData);
            Debug.Log(JsonUtility.ToJson(wavesConfigData, true));
        }
        else
        {
            wavesConfigData = new WavesConfigData();
        }
        Debug.Log("GameData name \"" + globalConfigData.archetypes[0].name + "\"" + " path " + filePath);
    }

    private void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(globalConfigData);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }
}
