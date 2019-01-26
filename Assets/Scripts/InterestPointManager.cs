using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterestPointManager
{
    // TODO(samkern): Make this less general; specify by room and object type
    private static List<InterestPoint> _allInterestPoints;

    public static List<InterestPoint> allInterestPoints
    {
        get
        {
            if (_allInterestPoints == null)
                _allInterestPoints = new List<InterestPoint>();
            return _allInterestPoints;
        }
    }

    public static void AddInterestPoint(InterestPoint interestPoint)
    {
        allInterestPoints.Add(interestPoint);
    }
}
