using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerInteractionManager : MonoBehaviour
{
    public static PlayerInteractionManager _instance;

    [SerializeField]
    private GameObject _pentagram;

    [SerializeField]
    private ProgressBarPro _pentagramRechargeProgress;
    
    [SerializeField]
    private ProgressBarPro _vaseRechargeProgress;

    [SerializeField]
    private ProgressBarPro _lightRechargeProgress;

    private const float c_pentagramMaxTime = 1.0f;
    private const float c_vaseMaxTime = 10.0f;

    private float _pentagramRemainingTime = 0.0f;

    private bool _clickProcessed = false;

    private float _vaseRemainingTime = 0.0f;

    private float _lightRemainingTime = 0.0f;

    private const float c_lightMaxTime = 10.0f;

    private float _currentFear;

    private bool _potentialEffectShowned = false;

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        _pentagramRechargeProgress.SetValue(_pentagramRemainingTime / c_pentagramMaxTime);
        _pentagramRemainingTime -= Time.deltaTime;


        _vaseRechargeProgress.SetValue(_vaseRemainingTime / c_vaseMaxTime);
        _vaseRemainingTime -= Time.deltaTime;

        _lightRechargeProgress.SetValue(_lightRemainingTime / c_lightMaxTime);
        _lightRemainingTime -= Time.deltaTime;


        if (PlayerInteractionManager._instance.CannotPentagram())
        {
            return;
        }

        if (!_potentialEffectShowned)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //todo: make 3.66 actual floor height
            ShowPotentiallyScaredInRadius(new Vector3(worldPos.x, 3.66f, worldPos.z) , Pentagram.c_fearRadius);
        }
    }

    internal bool CannotPentagram()
    {
        if (_pentagramRemainingTime > 0)
        {
            return true;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //todo: make 3.66 actual floor height

        Vector3 adjustedWorldPos = new Vector3(worldPos.x, 3.66f, worldPos.z);

        RaycastHit[] hits = Physics.SphereCastAll(adjustedWorldPos, 6.0f, Vector3.one);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.GetComponentInChildren<Pentagram>() != null)
            {
                return true;
            }
        }

        return false;
    }

    internal bool CannotSwitchLight()
    {
        return _lightRemainingTime > 0;
    }

    internal bool CannotBreakVase()
    {
        return _vaseRemainingTime > 0;
    }

    public static void ShowPotentiallyScaredInRadius(Vector3 position, float fearRadius)
    {
        RaycastHit[] hits = Physics.SphereCastAll(position, fearRadius, Vector3.one);

        foreach (RaycastHit hit in hits)
        {
            IFearable fearable = hit.transform.GetComponentInChildren<IFearable>();
            if (fearable != null)
            {
                fearable.ShowPotentiallyScarable();
            }
        }
    }

    private void LateUpdate()
    {
        _clickProcessed = false;
    }

    internal bool TryBreakVase(IClickable vase)
    {
        if (_clickProcessed)
        {
            return false;
        }

        if (_vaseRemainingTime > 0)
        {
            return false;
        }

        _vaseRemainingTime = c_vaseMaxTime;
        vase.Interact();
        _clickProcessed = true;
        return true;
    }

    internal bool TrySwitchLight(IClickable light)
    {
        if (_clickProcessed)
        {
            return false;
        }

        if (_lightRemainingTime > 0)
        {
            return false;
        }

        _lightRemainingTime = c_lightMaxTime;
        light.Interact();
        _clickProcessed = true;
        return true;
    }

    public bool TryDropPentagram()
    {
        if (_clickProcessed)
        {
            return false;
        }

        if (CannotPentagram())
        {
            return false;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //todo: make 3.66 actual floor height

        Vector3 adjustedWorldPos = new Vector3(worldPos.x, 3.66f, worldPos.z);

        Instantiate(_pentagram, adjustedWorldPos, Quaternion.identity);
        _pentagramRemainingTime = c_pentagramMaxTime;
        _clickProcessed = true;
        return true;
    }
}
