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

    private bool _animatingMovement = false;

    void Start()
    {
        StartAnimating();
    }

    public void StartAnimating()
    {
        if (!_animatingMovement)
        {
            _animatingMovement = true;
            StartCoroutine(Animate());
        }
    }
    
    public void StopAnimating()
    {
        if (_animatingMovement)
        {
            _renderer.sprite = _sprites[2];   // Hard coded to the image of the visitor standing.
            _animatingMovement = false;
            StopCoroutine(Animate());
        }
    }

    private IEnumerator Animate()
    {
        int frame = 0;
        while (_animatingMovement)
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
