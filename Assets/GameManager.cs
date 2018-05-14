using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance;
    private int m_PreparationTime = 5;

    public delegate void PreparationTimeUpdated(int time);
    public static PreparationTimeUpdated s_OnPreparationTimeUpdated;

    public delegate void GameStarted();
    public static GameStarted s_OnGameStart;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if(s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);

        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(StartPreparationTime(() => {
            print("start spawning enemies");
            if(s_OnGameStart != null) s_OnGameStart();
            EnemySpawner.s_Instance.SpawnWave(10, 1, () => { print("done spawning wave"); });
        }));

        CameraMovement.s_Instance.ScrollCameraToPosition(HexGrid.s_Instance.GetTile(10, 5).transform, 1, true, () => { print("done scrolling"); });
    }

    private IEnumerator StartPreparationTime(System.Action callback)
    {
        for (int i = m_PreparationTime; i >= 0; i--)
        {
            if(s_OnPreparationTimeUpdated != null) s_OnPreparationTimeUpdated(i);
            print("Time left: " + i);
            yield return new WaitForSeconds(1);
        }
        callback();
    }
}
