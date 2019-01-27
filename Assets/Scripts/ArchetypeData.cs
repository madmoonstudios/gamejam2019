using UnityEngine;
using System.Collections;


[System.Serializable]
public class InterestPointData
{
    public string id;
}

[System.Serializable]
public class PhraseData
{
      public string text;
      public float alignment;
}


[System.Serializable]
public class ArchetypeData
{
    public string name;
    public float moveSpeed; // 0 to 1 number indicating how fast the buyers move when walking
    public float runSpeed; // 0 to 1 number indicating how fast the buyers move when running
    public float fearVulnerability; // 0 to 1 number indicating how much fearful events affect this character, "fear multiplier"
    public float fearResistance; // 0 to 1 number indicating how much fear decrement events affect player, inverse multiplier tofearVulnerability 
    public float buyingIntent; // number from 0 to 100 of how much buying intent vs fear the character has
    public float fearStamina; // number from 1 to 100 indicating how much fear stamina it has "health bar
    public InterestPointData[] interestPoints;
    public PhraseData[] phrases;
}

[System.Serializable]
public class GlobalConfigData
{
    public ArchetypeData[] archetypes;
}


[System.Serializable]
public class BuyerSpawn {
    public float waitSeconds;
    public string name;
    public InterestPoint[] interestPoints;
}

[System.Serializable]
public class WaveData 
{
    public string name;
    public BuyerSpawn[] buyers;
}

[System.Serializable]
public class WavesConfigData
{
    public WaveData[] waves;
}