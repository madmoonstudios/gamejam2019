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
            GameObject.Instantiate(_buyerPrefab, this.transform.position, Quaternion.identity, null);
            this._buyersRemaining--;

        }
    }
}
