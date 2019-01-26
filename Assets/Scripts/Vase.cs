using System;
using UnityEngine;

public class Vase : MonoBehaviour, IClickable
{
    void IClickable.Interact()
    {
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        PlayerInteractionManager._instance.TryBreakVase(this);
    }
}