using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

    public static PlayerData s_Instance;

    private float m_Coins;
    public float Coins
    {
        get { return m_Coins; }
        set { m_Coins = value; }
    }
}
