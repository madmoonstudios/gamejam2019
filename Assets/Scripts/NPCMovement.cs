using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface INPCMovementCallback
{
    /// <summary>
    /// Remember to call this from Start!
    /// </summary>
    void RegisterCallback();
    void TargetReached();
}

public class NPCMovement : MonoBehaviour
{    
    private NavMeshAgent _agent;
    [SerializeField] private Transform _moveTarget;
    private float _moveSpeedCurrent;
    private float _moveSpeedNormal = 5f;
    private float _navAgentHeight;

    private INPCMovementCallback _movementCallback;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _moveSpeedCurrent = _moveSpeedNormal;
        _agent.speed = _moveSpeedCurrent;
        _agent.isStopped = true;
        _agent.stoppingDistance = 2.0f;

        _navAgentHeight = transform.position.y;
    }

    /// <summary>
    /// Set the agent's move destination to a static position.
    /// </summary>
    /// <param name="position">static position for the agent to move to.</param>
    public void SetMoveTarget(Vector3 position)
    {
        // TODO(samkern): Destroy transform if it is no longer being moved to.
        Transform t = new GameObject().transform;
        t.position = position;
        _moveTarget = t;

        _agent.isStopped = false;
        _agent.destination = new Vector3(_moveTarget.position.x, _navAgentHeight, _moveTarget.position.z);
        StopCoroutine(UpdateDestination());
    }

    /// <summary>
    /// Set the agent's move destination to a dynamic position and start updates to check if the target moved.
    /// </summary>
    /// <param name="position">dynamic position for the agent to move to.</param>
    public void SetMoveTarget(Transform t)
    {
        if (_moveTarget == t) return;

        _moveTarget = t;
        _agent.isStopped = false;
        StartCoroutine(UpdateDestination());
    }

    /// <summary>
    /// Stop the agent's movement, but do not clear its target.
    /// </summary>
    public void StopMoving()
    {
        Debug.Log("Stop Moving");
        _agent.isStopped = true;
    }

    /// <summary>
    /// Resume the agent's movement toward its current target.
    /// </summary>
    public void ResumeMoving()
    {
        _agent.isStopped = false;
    }
    
    private float _destinationUpdateInterval = .2f;
    
    /// <summary>
    /// Coroutine that periodically updates the agent's destination based on its current target.
    /// </summary>
    private IEnumerator UpdateDestination()
    {
        while (true)
        {
            _agent.destination = new Vector3(_moveTarget.position.x, _navAgentHeight, _moveTarget.position.z);
            yield return new WaitForSeconds(_destinationUpdateInterval);
        }
    }
    
    public void OnDestroy()
    {
        StopCoroutine(UpdateDestination());
    }
    
    // MOVEMENT CALLBACK

    public void Update()
    {
        if (_agent.isStopped || _agent.pathPending) return;
        
        // Check if we've reached the destination
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (_agent.hasPath || _agent.velocity.sqrMagnitude <= .1f)
            {
                HasReachedTarget();
            }
        }
    }

    private void HasReachedTarget()
    {
        _agent.isStopped = true;
        _movementCallback.TargetReached();
    }

    public void SetNPCMovementCallback(INPCMovementCallback moveCallback)
    {
        _movementCallback = moveCallback;
    }
}
