using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerController : MonoBehaviour, IFearable
{
    private float _scareLevelCurrent = 0;
    private float _scareLevelMax = 100;
    private NPCMovement _npcMovement;

    void Awake()
    {
        _npcMovement = GetComponent<NPCMovement>();
    }

    public void Start()
    {
        _npcMovement.SetMoveTarget(new Vector3(0, 0, 0));
    }

    private void MoveToRealtor()
    {
        _npcMovement.SetMoveTarget(RealtorController.realtorTransform);
    }

    private void FleeHouse()
    {
        // TODO(samkern): Implement
    }

    private void DepartHouse()
    {
        // TODO(samkern): Implement
    }

    // Override from IFearable
    public void Scare()
    {
        
    }
}
