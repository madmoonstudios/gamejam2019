using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pentagram : FearInducer
{
    private int _maxTimeSeconds = 4;
    
    public static float c_fearRadius = 2.0f;

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
        this.transform.localScale = Vector3.one * .1f;

        float tPassed = 0.0f;
        while (tPassed < 1.0f)
        {
            tPassed += Time.deltaTime;
            this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one *.5f, tPassed);
            yield return new WaitForEndOfFrame();
        }

        tmp.a = 1.0f;
        _renderer.color = tmp;

        for (int timeLeft = _maxTimeSeconds; timeLeft > 0; timeLeft--)
        {
            yield return new WaitForSeconds(0.1f);
            _renderer.color = Color.black;


            base.ScareInRadius(this.transform.position, c_fearRadius);
            yield return new WaitForSeconds(0.1f);
            _renderer.color = Color.white;
            yield return new WaitForSeconds(0.8f);
        }

        Destroy(this.gameObject);
    }
}
