using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public delegate void StopSpawning();
    public static StopSpawning s_OnStopSpawning;

    public static EnemySpawner s_Instance;

    [SerializeField] private List<Enemy> m_Enemies = new List<Enemy>();
    public List<Enemy> SpawnedEnemies = new List<Enemy>();
    private bool m_Paused;
    private Coroutine m_SpawnEnemies;

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }

        Enemy.s_OnDestroyEnemy += RemoveEnemyFromList;
        s_OnStopSpawning += StopEnemySpawning;
        PauseCheck.Pause += TogglePause;
    }

    /// <summary>
    /// Spawns a random enemy
    /// </summary>
    /// <returns></returns>
    public void SpawnEnemy()
    {
        int randomEnemy = UnityEngine.Random.Range(0, m_Enemies.Count);

        if (m_Enemies[randomEnemy] == null)
            return;

        Enemy newEnemy = Instantiate(m_Enemies[randomEnemy]);
        SpawnedEnemies.Add(newEnemy);
        newEnemy.transform.position = MapLoader.s_Instance.Path[0].transform.position;//PathManager.s_Instance.CurrentPathNodes[0];
        newEnemy.transform.SetParent(transform);
        EffectsManager.s_Instance.SpawnEffect(EffectType.ENEMY_SPAWN, false, MapLoader.s_Instance.Path[0].transform.position);
        newEnemy.Move();
    }

    private void TogglePause(bool Pause)
    {
        m_Paused = Pause;
    }

    public void SpawnWave(int amountOfEnemies, float interval, Action callback = null)
    {
        m_SpawnEnemies = StartCoroutine(SpawnEnemies(amountOfEnemies, interval, callback));
    }

    private IEnumerator SpawnEnemies(int amountOfEnemies, float interval, Action callback = null)
    {
        for (int i = 0; i < amountOfEnemies; i++)
        {
            SpawnEnemy();

            float timer = 0;
            while (timer <= interval)
            {
                if (!m_Paused)
                {
                    timer += Time.deltaTime;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        if(callback != null)
            callback();
    }

    void RemoveEnemyFromList(Enemy enemy)
    {
        SpawnedEnemies.Remove(enemy);
    }

    void StopEnemySpawning()
    {
        StopCoroutine(m_SpawnEnemies);
    }

    private void OnDestroy()
    {
        s_OnStopSpawning -= StopEnemySpawning;
        Enemy.s_OnDestroyEnemy -= RemoveEnemyFromList;
    }
}