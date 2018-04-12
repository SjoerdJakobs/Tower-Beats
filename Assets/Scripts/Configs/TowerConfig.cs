using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerConfig
{
    public static Dictionary<TowerTypes, List<TowerData>> s_Towers = new Dictionary<TowerTypes, List<TowerData>>()
    {
        // TYPE, LEVEL, COSTS, DAMAGE, RANGE, INTERVAL
        {TowerTypes.BASS_TOWER, new List<TowerData>() {
            new TowerData(TowerTypes.BASS_TOWER, 1, 200, 100, 300, 240f, 500f, 0.4f),
            new TowerData(TowerTypes.BASS_TOWER, 2, 200, 0, 500, 500f, 240f, 0.25f)
        }},
        {TowerTypes.DRUM_TOWER, new List<TowerData>()
        {
            new TowerData(TowerTypes.DRUM_TOWER, 1, 200, 100, 300, 260f, 510f, 0.35f)
        }}
    };
}
