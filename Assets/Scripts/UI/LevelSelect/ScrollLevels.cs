using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScrollLevels : MonoBehaviour {

    [SerializeField] private Transform[] m_LevelPositions;
    [SerializeField] private List<GameObject> m_Levels = new List<GameObject>();

    private bool m_IsAnimating;

    public static ScrollLevels s_Instance;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
            Destroy(gameObject);

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        if(MapInfo.s_OnLevelChange != null)
            MapInfo.s_OnLevelChange(GetSelectedLevel());
    }

    public void NextLevel()
    {
        RepositionLevels(-1);
    }

    public void PreviousLevel()
    {
        RepositionLevels(1);
    }

    IEnumerator WaitForTween()
    {
        m_IsAnimating = true;
        yield return new WaitForSeconds(0.15f);
        m_IsAnimating = false;
    }

    void RepositionLevels(int direction)
    {
        if (!m_IsAnimating)
        {
            SoundManager.s_Instance.PlaySound(SoundNames.LEVELSELECTSCROLLSOUND);

            StartCoroutine(WaitForTween());

            if (direction == 1)
            {
                for (int i = 0; i < m_Levels.Count; i++)
                {
                    if (i <= 0)
                        m_Levels[i].transform.DOMove(m_LevelPositions[m_LevelPositions.Length - 1].transform.position, 0.05f);
                    else
                        m_Levels[i].transform.DOMove(m_LevelPositions[i - 1].transform.position, 0.2f);
                }
            }
            else
            {
                for (int i = 0; i < m_Levels.Count; i++)
                {
                    if (i >= m_Levels.Count - 1)
                        m_Levels[i].transform.DOMove(m_LevelPositions[0].transform.position, 0.05f);
                    else
                        m_Levels[i].transform.DOMove(m_LevelPositions[i + 1].transform.position, 0.2f);
                }
            }

            RepositionList(direction);
        }
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
        SetColors();
        Level selectedLevel = m_Levels[2].GetComponent<Level>();
        return selectedLevel;
    }

    private void SetColors()
    {
        for (int i = 0; i < m_Levels.Count; i++)
        {
            if(i != 2)
            {
                Image[] images = m_Levels[i].GetComponentsInChildren<Image>();

                foreach (Image image in images)
                {
                   if(image != m_Levels[i].GetComponent<Image>())
                    {
                        image.color = Color.gray;
                    } 
                }
            }
            else
            {
                Image[] images = m_Levels[i].GetComponentsInChildren<Image>();

                foreach (Image image in images)
                {
                    if (image != m_Levels[i].GetComponent<Image>())
                    {
                        image.color = Color.white;
                    }
                }
            }
        }
    }

    private void ResetButtonColors()
    {
        for (int i = 0; i < m_Levels.Count; i++)
        {
            m_Levels[i].GetComponent<Button>().image.color = Color.white;
        }
    }
}
