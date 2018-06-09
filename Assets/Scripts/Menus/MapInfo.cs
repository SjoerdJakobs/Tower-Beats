using UnityEngine;
using UnityEngine.UI;

public class MapInfo : MonoBehaviour {

    public delegate void OnLevelChange(Level selectedLevel);
    public static OnLevelChange s_OnLevelChange;

    [Header("General level info")]
    [SerializeField] private Text m_TurretPlacement;
    [SerializeField] private Text m_MapSize;
    [SerializeField] private Text m_MapDifficulty;

    [Header("Song names")]
    [SerializeField] private Text[] m_Songs = new Text[3];

    private void OnEnable()
    {
        s_OnLevelChange += SetMapInfo;
    }

    private void SetMapInfo(Level selectedLevel)
    {
        if(selectedLevel != null)
        {
            m_TurretPlacement.text = selectedLevel.TurretPlacements.ToString();
            m_MapSize.text = selectedLevel.MapSize.ToString();
            m_MapDifficulty.text = selectedLevel.MapDifficulty.ToString();

            for (int i = 0; i < m_Songs.Length; i++)
            {
                m_Songs[i].text = selectedLevel.Songs[i];
            }
        }
    }

    private void OnDisable()
    {
        s_OnLevelChange -= SetMapInfo;
    }
}