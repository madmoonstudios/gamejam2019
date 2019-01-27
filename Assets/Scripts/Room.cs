using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum RoomType
{
    KITCHEN, LIVING_ROOM, BEDROOM, BATHROOMS, CLOSETS, STUDY
}

public class Room : MonoBehaviour
{
    public static List<Room> allRooms = new List<Room>();
    public static Dictionary<RoomType, Room> roomsMap = new Dictionary<RoomType, Room>();
    private InterestPoint[] _myInterestPoints;
    [SerializeField] private RoomType _roomType;

    void Awake()
    {
        transform.position = new Vector3(transform.position.x, 2, transform.position.z); // Hack to get everything on the same y
        _myInterestPoints = GetComponentsInChildren<InterestPoint>();
        
        allRooms.Add(this);
        roomsMap[_roomType] = this;
        foreach (InterestPoint interestPoint in GetComponentsInChildren<InterestPoint>())
        {
            interestPoint.roomType = _roomType;
            interestPoint.transform.position = new Vector3(interestPoint.transform.position.x, 2, interestPoint.transform.position.z);
            interestPoint.AddToLists();
        }
    }

    public InterestPoint GetRandomInterestPoint()
    {
        int index = UnityEngine.Random.Range(0, _myInterestPoints.Length);
        return _myInterestPoints[index];
    }
}
