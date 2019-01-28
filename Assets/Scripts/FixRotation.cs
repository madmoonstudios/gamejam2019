using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
    Quaternion rotation;

    void Awake()
    {
        rotation = transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
        transform.position = transform.parent.position + new Vector3(-1.6f, 6, 1.4f);
    }
}
