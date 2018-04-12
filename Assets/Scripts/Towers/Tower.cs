using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerTypes
{
    BASS_TOWER,
    DRUM_TOWER
}
[System.Serializable]
public struct TowerData
{
    public TowerData(TowerTypes Type, int Level, float UpgradeCost,float BuyCost,float Value, float AttackDamage, float AttackRange, float AttackInterval){
        this.Type = Type;
        this.Level = Level;
        this.UpgradeCost = UpgradeCost;
        this.BuyCost = BuyCost;
        this.Value = Value;
        this.AttackDamage = AttackDamage;
        this.AttackRange = AttackRange;
        this.AttackInterval = AttackInterval;
    }

    public TowerTypes Type;
    public int Level;
    public float UpgradeCost;
    public float BuyCost;
    public float Value;
    public float AttackDamage;
    public float AttackRange;
    public float AttackInterval;
}

public class Tower : MonoBehaviour
{
    private TowerData m_TowerData;
    public TowerData TowerData { get; set; }

    protected float m_BuyCost;
    public float BuyCost { get; set; }

    private void Awake()
    {
        // GETS THE LEVEL 1 BASS TOWER CONFIG
        m_TowerData = TowerConfig.s_Towers[TowerTypes.BASS_TOWER][0];
    }

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