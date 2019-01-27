using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

public class BuyerSpawner : MonoBehaviour
{
    public int buyerNumber = int.MaxValue;

    [SerializeField]
    private int _buyersRemaining;

    [SerializeField]
    private GameObject _buyerPrefab;

    [SerializeField]
    private float _waitTime;

    [SerializeField]
    private float _raycastStartY = 80.0f;
    // Start is called before the first frame update
    void Start()
    {
        this._buyersRemaining = this.buyerNumber;
        _waitTime = UnityEngine.Random.Range(3.0f, 10.0f);
        StartCoroutine(Spawn());

        var globalConfigData = this.LoadArchetypeData("/archetypes-full.json");
        Debug.Log(JsonUtility.ToJson(globalConfigData, true));

        var waveConfigData = this.LoadWaveData("/waves.json");
        Debug.Log(JsonUtility.ToJson(waveConfigData, true));
    }

    private IEnumerator Spawn()
    {
        while (this._buyersRemaining > 0)
        {


            int layerMask = 1 << 8;
            // Everything but layer 8, floor should be in something OTHER than layer 8!
            layerMask = ~layerMask;
            RaycastHit hit;
            Physics.Raycast(
                new Ray(
                    new Vector3(
                        this.transform.position.x,
                         _raycastStartY,
                         this.transform.position.z
                    ),
                    new Vector3(0.0f, -1.0f, 0.0f)),
                    out hit,
                    Mathf.Infinity,
                    layerMask
            );
            NavMeshAgent agent = GameObject.Instantiate(
                _buyerPrefab,
                 hit.point,
                 Quaternion.identity,
                 null
            ).GetComponent<NavMeshAgent>();

            agent.transform.position += new Vector3(0, agent.height / 2, 0);
            this._buyersRemaining--;
            yield return new WaitForSeconds(_waitTime);

        }
    }

    private GlobalConfigData LoadArchetypeData(string jsonFilePath)
    {
        string filePath = Application.dataPath + jsonFilePath;

        GlobalConfigData globalConfigData;
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
           globalConfigData = JsonUtility.FromJson<GlobalConfigData>(dataAsJson);
        }
        else
        {
            globalConfigData = new GlobalConfigData();
        }
        return globalConfigData;

       
    }

    private WavesConfigData LoadWaveData(string jsonFilePath)
    {
        string wavesConfigFilePath = Application.dataPath + jsonFilePath;
        WavesConfigData wavesConfigData;

        if (File.Exists(wavesConfigFilePath))
        {
            string waveJsonData = File.ReadAllText(wavesConfigFilePath);
            wavesConfigData = JsonUtility.FromJson<WavesConfigData>(waveJsonData);
        }
        else
        {
            wavesConfigData = new WavesConfigData();
        }
        return wavesConfigData;
    }

}
