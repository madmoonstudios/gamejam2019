using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtorController : MonoBehaviour
{
    public static Transform realtorTransform;
    
    void Awake()
    {
        realtorTransform = transform;
    }
}
