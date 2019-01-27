using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
}
