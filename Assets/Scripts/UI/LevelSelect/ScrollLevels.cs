using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScrollLevels : MonoBehaviour {

    [SerializeField] private Transform[] m_LevelPositions;
    [SerializeField] private List<GameObject> m_Levels = new List<GameObject>();
    [SerializeField] private RectTransform m_SelectedLevelPosition;

    public static ScrollLevels s_Instance;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);

        if (MapInfo.s_OnLevelChange != null)
            MapInfo.s_OnLevelChange(GetSelectedLevel());
    }

    public void NextLevel()
    {
        RepositionLevels(1);
    }

    public void PreviousLevel()
    {
        RepositionLevels(-1);
    }

    void RepositionLevels(int direction)
    {
        if(direction == 1)
        {
            for (int i = 0; i < m_Levels.Count; i++)
            {
                if (i <= 0)
                    m_Levels[i].transform.DOMove(m_LevelPositions[m_LevelPositions.Length - 1].transform.position, 0.1f);
                else
                    m_Levels[i].transform.DOMove(m_LevelPositions[i - 1].transform.position, 0.2f);
            }
        }
        else
        {
            for (int i = 0; i < m_Levels.Count; i++)
            {
                if(i >= m_Levels.Count - 1)
                    m_Levels[i].transform.DOMove(m_LevelPositions[0].transform.position, 0.1f);
                else
                    m_Levels[i].transform.DOMove(m_LevelPositions[i + 1].transform.position, 0.2f);
            }
        }

        RepositionList(direction);
    }

    /// <summary>
    /// Reorders the list.
    /// </summary>
    /// <param name="direction"></param>
    public void RepositionList(int direction)
    {
        List<GameObject> tmpList = new List<GameObject>();

        for (int i = 0; i < m_Levels.Count; i++)
        {
            tmpList.Add(m_Levels[i]);
        }

        //Clear old list
        m_Levels.Clear();

        if(direction == 1)
        {
            int lastIndex = tmpList.Count - 1;
            //Reorder the list so that everything moves down 1 slot.

            for (int i = 0; i < tmpList.Count; i++)
            {
                if (i != lastIndex)
                {
                    m_Levels.Add(tmpList[i + 1]);
                }
                else
                {
                    m_Levels.Add(tmpList[0]);
                }
            }
        }
        else
        {
            m_Levels.Add(tmpList[tmpList.Count - 1]);
            tmpList.RemoveAt(tmpList.Count - 1);

            for (int i = 0; i < tmpList.Count; i++)
            {
                m_Levels.Add(tmpList[i]);
            }
        }

        MapInfo.s_OnLevelChange(GetSelectedLevel());
    }

    public Level GetSelectedLevel()
    {
        Level selectedLevel = m_Levels[1].GetComponent<Level>();
        return selectedLevel;
    }

    private void ResetButtonColors()
    {
        for (int i = 0; i < m_Levels.Count; i++)
        {
            m_Levels[i].GetComponent<Button>().image.color = Color.white;
        }
    }
}
