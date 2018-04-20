using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner s_Instance;
    [SerializeField] private Transform m_EnemyContainer;

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
        int randomEnemy = Random.Range(0, m_Enemies.Count);
        Enemy newEnemy = Instantiate(m_Enemies[randomEnemy]);
        SpawnedEnemies.Add(newEnemy);
        newEnemy.transform.position = m_EnemyContainer.position;
        newEnemy.transform.SetParent(m_EnemyContainer);
    }

    void RemoveEnemyFromList(Enemy enemy)
    {
        SpawnedEnemies.Remove(enemy);
    }
}