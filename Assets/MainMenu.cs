using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadLevelSelect()
    {
        Sceneloader.s_Instance.LoadScene("LevelSelection");
    }
}
