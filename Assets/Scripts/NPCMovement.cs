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
    private float _moveSpeedNormal;
    private float _navAgentHeight;

    private INPCMovementCallback _movementCallback;
    
    
    // TODO(samkern): Replace this with some sort of archetype generator, if we end up having one.
    private void ConfigureStats()
    {
        _agent.stoppingDistance = 2.0f;
        _moveSpeedNormal = UnityEngine.Random.Range(4.0f, 7.0f);
        _agent.speed = _moveSpeedNormal;
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.isStopped = true;

        _navAgentHeight = 2; // TODO(samkern): make this a global
        transform.position = new Vector3(transform.position.x, _navAgentHeight, transform.position.z);
        
        ConfigureStats();
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
    /// Pause the agent's movement, but do not clear its target.
    /// </summary>
    public void PauseMoving()
    {
        _agent.velocity = Vector3.zero;
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
    private bool updateDestinationRunning = false;
    
    /// <summary>
    /// Coroutine that periodically updates the agent's destination based on its current target.
    /// </summary>
    private IEnumerator UpdateDestination()
    {
        updateDestinationRunning = true;
        while (_moveTarget != null)
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
        if (_moveTarget == null || _agent.isStopped || _agent.pathPending) return;
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
        _moveTarget = null;
        _movementCallback.TargetReached();
    }

    public void SetNPCMovementCallback(INPCMovementCallback moveCallback)
    {
        _movementCallback = moveCallback;
    }

    public void SetAgentVelocity(Vector3 velocity)
    {
        _agent.velocity = velocity;
    }

    public void SetSpeedMod(float speedMod)
    {
        _agent.speed = _moveSpeedNormal * speedMod;
    }
    
    public void SetSpeed(float speed)
    {
        _agent.speed = speed;
    }
}
