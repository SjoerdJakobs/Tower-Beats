using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner s_Instance;

    [SerializeField] private List<Enemy> m_Enemies = new List<Enemy>();
    public List<Enemy> SpawnedEnemies = new List<Enemy>();

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }

        Enemy.s_OnDestroyEnemy += RemoveEnemyFromList;
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
        newEnemy.Move();
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
            yield return new WaitForSeconds(interval);
        }

        if(callback != null)
            callback();
    }

    void RemoveEnemyFromList(Enemy enemy)
    {
        SpawnedEnemies.Remove(enemy);
    }
}