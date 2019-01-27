using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum InterestPointObjectType
{
    NONE, ART, FURNITURE
}

public class InterestPoint : MonoBehaviour
{
    public RoomType roomType;
    public InterestPointObjectType objectType;

    public void AddToLists()
    {
        InterestPointManager.AddInterestPoint(this);
    }
}
