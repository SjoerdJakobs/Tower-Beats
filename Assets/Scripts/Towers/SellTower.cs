using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTower : MonoBehaviour {

	void Sell(TowerData tower)
    {
        PlayerData.s_Instance.Coins += tower.Value * 0.75f ; //Returns 75% of a towers value if you sell it
    }
}