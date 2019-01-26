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
}
