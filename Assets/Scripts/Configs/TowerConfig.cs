using System.Collections.Generic;

public class TowerConfig
{
    public static Dictionary<string, List<TowerData>> s_Towers = new Dictionary<string, List<TowerData>>()
    {
        // TYPE, LEVEL, MAXLEVEL, UPGRADECOST, BUYCOST, VALUE (Sell), DAMAGE, RANGE, INTERVAL
        {TowerTypeTags.BASS_TOWER, new List<TowerData>() {
            new TowerData(TowerTypeTags.BASS_TOWER, 1, 3, 200, 100, 100, 240f, 500f, 0.4f),
            new TowerData(TowerTypeTags.BASS_TOWER, 2, 3, 200, 0, 500, 500f, 600f, 0.25f),
            new TowerData(TowerTypeTags.BASS_TOWER, 3, 3, 200, 0, 700, 720f, 700f, 0.2f)
        }},
        {TowerTypeTags.DRUM_TOWER, new List<TowerData>()
        {
            new TowerData(TowerTypeTags.DRUM_TOWER, 1, 3, 200, 300, 300, 260f, 510f, 0.35f)
        }},
        {TowerTypeTags.SYNTH_TOWER, new List<TowerData>()
        {
            new TowerData(TowerTypeTags.SYNTH_TOWER, 1, 3,200,250, 250,300,300,0.4f)
        }}
    };
}