﻿using System;
using UnityEngine;

public class Vase : FearInducer, IClickable
{
    void IClickable.Interact()
    {
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        PlayerInteractionManager._instance.TryBreakVase(this);
        base.ScareInRadius(this.transform.position, 20.0f);
    }

    private void OnMouseOver()
    {
        if (PlayerInteractionManager._instance.CannotBreakVase())
        {
            return;
        }

        PlayerInteractionManager.ShowPotentiallyScaredInRadius(this.transform.position, 20.0f);

    }
}