using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{    
    private NavMeshAgent _agent;
    private Transform _moveTarget;
    private float _moveSpeedCurrent;
    private float _moveSpeedNormal = 5f;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _moveSpeedCurrent = _moveSpeedNormal;
        _agent.speed = _moveSpeedCurrent;
        _agent.isStopped = true;
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
        _agent.destination = _moveTarget.position;
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
            _agent.destination = _moveTarget.position;
            yield return new WaitForSeconds(_destinationUpdateInterval);
        }
    }
}
