﻿using System.Collections;
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
        StartCoroutine(ShowTutorial(() => {
            StartCoroutine(StartPreparationTime(() => {
                print("start spawning enemies");
                if (s_OnGameStart != null) s_OnGameStart();
                SpawnContinuousWaves();
            }));
        }));   
    }

    private void SpawnContinuousWaves()
    {
        EnemySpawner.s_Instance.SpawnWave(10, 1, () => { SpawnContinuousWaves(); });
    }

    private IEnumerator StartPreparationTime(System.Action callback)
    {
        for (int i = m_PreparationTime; i >= 0; i--)
        {
            //if(s_OnPreparationTimeUpdated != null) s_OnPreparationTimeUpdated(i);
            NotificationManager.s_Instance.EnqueueNotification((i == 0 ? "Enemies incoming" : i.ToString()), (i == 0 ? 1.5f : 0.5f));
            print("Time left: " + i);
            yield return new WaitForSeconds(1);
        }
        callback();
    }

    private IEnumerator ShowTutorial(System.Action callback)
    {
        NotificationManager.s_Instance.EnqueueNotification("Your objective is to defend this tower in Hexagonia.", 2);
        CameraMovement.s_Instance.ScrollCameraToPosition(HexGrid.s_Instance.GetTile(10, 5).transform, 1, false);
        yield return new WaitForSeconds(2.5f);
        NotificationManager.s_Instance.EnqueueNotification("Enemies will spawn from here", 2);
        CameraMovement.s_Instance.ScrollCameraToPosition(PathManager.s_Instance.CurrentPathNodes[0], 1, false);
        yield return new WaitForSeconds(2.5f);
        CameraMovement.s_Instance.CanMoveCamera = true;
        if (callback != null) callback();
    }
}