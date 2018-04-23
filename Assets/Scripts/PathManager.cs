using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[System.Serializable]
public class GridPath
{
    public GridPath(string Name, List<Vector2> Path)
    {
        this.Name = Name;
        this.Path = Path;
    }

    public string Name;
    public List<Vector2> Path;
}

public class PathData
{
    public List<GridPath> Paths;
}

public class PathManager : MonoBehaviour
{
    private string m_FilePath;
    public static PathManager s_Instance;

    private PathData m_PathData;

    private void Awake()
    {
        m_PathData = new PathData();
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

        m_FilePath = Path.Combine(Application.dataPath, "Data/Paths.json");

        LoadData();
    }

    public GridPath GetPathByName(string name)
    {
        GridPath data = m_PathData.Paths.Find(x => x.Name == name);
        if (data != null)
            return data;
        else
            return null;
    }

    public void SavePath(GridPath pathdata)
    {
        m_PathData.Paths.Add(pathdata);
        print(m_PathData.Paths[0].Name);
        print(m_PathData.Paths.Count);

        SaveData();
    }
    

    public void SavePath(string name, List<Vector2> path)
    {
        m_PathData.Paths.Add(new GridPath(name, path));
        SaveData();
    }

    public void LoadData()
    {
        m_PathData.Paths = new List<GridPath>();
        string jsonString = File.ReadAllText(m_FilePath);

        JsonUtility.FromJsonOverwrite(jsonString, m_PathData);
    }

    public void SaveData()
    {
        if(!File.Exists(m_FilePath))
        {
            File.Create(m_FilePath);
        }



        string jsonString = JsonUtility.ToJson(m_PathData, true);
        print("Saving: " + jsonString);
        File.WriteAllText(m_FilePath, jsonString);
    }
}