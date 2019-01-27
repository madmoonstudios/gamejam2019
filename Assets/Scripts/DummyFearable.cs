using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyFearable : MonoBehaviour, IFearable
{
    public void ShowPotentiallyScarable()
    {
        throw new System.NotImplementedException();
    }

    void IFearable.Scare()
    {
        Debug.Log("I am scared");
    }
}
