using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVisitors : MonoBehaviour
{
    [SerializeField]
    public int buyerNumber = 4;

    [SerializeField]
    private int _buyersRemaining;

    [SerializeField]
    private GameObject _visitor;

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
            GameObject.Instantiate(_visitor, this.transform.position, Quaternion.identity, null);
            this._buyersRemaining--;

        }
        //for (int i = 0; i < this.buyerNumber; i++)
        //{
        //    yield return new WaitForSeconds(_waitTime); 
        //    GameObject.Instantiate(_visitor, this.transform.position, Quaternion.identity, null);
        //}
    }
}
