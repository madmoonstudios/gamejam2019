﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLight : FearInducer, IClickable
{
    [SerializeField]
    private Light _light;

    [SerializeField]
    private SwitchContainer container;

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
    }

    private void OnMouseOver()
    {
        Debug.Log("Mouse captured");

        foreach (IFearable fearable in SwitchFearableContainer())
        {
            fearable.ShowPotentiallyScarable();
        }
    }

    private void OnMouseDown()
    {
        PlayerInteractionManager._instance.TrySwitchLight(this);
    }
}