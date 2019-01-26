using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterestPointObjectType
{
    ART, FURNITURE
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
