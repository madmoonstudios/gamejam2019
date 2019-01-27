using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyerController : MonoBehaviour, IFearable, INPCMovementCallback
{
    enum MoveTargetType
    {
        NONE, ROOM, REALTOR, FLEE
    }

    [SerializeField] private float _fearLevelCurrent = 0;

    [SerializeField]
    private ProgressBarPro _fearBar;

    private float _fearLevelInitial = 40;               // Standard fear for a new buyer.

    internal void TryDestroy()
    {
        if (_moveTargetType == MoveTargetType.FLEE)
        {
            GameManager._instance.AddToScore();
            Destroy(this.gameObject);
        }
    }

    private float _fearLevelMax = 100;                  // The fear level at which the buyer will flee the house.
    private float _fearIncrementAmount = 10;            // Standard fear gained when scared.
    private float _fearDecrementAmount = 5;             // Standard fear lost over time.

    [SerializeField] private List<int> _roomsLeftToVisit;
    [SerializeField] private int _nextRoomIndex;
    
    private NPCMovement _npcMovement;
    private MoveTargetType _moveTargetType; // Are they moving within the room, to a realtor, or fleeing the house?
    
    // TODO(samkern): Choose an interest point randomly within the room to go visit.

    void Awake()
    {        
        _fearLevelCurrent = _fearLevelInitial;
        _npcMovement = GetComponent<NPCMovement>();
        RegisterCallback();
        _fearBar = GetComponentInChildren<ProgressBarPro>();
    }

    public void Start()
    {
        
        // Intialize array of rooms visited to false.
        _roomsLeftToVisit = Enumerable.Range(0, Room.allRooms.Count).ToList();
        MoveToUnvisitedRoom();
        
        StartCoroutine(DecrementFear());
    }

    private void MoveToUnvisitedRoom()
    {
        if (_roomsLeftToVisit.Count == 0)
        {
            // TODO(samkern): If the scare level is a certain amount, do not attempt to purchase the house, keep moving between rooms
            MoveToRealtor(); // We have visited all rooms; try to purchase the house.
        }
        else
        { 
            // Randomly select a room to go to first.
            _nextRoomIndex = UnityEngine.Random.Range(0, _roomsLeftToVisit.Count);
            
            // Start moving toward that room.
            _moveTargetType = MoveTargetType.ROOM;
            _npcMovement.SetMoveTarget(Room.allRooms[_nextRoomIndex].transform);
        }
    }

    internal bool IsScared()
    {
        return _fearLevelCurrent > _fearLevelMax / 2.0f;
    }

    internal float GetSpookLevel()
    {
        return _fearLevelCurrent / _fearLevelMax;
    }

    internal bool TryEndGame()
    {
        if (_moveTargetType == MoveTargetType.REALTOR)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return true;
        }

        return false;
    }

    private void MoveToRealtor()
    {
        _moveTargetType = MoveTargetType.REALTOR;
        _npcMovement.SetMoveTarget(RealtorController.realtorTransform);
    }

    private void FleeHouse()
    {
        _moveTargetType = MoveTargetType.FLEE;
        _npcMovement.SetMoveTarget(FrontDoor.frontDoorTransform);
        StopCoroutine(DecrementFear());    // The buyer is panicking; do not reduce fear over time.
    }

    // IFearable
    public void Scare()
    {
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
        // If the buyer is not scared and they have visited all rooms.
        else if (_fearLevelCurrent <= 0 && _roomsLeftToVisit.Count == 0)
        {
            MoveToRealtor();     // Go buy the house!
        }
    }
    
    public void OnDestroy()
    {
        StopCoroutine(DecrementFear());
    }
    
    // NPC MOVEMENT CALLBACK

    public void TargetReached()
    {
        switch (_moveTargetType)
        {
            case MoveTargetType.NONE:
                Debug.Log("No movement type set; doing nothing");
                break;
            case MoveTargetType.ROOM:
                _roomsLeftToVisit.RemoveAt(_nextRoomIndex);
                MoveToUnvisitedRoom();
                break;
            case MoveTargetType.REALTOR:
                TryEndGame();
                break;
            case MoveTargetType.FLEE:
                TryDestroy();
                break;
        }
    }

    public void RegisterCallback()
    {
        _npcMovement.SetNPCMovementCallback(this);
    }
}
