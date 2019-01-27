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

    private bool _flickering;
    private bool isFlickering;
    private float _timeSinceCanBeFeared;

    // Start is called before the first frame updat
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

    private void Update()
    {
        _timeSinceCanBeFeared -= Time.deltaTime;

        if (isFlickering == true)
        {
            _renderer.color = Color.red;
            return;
        }

        if (_timeSinceCanBeFeared > 0.0f)
        {
            _renderer.color = Color.green;
            return;
        }

        _renderer.color = Color.white;
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
        isFlickering |= true;

        yield return new WaitForSeconds(.1f);

        isFlickering = false;
    }

    internal void CanBeFeard()
    {
        _timeSinceCanBeFeared = 1.0f;
    }
}
