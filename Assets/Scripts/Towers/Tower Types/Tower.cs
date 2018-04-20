using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct TowerData
{
    public TowerData(string Type, int Level, int MaxLevel, float UpgradeCost,float BuyCost,float SellValue, float AttackDamage, float AttackRange, float AttackInterval){
        this.Type = Type;
        this.Level = Level;
        this.MaxLevel = MaxLevel;
        this.UpgradeCost = UpgradeCost;
        this.BuyCost = BuyCost;
        this.SellValue = SellValue;
        this.AttackDamage = AttackDamage;
        this.AttackRange = AttackRange;
        this.AttackInterval = AttackInterval;
    }

    public string Type;
    public int Level;
    public int MaxLevel;
    public float UpgradeCost;
    public float BuyCost;
    public float SellValue;
    public float AttackDamage;
    public float AttackRange;
    public float AttackInterval;
}

public class Tower : MonoBehaviour
{
    protected TowerData m_TowerData;
    public TowerData TowerData { get; set; }
    [SerializeField]private List<Enemy> m_EnemiesInRange = new List<Enemy>();
    protected Enemy m_Target;
    protected bool m_ReadyToAttack = true;
    protected bool m_StartedCooldown;

    public Tower Self { get; set; } //Reference to itself


    private void Awake()
    {
        Enemy.s_OnDestroyEnemy += RemoveEnemyFromList;
    }

    private void Update()
    {
        GetTarget();
        if(m_Target != null)
        {
            Attack();
        }
    }

    /// <summary>
    /// Sets the enemy that enters range first as target until it dies or leaves.
    /// After that the first enemy in the list (enemy that entered after the first) will become the new target
    /// </summary>
    /// <returns></returns>
    public Enemy GetTarget()
    {
        GetEnemiesInRange();
        if (m_EnemiesInRange.Count > 0)
        {
            m_Target = m_EnemiesInRange[0];
            return m_Target;
        }
        else
        {
            m_Target = null;
            return null;
        }
    }

    /// <summary>
    /// Gets all the enemies that are within the towers range
    /// </summary>
    /// <returns></returns>
    public List<Enemy> GetEnemiesInRange()
    {
        for (int i = 0; i < EnemySpawner.s_Instance.SpawnedEnemies.Count; i++)
        {
            Enemy enemy = EnemySpawner.s_Instance.SpawnedEnemies[i];
            float distance = Vector3.Distance(transform.position, enemy.transform.position);            
            if(distance <= TowerData.AttackRange && !m_EnemiesInRange.Contains(enemy))
            {
                m_EnemiesInRange.Add(enemy);
            }
            else if(distance >= TowerData.AttackRange && m_EnemiesInRange.Contains(enemy))
            {
                RemoveEnemyFromList(enemy);
            }
        }
        
        return m_EnemiesInRange;
    }

    //
    void RemoveEnemyFromList(Enemy enemy)
    {
        m_EnemiesInRange.Remove(enemy);
    }

    public virtual void Attack()
    {
        // Do attack (done in child classes)
        //After tower has attacked, start cooldown
        if (!m_ReadyToAttack && !m_StartedCooldown)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        m_StartedCooldown = true;
        yield return new WaitForSeconds(TowerData.AttackInterval);
        m_ReadyToAttack = true;
    }
}