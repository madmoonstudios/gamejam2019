using System;
using UnityEngine;
using TMPro;

internal class SpookyWord : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _text;
    internal void SetText(string word)
    {
        _text.text = word;
    }
}