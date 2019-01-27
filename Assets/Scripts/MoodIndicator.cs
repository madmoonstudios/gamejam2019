using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodIndicator : MonoBehaviour
{
    private enum IndicatorType
    {
        NONE, HAPPY, PURCHASE_HOUSE, SCARED, PANIC
    }

    private IndicatorAnimator _indicatorAnimator;
    private SpriteRenderer _spriteRenderer;

    private float _indicatorShowTime = 2.0f;
    private Vector2 _startScale;

    [SerializeField] private Sprite happy, scared, panic;

    private int _activeIndicatorValue;

    void Awake()
    {
        _startScale = transform.localScale;
        _indicatorAnimator = GetComponent<IndicatorAnimator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    private void AllTheImportantThings(int typeValue)
    {
        CancelInvoke();
        _indicatorAnimator.StopAllAnimations();
        _activeIndicatorValue = typeValue;
        _spriteRenderer.enabled = true;
    }

    public void HappyIndicator()
    {
        int typeValue = (int) IndicatorType.HAPPY;
        if (typeValue > _activeIndicatorValue)
        {
            _spriteRenderer.sprite = happy;
            AllTheImportantThings(typeValue);
            _indicatorAnimator.AnimateToSize(Vector2.zero, _startScale * .8f, .4f, RepeatMode.Once);
            _indicatorAnimator.AnimateToColor(Color.white, Color.green, 1.5f, RepeatMode.PingPong);
            Invoke("ShrinkIndicator", _indicatorShowTime);
        }
    }

    public void ScaredIndicator()
    {
        int typeValue = (int) IndicatorType.SCARED;
        if (typeValue > _activeIndicatorValue)
        {
            _spriteRenderer.sprite = scared;
            AllTheImportantThings(typeValue);
            _indicatorAnimator.AnimateToSize(_startScale * .8f, _startScale, .3f, RepeatMode.PingPong);
            _indicatorAnimator.AnimateToColor(Color.yellow, Color.red, .3f, RepeatMode.PingPong);
            Invoke("ShrinkIndicator", _indicatorShowTime * 2f);
        }
    }

    public void PurchaseHouseIndicator()
    {
        Debug.Log("Purchase house");
        int typeValue = (int) IndicatorType.PURCHASE_HOUSE;
        if (typeValue > _activeIndicatorValue)
        {
            _spriteRenderer.sprite = happy;
            AllTheImportantThings(typeValue);
            _indicatorAnimator.AnimateToSize(_startScale, _startScale * 1.3f, .4f, RepeatMode.PingPong);
            _indicatorAnimator.AnimateToColor(Color.yellow, Color.green, .8f, RepeatMode.PingPong);
        }
    }

    public void PanicIndicator()
    {
        int typeValue = (int) IndicatorType.PANIC;
        if (typeValue > _activeIndicatorValue)
        {
            _spriteRenderer.sprite = panic;
            AllTheImportantThings(typeValue);
            _indicatorAnimator.AnimateToSize(_startScale, _startScale * 1.4f, .3f, RepeatMode.PingPong);
            _indicatorAnimator.AnimateToColor(Color.white, Color.red, .3f, RepeatMode.PingPong);
        }
    }

    public void HideIndicator()
    {
        _spriteRenderer.enabled = false;
    }

    public void ShrinkIndicator()
    {
        _indicatorAnimator.AnimateToSize(transform.localScale, Vector2.zero, .4f, RepeatMode.Once);
        Invoke("HideIndicator", .5f);
    }
}
