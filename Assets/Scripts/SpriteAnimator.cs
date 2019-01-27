using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField]
    private Sprite [] _sprites;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private float _timeBetweenFrames;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int frame = 0;
        while (true)
        {
            _renderer.sprite = _sprites[frame++ % _sprites.Length]; 
            yield return new WaitForSeconds(_timeBetweenFrames);
        }
    }

    internal void DoFearFlicker()
    {
        StartCoroutine(FearFlickerRoutine());
    }

    private IEnumerator FearFlickerRoutine()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(.1f);

        _renderer.color = Color.white;
    }
}
