using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    /// <summary>
    /// Open = buildable, Not_Usable = unusable tile, Occupied = Tile that contains tower
    /// </summary>
    public enum Status
    {
        OPEN,
        NOT_USABLE,
        OCCUPIED
    }

    public Status CurrentStatus;

    private Tower m_Tower; //The tower on this tile

	void Start () {
        CurrentStatus = Status.OPEN;
	}

    void OnClick()
    {
        switch (CurrentStatus)
        {
            case Status.OPEN:
                //Build tower if player has selected a tower and has enough money
                break;
            case Status.OCCUPIED:
                //Open tower menu
                break;
        }
    }
}