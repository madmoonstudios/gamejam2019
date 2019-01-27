﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pentagram : FearInducer
{
    [SerializeField]
    private int _maxTimeSeconds;
    
    public static float c_fearRadius = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScareThenDissapear());
    }

    private IEnumerator ScareThenDissapear()
    {
        yield return new WaitForSeconds(2.0f);

        for (int timeLeft = _maxTimeSeconds; timeLeft > 0; timeLeft--)
        {
            yield return new WaitForSeconds(0.9f);
            
            base.ScareInRadius(this.transform.position, c_fearRadius);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(this.gameObject);
    }
}
