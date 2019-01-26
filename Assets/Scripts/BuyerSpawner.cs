using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class BuyerSpawner : MonoBehaviour
{
    // Use this for initialization
    static void Start()
    {
        print("Startup");
        GameObject myRoadInstance =
            Instantiate(Resources.Load("Buyer"),
            new Vector3(5, 5, 5),
            Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
