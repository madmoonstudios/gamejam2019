using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVisitors : MonoBehaviour
{
    [SerializeField]
    private GameObject _visitor;

    [SerializeField]
    private float _waitTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(_waitTime);
            GameObject.Instantiate(_visitor, this.transform.position, Quaternion.identity, null);
        }
    }
}
