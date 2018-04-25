using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathLoader : MonoBehaviour
{
    [SerializeField]private string m_PathToLoad;

    private void Start()
    {
        PathManager.s_Instance.LoadPath(m_PathToLoad);
    }
}
