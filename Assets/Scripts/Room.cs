using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    KITCHEN, LIVING_ROOM, BEDROOM, BATHROOM
}

public class Room : MonoBehaviour
{
    public static List<Room> allRooms = new List<Room>();
    [SerializeField] private RoomType _roomType;

    void Awake()
    {
        transform.position = new Vector3(transform.position.x, 2, transform.position.z); // Hack to get everything on the same y
        
        allRooms.Add(this);
        foreach (InterestPoint interestPoint in GetComponentsInChildren<InterestPoint>())
        {
            interestPoint.roomType = _roomType;
            interestPoint.transform.position = new Vector3(interestPoint.transform.position.x, 2, interestPoint.transform.position.z);
            interestPoint.AddToLists();
        }
    }
}
