using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner s_Instance;
    
    [SerializeField]
    private List<GameObject> m_EnemyPrefabs;

    [SerializeField]
    private List<GameObject> m_Bosses;

    public List<Enemy> SpawnedEnemies = new List<Enemy>();
    private bool m_Paused;
    private Coroutine m_SpawnEnemies;
    
    public List<ObjectPool> EnemyPools = new List<ObjectPool>();
    public List<ObjectPool> BossPools = new List<ObjectPool>();

    [SerializeField] private ParticleSystem m_SpawnerParticles;

    private int m_BossWaveCounter = 0;
    private int m_WaveCounter = 0;

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }

        Enemy.s_OnDestroyEnemy += RemoveEnemyFromList;
        GameManager.s_OnGameStop += StopEnemySpawning;
        PauseCheck.Pause += TogglePause;
        MapLoader.s_OnMapLoaded += OnMapLoaded;
    }

    private void Start()
    {
        for (int i = 0; i < m_EnemyPrefabs.Count; i++)
        {
            EnemyPools.Add(ObjectPoolManager.s_Instance.GetObjectPool(m_EnemyPrefabs[i].gameObject, 5, 5, int.MaxValue, int.MaxValue, false, PooledSubObject.Enemy));
        }
        for (int i = 0; i < m_Bosses.Count; i++)
        {
            BossPools.Add(ObjectPoolManager.s_Instance.GetObjectPool(m_Bosses[i].gameObject, 5, 5, int.MaxValue, int.MaxValue, false, PooledSubObject.Enemy));
        }
    }

    /// <summary>
    /// Callback for when the map has been loaded
    /// </summary>
    private void OnMapLoaded()
    {
        m_SpawnerParticles.gameObject.transform.position = MapLoader.s_Instance.Path[0].transform.position;
        MapLoader.s_OnMapLoaded -= OnMapLoaded;
    }

    /// <summary>
    /// Spawns a random enemy
    /// </summary>
    /// <returns></returns>
    public void SpawnEnemy(bool isBoss)
    {
        Enemy newEnemy = null;
        if (!isBoss)
        {
            newEnemy = EnemyPools[UnityEngine.Random.Range(0, EnemyPools.Count)].GetFromPool().GenericObj as Enemy;
            if (m_WaveCounter > 2)
            {
                newEnemy.SetMaxHealth((int)(20 + ((m_WaveCounter - 2) * 3)+((m_WaveCounter*m_WaveCounter)/40)));
            }
        }
        else
        {
            newEnemy = BossPools[UnityEngine.Random.Range(0, BossPools.Count)].GetFromPool().GenericObj as Enemy;
            if (m_WaveCounter > 2)
            {
                newEnemy.SetMaxHealth((int)(40 + ((m_WaveCounter - 2) * 8) + ((m_WaveCounter * m_WaveCounter) / 20)));
            }
        }
        newEnemy.RestoreHealth();
        newEnemy.IsAlive = true;
        newEnemy.SkeletonAnims.AnimationState.SetAnimation(0, newEnemy.EnemyString + "MOVE", true);

        SpawnedEnemies.Add(newEnemy);
              
        newEnemy.transform.position = MapLoader.s_Instance.Path[0].transform.position;

        EffectsManager.s_Instance.SpawnEffect(EffectType.ENEMY_SPAWN, false, MapLoader.s_Instance.Path[0].transform.position);

        newEnemy.Move(MapLoader.s_Instance.Path[0].transform.position);
    }

    private void TogglePause(bool Pause)
    {
        m_Paused = Pause;
    }

    public void SpawnWave(int amountOfEnemies, float interval, Action callback = null)
    {
        if (m_BossWaveCounter < 3)
        {
            m_SpawnEnemies = StartCoroutine(SpawnEnemies(amountOfEnemies, interval, callback));
            m_BossWaveCounter++;
            m_WaveCounter++;
        }
        else if(m_BossWaveCounter >= 3)
        {
            m_SpawnEnemies = StartCoroutine(SpawnBossWave(amountOfEnemies, interval, callback));
            m_BossWaveCounter = 0; //resets wave count
            m_WaveCounter++;
        }
        else
        {
            //Spawn no wave, just in case
        }
    }

    private IEnumerator SpawnEnemies(int amountOfEnemies, float interval, Action callback = null)
    {
        yield return new WaitForSeconds(1f);
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
        yield return new WaitForSeconds(2);
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