using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner s_Instance;

    [SerializeField] private List<Enemy> m_Enemies = new List<Enemy>();
    [SerializeField] private List<Enemy> m_Bosses = new List<Enemy>();
    public List<Enemy> SpawnedEnemies = new List<Enemy>();
    private bool m_Paused;
    private Coroutine m_SpawnEnemies;

    private int m_Wave = 0;

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }

        Enemy.s_OnDestroyEnemy += RemoveEnemyFromList;
        GameManager.s_OnGameStop += StopEnemySpawning;
        PauseCheck.Pause += TogglePause;
    }

    /// <summary>
    /// Spawns a random enemy
    /// </summary>
    /// <returns></returns>
    public void SpawnEnemy(bool isBoss)
    {
        int randomEnemy = UnityEngine.Random.Range(0, m_Enemies.Count);
        int randomBoss = UnityEngine.Random.Range(0, m_Bosses.Count);

        Enemy newEnemy;

        if (m_Enemies[randomEnemy] == null)
            return;

        if (!isBoss)
        {
            newEnemy = Instantiate(m_Enemies[randomEnemy]);
            SpawnedEnemies.Add(newEnemy);
        }
        else
        {
            newEnemy = Instantiate(m_Bosses[randomBoss]);
            SpawnedEnemies.Add(newEnemy);
        }
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
        if (m_Wave < 3)
        {
            m_SpawnEnemies = StartCoroutine(SpawnEnemies(amountOfEnemies, interval, callback));
            m_Wave++;
            Debug.Log("spawn normal wave");
        }
        else if(m_Wave >= 3)
        {
            m_SpawnEnemies = StartCoroutine(SpawnBossWave(amountOfEnemies, interval, callback));
            m_Wave = 0; //resets wave count
            Debug.Log("spawn boss wave");
        }
        else
        {
            Debug.Log("spawn no wave");
        }
    }

    private IEnumerator SpawnEnemies(int amountOfEnemies, float interval, Action callback = null)
    {
        for (int i = 0; i < amountOfEnemies; i++)
        {
            SpawnEnemy(false);

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

    private IEnumerator SpawnBossWave(int amountOfEnemies, float interval, Action callback = null)
    {
        for (int i = 0; i < amountOfEnemies; i++)
        {
            SpawnEnemy(false);
            yield return new WaitForSeconds(interval);
        }
        yield return new WaitForSeconds(interval);
        SpawnEnemy(true);

        if(callback != null)
        {
            callback();
        }

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
        GameManager.s_OnGameStop -= StopEnemySpawning;
        Enemy.s_OnDestroyEnemy -= RemoveEnemyFromList;
    }
}