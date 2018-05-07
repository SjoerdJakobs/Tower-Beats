using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

#region Serializable Data Classes

[System.Serializable]
public class GridPath
{
    /// <summary>
    /// Constructor of a GridPath
    /// </summary>
    /// <param name="Name">Name of the GridPath</param>
    /// <param name="GridSize">Grid size of when the path was created (for loading)</param>
    /// <param name="Path">The path with all its nodes</param>
    public GridPath(string Name, Vector2Int GridSize, List<Vector2Int> Path)
    {
        this.Name = Name;
        this.GridSize = GridSize;
        this.Path = Path;
    }

    public string Name;
    public Vector2Int GridSize;
    public List<Vector2Int> Path;
}

[System.Serializable]
public class PathData
{
    public List<GridPath> Paths = new List<GridPath>();
}

#endregion

public class PathManager : MonoBehaviour
{
    #region Variables

    public static PathManager s_Instance;

    private string m_FilePath;
    private PathData m_PathData;

    private List<Vector3> m_CurrentPathNodes = new List<Vector3>();
    public List<Vector3> CurrentPathNodes { get { return m_CurrentPathNodes; } }

    #endregion

    #region Initialization

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Gets called upon initialization
    /// </summary>
    private void Init()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_PathData = new PathData();
        m_FilePath = Path.Combine(Application.dataPath, "Data/Paths.json");

        LoadData();
    }

    #endregion

    #region Utils

    /// <summary>
    /// Returns a GridPath by name
    /// </summary>
    /// <param name="name">Name of the GridPath</param>
    /// <returns>A GridPath by name</returns>
    public GridPath GetPathByName(string name)
    {
        GridPath data = m_PathData.Paths.Find(x => x.Name.ToLower() == name.ToLower());
        if (data != null)
            return data;
        else
            return null;
    }

    /// <summary>
    /// Checks if a path name already exists
    /// </summary>
    /// <param name="name">Name of the path</param>
    /// <returns>If the entered path name already exists in the files</returns>
    public bool PathNameExists(string name)
    {
        for (int i = 0; i < m_PathData.Paths.Count; i++)
        {
            if (m_PathData.Paths[i].Name.ToLower() == name.ToLower())
                return true;
        }
        return false;
    }

    #endregion

    #region Save and Load Paths

    /// <summary>
    /// Saves a path
    /// </summary>
    /// <param name="pathdata">Data of the path</param>
    public void SavePath(GridPath pathdata)
    {
        m_PathData.Paths.Add(pathdata);
        print(m_PathData.Paths[0].Name);
        print(m_PathData.Paths.Count);

        SaveData();
    }

    /// <summary>
    /// Saves a path
    /// </summary>
    /// <param name="name">Name of the GridPath</param>
    /// <param name="gridSize">Grid size of when the path was created (for loading)</param>
    /// <param name="path">The path with all its nodes</param>
    public void SavePath(string name, Vector2Int gridSize, List<Vector2Int> path)
    {
        SavePath(new GridPath(name, gridSize, path));
    }

    /// <summary>
    /// Loads a path by name
    /// </summary>
    /// <param name="pathName">Name of the path</param>
    public void LoadPath(string pathName)
    {
        GridPath pathData = GetPathByName(pathName);

        if (pathData == null) return;

        m_CurrentPathNodes.Clear();

        HexGrid.s_Instance.DestroyGrid(false);
        HexGrid.s_Instance.CreateGrid(pathData.GridSize.x, pathData.GridSize.y);

        for (int i = 0; i < pathData.Path.Count; i++)
        {
            Tile tile = HexGrid.s_Instance.GetTile(pathData.Path[i]);
            tile.SetTileVisualsState(TileVisualState.PATH);
            tile.CurrentState = TileState.PATH;
            m_CurrentPathNodes.Add(tile.transform.position);
        }
    }

    #endregion

    #region Save And Load Data

    /// <summary>
    /// Loads all the data
    /// </summary>
    public void LoadData()
    {
        TryCreateFile();

        string jsonString = File.ReadAllText(m_FilePath);
        JsonUtility.FromJsonOverwrite(jsonString, m_PathData);
    }

    /// <summary>
    /// Saves all the data
    /// </summary>
    public void SaveData()
    {
        TryCreateFile();

        string jsonString = JsonUtility.ToJson(m_PathData, true);
        File.WriteAllText(m_FilePath, jsonString);
    }

    /// <summary>
    /// Tries to create a .JSON file
    /// </summary>
    private void TryCreateFile()
    {
        if (!File.Exists(m_FilePath))
            File.Create(m_FilePath).Dispose();
    }

    #endregion
}