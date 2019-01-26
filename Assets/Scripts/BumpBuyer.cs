using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpBuyer : MonoBehaviour
{
    [SerializeField]
    private BuyerController _controller;
    [SerializeField]
    private Rigidbody _rigidbody;
    private const float c_bounceForce = 10.0f;

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
        _rigidbody.AddForce(bounceVector * c_bounceForce);
        if (bumpBuyer.IsScared())
        {
            _controller.Scare();
        }
    }

    private bool IsScared()
    {
        return _controller.IsScared();
    }
}
