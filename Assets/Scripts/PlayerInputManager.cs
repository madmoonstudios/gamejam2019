using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerInteractionManager._instance.TryDropPentagram();
        }
    }
}
