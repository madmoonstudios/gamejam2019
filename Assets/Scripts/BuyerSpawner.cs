using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Collections.Generic;

using ArchetypesMap =
    System.Collections.Generic.Dictionary<string, ArchetypeData>;

public class BuyerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _buyerPrefab;

    [SerializeField]
    private float _waitTime;

    [SerializeField]
    private float _waveWaitTime;

    [SerializeField]
    private float _raycastStartY = 80.0f;
    // Start is called before the first frame update
    void Start()
    {
        _waitTime = UnityEngine.Random.Range(3.0f, 10.0f);
        _waveWaitTime = 10.0f;

        var globalConfigData = this.LoadArchetypeData("/archetypes-full.json");

        ArchetypesMap archetypes = new ArchetypesMap();
        foreach (var archetype in globalConfigData.archetypes)
        {
            archetypes[archetype.name] = archetype;
        }
        Debug.Log(JsonUtility.ToJson(globalConfigData, true));

        var waveConfigData = this.LoadWaveData("/waves.json");
        Debug.Log(JsonUtility.ToJson(waveConfigData, true));

        StartCoroutine(Spawn(archetypes, waveConfigData));
    }

    private IEnumerator Spawn(ArchetypesMap archetypes, WavesConfigData waveConfig)
    {
        foreach(var wave in waveConfig.waves)
        {
            Debug.Log(wave.name);
            foreach (var buyer in wave.buyers)
            {
                yield return new WaitForSeconds(buyer.waitSeconds);
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
                GameObject gameObject = GameObject.Instantiate(
                    _buyerPrefab,
                     hit.point,
                     Quaternion.identity,
                     null
                );

                // Position the buyer.
                NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
                agent.transform.position += new Vector3(0, agent.height / 2, 0);

                BuyerController buyerController = gameObject.GetComponent<BuyerController>();
                ArchetypeData arch = archetypes[buyer.name];
                // Populate buyer properties.
                buyerController._moveSpeed = arch.moveSpeed;
                buyerController._runSpeed = arch.moveSpeed;
                buyerController._fearLevelInitial = arch.buyingIntent;
                buyerController._fearLevelMax = arch.fearStamina;
                buyerController._fearIncrementRatio = arch.fearVulnerability;
                buyerController._fearDecrementRatio = arch.fearResistance;
                // Populate the interest points of the buyer.
                foreach (var poi in arch.interestPoints)
                {
                    // TODO(dandov): Need to find how to map the rooms. An id per room.
                    RoomType roomType = RoomType.BATHROOMS;
                    // TODO(dandov): Make this deterministic.
                    InterestPoint interestPoint =
                        Room.roomsMap[roomType].GetRandomInterestPoint();
                    buyerController._interestPoints.Add(interestPoint);
                }
                
            }

            yield return new WaitForSeconds(_waveWaitTime);
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
