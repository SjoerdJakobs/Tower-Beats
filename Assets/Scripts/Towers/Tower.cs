using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerTypes
{
    BASS_TOWER,
    DRUM_TOWER
}

public struct TowerData
{
    public TowerData(string Type, int Level, int MaxLevel, float UpgradeCost,float BuyCost,float Value, float AttackDamage, float AttackRange, float AttackInterval){
        this.Type = Type;
        this.Level = Level;
        this.MaxLevel = MaxLevel;
        this.UpgradeCost = UpgradeCost;
        this.BuyCost = BuyCost;
        this.Value = Value;
        this.AttackDamage = AttackDamage;
        this.AttackRange = AttackRange;
        this.AttackInterval = AttackInterval;
    }

    public string Type;
    public int Level;
    public int MaxLevel;
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

    private void Awake()
    {
        // GETS THE LEVEL 1 BASS TOWER CONFIG
        //m_TowerData = TowerConfig.s_Towers[TowerTypeTags.BASS_TOWER][0];
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

    //TODO remove this function once UI has been implemented
    private void OnMouseDown()
    {
        TowerUpgrades.OnUpgradeTower(this);
    }
}