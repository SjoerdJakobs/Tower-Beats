using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public static PlayerData s_Instance;

    [SerializeField]private float m_Coins;
    public float Coins
    {
        get { return m_Coins; }
        set { m_Coins = value; }
    }

    [SerializeField] private int m_Lives;
    public int Lives
    {
        get { return m_Lives; }
        set { m_Lives = value; }
    }

    private Tile m_SelectedTile;
    public Tile SelectedTile { get; set; }

    public Tower SelectedTower { get; set; }

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Adds or removes coins (-50 for example removes 50 coins) and updates the UI to show these changes
    /// </summary>
    /// <param name="coins">Amount of coins to add</param>
    public void ChangeCoinAmount(float coins)
    {
        m_Coins += coins;
        if (PlayerInfo.s_OnUpdateCoins != null)
        {
            PlayerInfo.s_OnUpdateCoins();
        }
        if(m_Coins <= 0)
        {
            m_Coins = 0; //If coins drop under 0, rounds the amount of coins up to 0
        }
    }

    /// <summary>
    /// Adds or removes lives and updates the UI to show these changes
    /// </summary>
    /// <param name="lives">Lives to add</param>
    public void ChangeLivesAmount(int lives)
    {
        m_Lives += lives;
        if (PlayerInfo.s_OnUpdateLives != null)
        {
            PlayerInfo.s_OnUpdateLives();
        }
    }
}