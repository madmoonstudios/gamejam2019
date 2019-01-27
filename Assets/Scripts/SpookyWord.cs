using System;
using UnityEngine;
using TMPro;
using System.Collections;

internal class SpookyWord : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _text;
    internal void SetText(string word)
    {
        _text.text = word;
        StartCoroutine(LerpAlpha());
    }

    private IEnumerator LerpAlpha()
    {
        while (_text.alpha > 0)
        {
            _text.alpha += UnityEngine.Random.Range(-.1f, .05f);

            yield return 0;//new WaitForSeconds(.1f);
        }

        Destroy(this.gameObject);
    }
}