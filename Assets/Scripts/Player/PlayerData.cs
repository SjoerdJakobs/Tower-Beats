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
}