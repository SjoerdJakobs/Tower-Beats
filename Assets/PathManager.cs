using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;

[System.Serializable]
public class PathsData
{
    public List<GridPath> Paths = new List<GridPath>();
}

[System.Serializable]
public class GridPath
{
    public GridPath(string Name, List<Vector2Int> Path, Vector2Int GridSize)
    {
        this.Name = Name;
        this.Path = Path;
        this.GridSize = GridSize;
    }

    public string Name;
    public List<Vector2Int> Path;
    public Vector2Int GridSize;
}

[ExecuteInEditMode]
public class PathManager : MonoBehaviour
{
    public static PathManager s_Instance;

    private string m_FileName = "PathData.dat";

    private PathsData m_PathsData;
    //public PathsData PathsData { get { return m_PathsData; } }

    private void Awake()
    {
        Init();
    }

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

        Load();
    }

    public void SavePath(string pathName, List<Tile> tiles, Vector2Int gridSize)
    {
        List<Vector2Int> tilePositions = new List<Vector2Int>();

        for (int i = 0; i < tiles.Count; i++)
            tilePositions.Add(tiles[i].PositionInGrid);

        GridPath path = new GridPath(pathName, tilePositions, gridSize);

        SavePath(path);
    }

    public void SavePath(GridPath path)
    {
        m_PathsData.Paths.Add(path);
        Save();
    }

    public GridPath LoadPath(string pathName)
    {
        Load();

        GridPath path = m_PathsData.Paths.Find(x => x.Name == pathName);

        if (path != null)
            return path;
        else
            return null;
    }

    public void Save()
    {
        string savePath = Application.persistentDataPath + m_FileName;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);

        bf.Serialize(file, m_PathsData);
        file.Close();
    }

    public void Load()
    {
        string savePath = Application.persistentDataPath + m_FileName;

        if (!File.Exists(savePath))
        {
            m_PathsData = new PathsData();
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(savePath, FileMode.Open);
        file.Position = 0;

        m_PathsData = (PathsData)bf.Deserialize(file);
        file.Close();
    }

    public void Load2()
    {
       
    }
}
