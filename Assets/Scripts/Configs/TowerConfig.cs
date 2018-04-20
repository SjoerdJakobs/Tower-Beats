using System.Collections.Generic;

public class TowerConfig
{
    public static Dictionary<string, List<TowerData>> s_Towers = new Dictionary<string, List<TowerData>>()
    {
        // TYPE, LEVEL, MAXLEVEL, UPGRADECOST, BUYCOST, VALUE (Sell), DAMAGE, RANGE, INTERVAL
        {TowerTypeTags.BASS_TOWER, new List<TowerData>() {
            new TowerData(TowerTypeTags.BASS_TOWER, 1, 3, 200, 100, 75, 240f, 5f, 0.4f),
            new TowerData(TowerTypeTags.BASS_TOWER, 2, 3, 200, 0, 225, 500f, 6f, 0.25f),
            new TowerData(TowerTypeTags.BASS_TOWER, 3, 3, 0, 0, 375, 720f, 7f, 0.2f)
        }},
        {TowerTypeTags.DRUM_TOWER, new List<TowerData>()
        {
            new TowerData(TowerTypeTags.DRUM_TOWER, 1, 3, 200, 300, 225, 260f, 5.1f, 0.35f),
            new TowerData(TowerTypeTags.DRUM_TOWER, 2, 3, 300, 0, 375, 300f,5.5f,0.3f),
            new TowerData(TowerTypeTags.DRUM_TOWER, 3, 3, 0, 0 , 600, 360, 6f, 0.25f)
        }},
        {TowerTypeTags.SYNTH_TOWER, new List<TowerData>()
        {
            new TowerData(TowerTypeTags.SYNTH_TOWER, 1, 3, 200, 200, 150,300,3f,0.4f),
            new TowerData(TowerTypeTags.SYNTH_TOWER, 2, 3, 300, 0, 300, 600, 4.5f, 0.3f),
            new TowerData(TowerTypeTags.SYNTH_TOWER, 3, 3, 0, 0, 525, 900, 5.5f, 0.2f)
        }}
    };
}