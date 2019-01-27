using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pentagram : FearInducer
{
    [SerializeField]
    private int _maxTimeSeconds;
    
    private float _fearRadius;

    [SerializeField]
    private MeshRenderer _collisionMesh;

    // Start is called before the first frame update
    void Start()
    {
        _fearRadius = _collisionMesh.transform.localScale.x;
        StartCoroutine(ScareThenDissapear());
    }

    private IEnumerator ScareThenDissapear()
    {
        yield return new WaitForSeconds(2.0f);

        for (int timeLeft = _maxTimeSeconds; timeLeft > 0; timeLeft--)
        {
            _collisionMesh.enabled = false;

            yield return new WaitForSeconds(0.9f);
            
            base.ScareInRadius(this.transform.position, _fearRadius);

            _collisionMesh.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(this.gameObject);
    }
}
