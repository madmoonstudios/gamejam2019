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

    private float _fearLevelInitial = 40;               // Standard fear for a new buyer.
    private float _fearLevelMax = 100;                  // The fear level at which the buyer will flee the house.
    private float _fearIncrementAmount = 10;            // Standard fear gained when scared.
    private float _fearDecrementAmount = 10;             // Standard fear lost over time.

    [SerializeField] private List<int> _roomsLeftToVisit;
    [SerializeField] private int _nextRoomIndex;

    private SpriteAnimator _spriteAnimator;
    private MoodIndicator _moodIndicator;
    
    private NPCMovement _npcMovement;
    private MoveTargetType _moveTargetType; // Are they moving within the room, to a realtor, or fleeing the house?

    private float leisurely = 0.0f;    
        
    // TODO(samkern): Replace this with some sort of archetype generator, if we end up having one.
    private void ConfigureStats()
    {
        leisurely = UnityEngine.Random.Range(0.0f, 1.0f);
    }

    void Awake()
    {
        _moodIndicator = GetComponentInChildren<MoodIndicator>();
        _spriteAnimator = GetComponentInChildren<SpriteAnimator>();
        _fearLevelCurrent = _fearLevelInitial;
        _npcMovement = GetComponent<NPCMovement>();
        RegisterCallback();
        ConfigureStats();
    }

    public void Start()
    {
        // Intialize array of rooms visited to false.
        _roomsLeftToVisit = Enumerable.Range(0, Room.allRooms.Count).ToList();
        MoveToNextRoom();
        
        StartCoroutine(DecrementFear());
    }

    private void MoveToNextRoom()
    {
        _npcMovement.SetSpeedMod(1.0f);
        _moveTargetType = MoveTargetType.ROOM;
        if (_roomsLeftToVisit.Count == 0)
        {
            int nextRoomIndex = UnityEngine.Random.Range(0, Room.allRooms.Count);
            _spriteAnimator.StartAnimating();
            _npcMovement.SetMoveTarget(Room.allRooms[_nextRoomIndex].GetRandomInterestPoint().transform);
        }
        else
        {
            //_nextRoomIndex is only used for unvisited rooms
            _nextRoomIndex = UnityEngine.Random.Range(0, _roomsLeftToVisit.Count);
            _spriteAnimator.StartAnimating();
            _npcMovement.SetMoveTarget(Room.allRooms[_nextRoomIndex].GetRandomInterestPoint().transform);
        }
    }

    private void MoveToRealtor()
    {
        StopCoroutine(PauseBeforeNextMove());
        _npcMovement.SetSpeed(1.8f);
        PurchaseHouseIndicator();
        _moveTargetType = MoveTargetType.REALTOR;
        _spriteAnimator.StartAnimating();
        _npcMovement.SetMoveTarget(RealtorController.realtorTransform);
    }

    private void PurchaseHouseIndicator()
    {
        _moodIndicator.PurchaseHouseIndicator();
    }

    private void FleeHouse()
    {
        StopCoroutine(PauseBeforeNextMove());
        _npcMovement.SetSpeedMod(3f);
        _moodIndicator.PanicIndicator();
        _moveTargetType = MoveTargetType.FLEE;
        _spriteAnimator.StartAnimating();
        _npcMovement.SetMoveTarget(FrontDoor.frontDoorTransform);
        StopCoroutine(DecrementFear());    // The buyer is panicking; do not reduce fear over time.
    }

    // IFearable
    public void Scare()
    {
        _moodIndicator.GhostIndicator();
        _fearLevelCurrent += _fearIncrementAmount;
        DoFearChecks();
        _animator.DoFearFlicker();
    }
    
    internal bool IsScaredMild()
    {
        return _fearLevelCurrent > _fearLevelMax / 3.0f;
    }
    
    internal bool IsScared()
    {
        return _fearLevelCurrent > _fearLevelMax / 2.0f;
    }

    internal float GetSpookLevel()
    {
        return _fearLevelCurrent / _fearLevelMax;
    }

    private float _fearDecrementInterval = 5.0f;

    [SerializeField]
    private SpriteAnimator _animator;

    /// <summary>
    /// Coroutine that periodically decrements the fear level of the NPC.
    /// </summary>
    private IEnumerator DecrementFear()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fearDecrementInterval);
            _fearLevelCurrent = Mathf.Clamp(_fearLevelCurrent - _fearDecrementAmount, 0, _fearLevelMax);
            DoFearChecks();
        }
    }

    private void DoFearChecks()
    {
        if (_moveTargetType == MoveTargetType.FLEE) return; // We are already fleeing; do nothing else
        
        if (_fearLevelCurrent >= _fearLevelMax)
        {
            FleeHouse();         // OMG leave, dis too scary
        }
        // If the buyer is not scared and they have visited all rooms.
        else if (_fearLevelCurrent <= (_fearLevelMax / 2.0f) && _roomsLeftToVisit.Count == 0 && _moveTargetType != MoveTargetType.REALTOR)
        {
            MoveToRealtor();     // Go buy the house!
        }
        else if (_moveTargetType == MoveTargetType.REALTOR) // Stop going to the realtor! We're too scared!
        {
            _moodIndicator.ShrinkIndicator();
            MoveToNextRoom();
        }
        
        if(IsScaredMild()) _moodIndicator.ScaredIndicator();
        else _moodIndicator.HideScaredIndicator();
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
                if (_roomsLeftToVisit.Count != 0)
                    _roomsLeftToVisit.RemoveAt(_nextRoomIndex);
                StartCoroutine(PauseBeforeNextMove());
                break;
            case MoveTargetType.REALTOR:
                TryEndGame();
                break;
            case MoveTargetType.FLEE:
                TryDestroy();
                break;
        }
    }

    private IEnumerator PauseBeforeNextMove()
    {
        float time = leisurely * 10.0f;
        _spriteAnimator.StopAnimating();
        _npcMovement.PauseMoving();
        //if(!IsScared()) _moodIndicator.HappyIndicator();

        yield return new WaitForSeconds(time);

        if (_moveTargetType != MoveTargetType.FLEE) // If we are already fleeing, do nothing
        {      
            if (_roomsLeftToVisit.Count == 0 && !IsScared())
            {
                MoveToRealtor(); // We have visited all rooms; try to purchase the house.
            }
            else
            {
                MoveToNextRoom();
            }
            _spriteAnimator.StartAnimating();
            _npcMovement.ResumeMoving();
        }
    }

    public void RegisterCallback()
    {
        _npcMovement.SetNPCMovementCallback(this);
    }
    
    internal void TryDestroy()
    {
        if (_moveTargetType == MoveTargetType.FLEE)
        {
            GameManager._instance.AddToScore();
            Destroy(this.gameObject);
        }
    }

    public void ShowPotentiallyScarable()
    {
        _animator.CanBeFeard();
    }
}
