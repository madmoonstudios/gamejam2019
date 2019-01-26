using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionManager : MonoBehaviour
{
    public static PlayerInteractionManager _instance;

    [SerializeField]
    private GameObject _pentagram;

    [SerializeField]
    private ProgressBarPro _pentagramRechargeProgress;
    
    [SerializeField]
    private ProgressBarPro _vaseRechargeProgress;

    private const float c_pentagramMaxTime = 10.0f;
    private const float c_vaseMaxTime = 10.0f;

    private float _pentagramRemainingTime = 0.0f;

    private bool _clickProcessed = false;

    private float _vaseRemainingTime = 0.0f;

    private float _currentFear;

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
    }

    private void LateUpdate()
    {
        _clickProcessed = false;
    }

    public bool TryBreakVase(Vase vase)
    {
        if (_clickProcessed)
        {
            return false;
        }

        if (_vaseRemainingTime > 0)
        {
            return false;
        }

        vase.Break();
        _clickProcessed = true;
        return true;
    }

    public bool TryDropPentagram()
    {
        if (_clickProcessed)
        {
            return false;
        }

        if (_pentagramRemainingTime > 0)
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
