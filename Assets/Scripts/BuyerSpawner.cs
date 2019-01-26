using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BuyerSpawner : MonoBehaviour
{
    [SerializeField]
    public int buyerNumber = 4;

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
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (this._buyersRemaining > 0)
        {
            yield return new WaitForSeconds(_waitTime);

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
            var navMeshComponent = this._buyerPrefab.GetComponent<NavMeshAgent>();
            GameObject.Instantiate(
                _buyerPrefab,
                 hit.point + new Vector3(0, navMeshComponent.height/2, 0),
                 Quaternion.identity,
                 null
            );
            this._buyersRemaining--;

        }
    }
}
