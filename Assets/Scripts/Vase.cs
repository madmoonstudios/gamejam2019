using System;
using UnityEngine;

public class Vase : MonoBehaviour
{
    internal void Break()
    {
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        PlayerInteractionManager._instance.TryBreakVase(this);
    }
}