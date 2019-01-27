using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
        allRooms.Add(this);
        foreach (InterestPoint interestPoint in GetComponentsInChildren<InterestPoint>())
        {
            interestPoint.roomType = _roomType;
            interestPoint.AddToLists();
        }
    }
}
