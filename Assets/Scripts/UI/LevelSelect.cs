using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    private int m_SelectedLevel;

    public void SelectLevel(int levelIndex)
    {
        m_SelectedLevel = levelIndex;
    }

    public void LoadSelectedLevel()
    {
        // Load level;
    }
}
