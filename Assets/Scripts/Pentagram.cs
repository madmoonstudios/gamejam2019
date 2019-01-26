using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pentagram : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _countdownText;

    [SerializeField]
    private int _maxTimeSeconds;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScareThenDissapear());
    }

    private IEnumerator ScareThenDissapear()
    {
        for (int timeLeft = _maxTimeSeconds; timeLeft > 0; timeLeft--)
        {
            _countdownText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        Destroy(this.gameObject);
    }
}
