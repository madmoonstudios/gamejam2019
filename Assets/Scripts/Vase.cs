using System;
using UnityEngine;

public class Vase : FearInducer, IClickable
{
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    void IClickable.Interact()
    {
        spriteRenderer.sprite = sprite;
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