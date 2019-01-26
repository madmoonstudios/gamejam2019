using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeparentOnAwake : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        this.transform.position = this.transform.parent.position + Vector3.up;
        this.transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
