using System;
using System.Collections;
using UnityEngine;

public class Vase : FearInducer, IClickable
{
    private const float c_scareRadius = 5.0f;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private Sprite spriteNormal;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private bool used = false;

    void IClickable.Interact()
    {
        if (used)
        {
            return;
        }
        spriteRenderer.sprite = sprite;

        base.ScareInRadius(this.transform.position, c_scareRadius);
        spriteRenderer.sprite = sprite;
        used = true;

        this.gameObject.GetComponent<AudioSource>().Play();

        StartCoroutine(ResetVase());
    }

    private void OnMouseDown()
    {
        
        PlayerInteractionManager._instance.TryBreakVase(this);

    }

    private IEnumerator ResetVase()
    {
        yield return new WaitForSeconds(10.0f);
        spriteRenderer.sprite = spriteNormal;
        used = false;
    }



    private void OnMouseEnter()
    {
        if (used)
            return;
        MouseCursor._instance._vaseHighlighted = true;
    }

    private void OnMouseExit()
    {
        if (used)
            return;
        MouseCursor._instance._vaseHighlighted = false;
    }

    private void OnMouseOver()
    {
        if (used)
            return;
        if (PlayerInteractionManager._instance.CannotBreakVase())
        {
            return;
        }

        PlayerInteractionManager.ShowPotentiallyScaredInRadius(this.transform.position, c_scareRadius);

    }
}