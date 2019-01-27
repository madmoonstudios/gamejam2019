using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InterestPointObjectType
{
    ART, FURNITURE
}

public class InterestPoint : MonoBehaviour
{
    public RoomType roomType; // We don't actually use this - remove if that continues to be the case!
    public InterestPointObjectType objectType;

    public void AddToLists()
    {
        InterestPointManager.AddInterestPoint(this);
    }
}
