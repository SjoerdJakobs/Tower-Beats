using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] private Transform m_EnemyContainer;
    [SerializeField] private Transform m_SpawnPosition;


    [SerializeField] private List<Enemy> m_Enemies = new List<Enemy>();

    /// <summary>
    /// Spawns a random enemy
    /// </summary>
    /// <returns></returns>
    public void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, m_Enemies.Count);
        Enemy newEnemy = Instantiate(m_Enemies[randomEnemy]);
        newEnemy.transform.position = m_EnemyContainer.position;
        newEnemy.transform.SetParent(m_EnemyContainer);
        //return newEnemy;
    }
}