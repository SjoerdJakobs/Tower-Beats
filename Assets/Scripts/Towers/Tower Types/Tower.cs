using System.Collections.Generic;
using UnityEngine;

public enum TowerTypes
{
    BASS_TOWER,
    DRUM_TOWER
}

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

    public Tower Self { get; set; } //Reference to itself

    public virtual void Attack()
    {
        // Do attack
    }

    public Enemy GetClosestEnemy()
    {
        return new Enemy();
    }

    public List<Enemy> GetEnemiesInRange()
    {
        return new List<Enemy>();
    }
}