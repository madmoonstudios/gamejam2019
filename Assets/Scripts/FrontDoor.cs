using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoor : MonoBehaviour
{
    public static Transform frontDoorTransform;
    
    void Awake()
    {
        frontDoorTransform = transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        BuyerController buyer = collision.transform.GetComponent<BuyerController>();
        if (buyer != null)
        {
            buyer.TryDestroy();
        }
    }
}
