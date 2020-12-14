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

    private float _mildScaredLevel = 20.0f;
    private float _scaredLevel = 50.0f;
    public float _fearLevelInitial = 0f;               // Standard fear for a new buyer.
    public float _fearLevelMax = 100f;                  // The fear level at which the buyer will flee the house.
    public float _fearIncrementRatio = 1.0f;            // Standard fear gained when scared.
    public float _fearDecrementRatio = 1.0f;             // Standard fear lost over time.
    public float _fearDecrementAmount = 10.0f;
    public float _moveSpeed = 1.0f;
    public float _runSpeed = 1.0f;
    public List<InterestPoint> _interestPoints = new List<InterestPoint>();

    [SerializeField] private List<int> _roomsLeftToVisit;
    [SerializeField] private int _nextRoomIndex = 0;

    public SpriteAnimator _spriteAnimator;
    private MoodIndicator _moodIndicator;
    
    private NPCMovement _npcMovement;
    private MoveTargetType _moveTargetType; // Are they moving within the room, to a realtor, or fleeing the house?

    private float leisurely = 0.0f;

    [SerializeField] private AudioClip buyHouseClip;
    [SerializeField] private AudioClip scaredClip;

    // TODO(samkern): Replace this with some sort of archetype generator, if we end up having one.
    // At this point the configuration should have been set and it is ok to use these values.
    private void ConfigureStats()
    {
        leisurely = UnityEngine.Random.Range(0.0f, 1.0f);
        _fearLevelCurrent = _fearLevelInitial;
    }

    void Awake()
    {
        _moodIndicator = GetComponentInChildren<MoodIndicator>();
        _spriteAnimator = GetComponentInChildren<SpriteAnimator>();
        _npcMovement = GetComponent<NPCMovement>();
        RegisterCallback();
        ConfigureStats();
    }

    public void Start()
    {
        // Intialize list of rooms visited to false.
        _roomsLeftToVisit = Enumerable.Range(0, Room.allRooms.Count).ToList();
        
        // Remove a random room to make it play faster
        _roomsLeftToVisit.Remove(UnityEngine.Random.Range(0, Room.allRooms.Count)); 
        
        MoveToNextRoom();
        
        StartCoroutine(DecrementFear());
        StartCoroutine(MakeScareable());
    }

    private IEnumerator MakeScareable()
    {
        _animator.InvulnerableIndicator();
        yield return new WaitForSeconds(4.0f);
        _scareable = true;
        _animator.VulnerableIndicator();
    }

    private void MoveToNextRoom()
    {
        _npcMovement.SetSpeedMod(1.0f);
        _moveTargetType = MoveTargetType.ROOM;
        if (_roomsLeftToVisit.Count == 0)
        {
            int roomToMoveIndex = UnityEngine.Random.Range(0, Room.allRooms.Count);
            _spriteAnimator.StartAnimating();
            _npcMovement.SetMoveTarget(Room.allRooms[roomToMoveIndex].GetRandomInterestPoint().transform);
        }
        else
        {
            //_nextRoomIndex is only used for unvisited rooms
            _nextRoomIndex = UnityEngine.Random.Range(0, _roomsLeftToVisit.Count);
            int roomToMoveIndex = _roomsLeftToVisit[_nextRoomIndex];
            _spriteAnimator.StartAnimating();
            _npcMovement.SetMoveTarget(Room.allRooms[roomToMoveIndex].GetRandomInterestPoint().transform);
        }

    }

    private void MoveToRealtor()
    {
        StopCoroutine(PauseBeforeNextMove());
        _npcMovement.SetSpeed(1.4f);
        PurchaseHouseIndicator();
        _moveTargetType = MoveTargetType.REALTOR;
        _spriteAnimator.StartAnimating();
        _npcMovement.SetMoveTarget(RealtorController.realtorTransform);

        var audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.clip = this.buyHouseClip;
            audioSource.Play();
        }
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
    public void Scare(float scareAmount)
    {
        if (!_scareable)
        {
            return;
        }

        var audioSource = this.gameObject.GetComponent<AudioSource>();
        if (audioSource) {
            audioSource.clip = this.scaredClip;
            audioSource.Play();
        }
        _moodIndicator.GhostIndicator();
        _fearLevelCurrent += _fearIncrementRatio * scareAmount;
        DoFearChecks();
        _animator.DoFearFlicker();
    }

    internal bool IsScaredMild()
    {
        return _fearLevelCurrent > _mildScaredLevel;
    }

    internal bool IsScared()
    {
        return _fearLevelCurrent >= _scaredLevel;
    }

    internal float GetSpookLevel()
    {
        return _fearLevelCurrent / _fearLevelMax;
    }

    private float _fearDecrementInterval = 5.0f;

    [SerializeField]
    private SpriteAnimator _animator;
    private bool _scareable = false;

    /// <summary>
    /// Coroutine that periodically decrements the fear level of the NPC.
    /// </summary>
    private IEnumerator DecrementFear()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fearDecrementInterval);
            _fearLevelCurrent = Mathf.Clamp(
                _fearLevelCurrent - (_fearDecrementRatio * _fearDecrementAmount), 0, _fearLevelMax);
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
        else if (!IsScared() && _roomsLeftToVisit.Count == 0 && _moveTargetType != MoveTargetType.REALTOR)
        {
            MoveToRealtor();     // Go buy the house!
        }
        else if (IsScaredMild() && _moveTargetType == MoveTargetType.REALTOR) // Stop going to the realtor! We're too scared!
        {
            _moodIndicator.ShrinkIndicatorByType(MoodIndicator.IndicatorType.PURCHASE_HOUSE);


            // Add additional rooms to go visit
            int mod = (int) Mathf.Floor(Room.allRooms.Count / 2.0f);
            int lowAdjust = UnityEngine.Random.Range(0, mod);
            int highAdjust = UnityEngine.Random.Range(0, mod) + lowAdjust;
            _roomsLeftToVisit = Enumerable.Range(lowAdjust, (Room.allRooms.Count - highAdjust)).ToList();
            MoveToNextRoom();
        }
        
        if(IsScaredMild()) _moodIndicator.ScaredIndicator();
        else
        {
            if (_moveTargetType == MoveTargetType.REALTOR) // TODO(samkern): Fix this hack, which basically ensures the purchase house indicator is shown
                _moodIndicator.PurchaseHouseIndicator();
            
            else _moodIndicator.ShrinkIndicatorByType(MoodIndicator.IndicatorType.SCARED);
        }
    }

    internal bool TryEndGame()
    {
        if (_moveTargetType == MoveTargetType.REALTOR)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(3);
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
