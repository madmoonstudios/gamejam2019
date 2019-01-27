using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLight : FearInducer, IClickable
{
    [SerializeField]
    private Light _light;

    [SerializeField]
    private SwitchContainer container;

    private Color _startingColor;
    private float _startingIntensity;

    public bool isFlickering { get; private set; }

    private void Awake()
    {
        _startingColor = _light.color;
        _startingIntensity = _light.intensity;
        StartCoroutine(AmbientFlicker());
    }

    private List<IFearable> SwitchFearableContainer()
    {
        return container.GetFearablesInContainer();
    }

    void IClickable.Interact()
    {
        StartCoroutine(FlickerLights());
    }

    private IEnumerator FlickerLights()
    {
        isFlickering = true;

        int lightFlicks = UnityEngine.Random.Range(2, 5);
        float maxIntensity = _light.intensity;
        for(int curFlick = 0; curFlick < lightFlicks; curFlick++)
        {
            _light.intensity = 0;
            yield return new WaitForSeconds(UnityEngine.Random.Range(.1f, .3f));


            foreach (IFearable fearable in SwitchFearableContainer())
            {
                fearable.Scare();
            }

            _light.intensity = maxIntensity;
            yield return new WaitForSeconds(UnityEngine.Random.Range(.01f, .1f));
        }

        _light.intensity = 0;
        yield return new WaitForSeconds(3.0f);
        base.ScareInRadius(this.transform.position, 20.0f);
        _light.intensity = maxIntensity;
        isFlickering = false;
    }

    private void OnMouseEnter()
    {
        MouseCursor._instance._lightHighlighted = true;
    }

    private void OnMouseExit()
    {
        MouseCursor._instance._lightHighlighted = false;
    }

    private void OnMouseOver()
    {
        if (PlayerInteractionManager._instance.CannotSwitchLight())
        {
            return;
        }

        foreach (IFearable fearable in SwitchFearableContainer())
        {
            fearable.ShowPotentiallyScarable();
        }
    }

    private IEnumerator AmbientFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(.1f, 1.0f));
            if (isFlickering)
            {
                continue;
            }

            if (PlayerInteractionManager._instance.CannotSwitchLight())
            {
                _light.color = _startingColor / UnityEngine.Random.Range(1.0f, 1.4f);
                _light.intensity = _startingIntensity / UnityEngine.Random.Range(1.0f, 1.4f);

            }
            else
            {
                _light.color = _startingColor;
                _light.intensity = _startingIntensity;
            }
        }
    }

    private void OnMouseDown()
    {
        PlayerInteractionManager._instance.TrySwitchLight(this);
    }
}