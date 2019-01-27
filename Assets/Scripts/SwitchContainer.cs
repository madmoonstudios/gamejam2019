using System;
using System.Collections.Generic;
using UnityEngine;

internal class SwitchContainer : MonoBehaviour
{
    private List<IFearable> _containedFearables = new List<IFearable>();

    private void OnTriggerEnter(Collider other)
    {
        IFearable feared = other.transform.GetComponentInChildren<IFearable>();

        if (feared == null)
        {
            return;
        }

        _containedFearables.Add(feared);
    }

    internal List<IFearable> GetFearablesInContainer()
    {
        return _containedFearables;
    }

    private void OnTriggerExit(Collider other)
    {
        IFearable feared = other.transform.GetComponentInChildren<IFearable>();

        if (feared == null)
        {
            return;
        }

        _containedFearables.Remove(feared);

    }
}