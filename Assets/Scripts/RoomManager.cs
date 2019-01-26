using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager _instance;

    [SerializeField]
    private List<Room> _rooms;

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }
}
