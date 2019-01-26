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
    
    private const float c_pentagramMaxTime = 10.0f;
    
    private float _pentagramRemainingTime = 0.0f;

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
    }

    public bool TryDropPentagram()
    {
        if (_pentagramRemainingTime > 0)
        {
            return false;
        }

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        Vector3 adjustedWorldPos = new Vector3(worldPos.x, worldPos.y, 0);

        Instantiate(_pentagram, adjustedWorldPos, Quaternion.identity);
        _pentagramRemainingTime = c_pentagramMaxTime;

        return true;
    }
}
