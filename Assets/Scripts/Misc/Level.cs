using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public enum Mapsizes
    {
        Small,
        Medium,
        Large,
        
    }

    public enum MapDifficulties
    {
        Easy,
        Normal,
        Hard
    }

    public int TurretPlacements;

    [Header("Map size")]
    public Mapsizes MapSize;

    [Header("Map difficulty")]
    public MapDifficulties MapDifficulty;

    [Header("Map name")]
    public string MapName;

    [Space(5)]
    public string[] Songs = new string[3];
}
