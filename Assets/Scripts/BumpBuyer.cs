using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpBuyer : MonoBehaviour
{
    private BuyerController _controller;
    private Rigidbody _rigidbody;
    private NPCMovement _npcMovement;
    private const float c_bounceForce = 10.0f;
    // TODO(samkern): Update and fix if possible :(
/*    
    private void Awake()
    {
        _controller = GetComponent<BuyerController>();
        _rigidbody = GetComponent<Rigidbody>();
        _npcMovement = GetComponent<NPCMovement>();
        _rigidbody.mass = 10.0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        BumpBuyer bumpBuyer = collision.transform.GetComponent<BumpBuyer>();
        if (bumpBuyer != null)
        {
            bumpBuyer.Bump(this);
            this.Bump(bumpBuyer);
        }
    }

    private void Bump(BumpBuyer bumpBuyer)
    {
        Vector3 bounceVector = this.transform.position - bumpBuyer.transform.position;
        _rigidbody.AddForce(bounceVector * c_bounceForce * UnityEngine.Random.Range(.5f, 1.5f));
        //_npcMovement.SetAgentVelocity(_rigidbody.velocity);
        //StartCoroutine(UpdateAgentVelocity());
        if (_controller.IsScared())
        {
            _controller.Scare();
        }
    }

    private float _agentVelocityUpdateInterval;
    private IEnumerator UpdateAgentVelocity()
    {
        while (true)
        {
            _npcMovement.SetAgentVelocity(_rigidbody.velocity);
            yield return new WaitForSeconds(_agentVelocityUpdateInterval);
        }
    }*/
}
