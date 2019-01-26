using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyFearable : MonoBehaviour, IFearable
{
    void IFearable.Scare()
    {
        Debug.Log("I am scared");
    }
}
