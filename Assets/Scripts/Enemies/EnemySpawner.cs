using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner s_Instance;

    [SerializeField] private List<Enemy> m_Enemies = new List<Enemy>();
    public List<Enemy> SpawnedEnemies = new List<Enemy>();
    private bool m_Paused;

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }

        Enemy.s_OnDestroyEnemy += RemoveEnemyFromList;
        PauseCheck.Pause += TogglePause;
    }

    /// <summary>
    /// Spawns a random enemy
    /// </summary>
    /// <returns></returns>
    public void SpawnEnemy()
    {
        int randomEnemy = UnityEngine.Random.Range(0, m_Enemies.Count);
        Enemy newEnemy = Instantiate(m_Enemies[randomEnemy]);
        SpawnedEnemies.Add(newEnemy);
        newEnemy.transform.position = PathManager.s_Instance.CurrentPathNodes[0];
        newEnemy.transform.SetParent(transform);
        EffectsManager.s_Instance.SpawnEffect(EffectType.ENEMY_SPAWN, false, PathManager.s_Instance.CurrentPathNodes[0]);
        newEnemy.Move();
    }

    private void TogglePause(bool Pause)
    {
        m_Paused = Pause;
    }

    public void SpawnWave(int amountOfEnemies, float interval, Action callback = null)
    {
        StartCoroutine(SpawnEnemies(amountOfEnemies, interval, callback));
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
}