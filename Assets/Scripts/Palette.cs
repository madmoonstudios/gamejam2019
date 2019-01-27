using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{

    public static Color red, orange, green, yellow;

    //HACKY --  init is called from MoodIndicator instead of somewhere sensible
    public static void Init()
    {
        ColorUtility.TryParseHtmlString("#ad2b2bFF", out red);
        ColorUtility.TryParseHtmlString("#977d28FF", out orange);
        ColorUtility.TryParseHtmlString("#d6cd39FF", out yellow);
        ColorUtility.TryParseHtmlString("#5d9965FF", out green);
    }
}
