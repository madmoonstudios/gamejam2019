using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerController : MonoBehaviour, IFearable
{
    [SerializeField] private float _fearLevelCurrent = 0;

    [SerializeField]
    private ProgressBarPro _fearBar;

    private float _fearLevelInitial = 40;               // Standard fear for a new buyer.
    private float _fearLevelMax = 100;                  // The fear level at which the buyer will flee the house.
    private float _fearIncrementAmount = 10;            // Standard fear gained when scared.
    private float _fearDecrementAmount = 5;             // Standard fear lost over time.
    private NPCMovement _npcMovement;

    void Awake()
    {
        _fearLevelCurrent = _fearLevelInitial;
        _npcMovement = GetComponent<NPCMovement>();
    }

    public void Start()
    {
        Debug.Log("Buyer");
        MoveToRealtor();
        StartCoroutine(DecrementFear());
    }

    internal bool IsScared()
    {
        return _fearLevelCurrent > _fearLevelMax / 2.0f;
    }

    private void MoveToRealtor()
    {
        _npcMovement.SetMoveTarget(RealtorController.realtorTransform);
    }

    private void FleeHouse()
    {
        _npcMovement.SetMoveTarget(FrontDoor.frontDoorTransform);
    }

    // IFearable
    public void Scare()
    {
        Debug.Log("I am scared");
        _fearLevelCurrent += _fearIncrementAmount;
        DoFearChecks();
    }

    private float _fearDecrementInterval = 2.0f;
    /// <summary>
    /// Coroutine that periodically decrements the fear level of the NPC.
    /// </summary>
    private IEnumerator DecrementFear()
    {
        while (true)
        { 
            yield return new WaitForSeconds(_fearDecrementInterval);
            _fearLevelCurrent = Mathf.Clamp(_fearLevelCurrent - _fearDecrementAmount, 0, _fearLevelMax);
            _fearBar.SetValue(_fearLevelCurrent, _fearLevelMax);
            DoFearChecks();
        }
    }

    private void DoFearChecks()
    {
        if (_fearLevelCurrent >= _fearLevelMax)
        {
            FleeHouse();         // OMG leave, dis too scary
        }
        else if (_fearLevelCurrent <= 0)
        {
            MoveToRealtor();     // Go buy the house!
        }
    }
    
    public void OnDestroy()
    {
        StopCoroutine(DecrementFear());
    }
}
