using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pentagram : FearInducer
{
    [SerializeField]
    private int _maxTimeSeconds;
    
    public static float c_fearRadius = 4.5f;

    [SerializeField]
    private SpriteRenderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScareThenDissapear());
    }

    private IEnumerator ScareThenDissapear()
    {
        Color tmp = _renderer.color;
        tmp.a = 0.3f;
        _renderer.color = tmp;
        yield return new WaitForSeconds(2.0f);
        tmp.a = 1.0f;
        _renderer.color = tmp;

        for (int timeLeft = _maxTimeSeconds; timeLeft > 0; timeLeft--)
        {
            yield return new WaitForSeconds(0.9f);
            _renderer.color = Color.black;


            base.ScareInRadius(this.transform.position, c_fearRadius);
            yield return new WaitForSeconds(0.1f);
            _renderer.color = Color.white;
        }

        Destroy(this.gameObject);
    }
}
