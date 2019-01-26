using System.Collections;
using UnityEngine;

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
            Debug.Log(hit.point);
            GameObject.Instantiate(_buyerPrefab, this.transform.position, Quaternion.identity, null);
            this._buyersRemaining--;

        }
    }
}
