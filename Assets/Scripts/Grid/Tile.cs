using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Open = buildable, Not_Usable = unusable tile, Path = Enemy AI Path, Occupied = Tile that contains tower
/// </summary>
public enum TileState
{
    OPEN,
    NOT_USABLE,
    PATH,
    OCCUPIED
}

public class Tile : MonoBehaviour
{
    public TileState CurrentState { get; set; }

    public Vector2Int PositionInGrid { get; set; }
    public int X { get { return PositionInGrid.x; } set { PositionInGrid = new Vector2Int(value, PositionInGrid.y); } }
    public int Y { get { return PositionInGrid.y; } set { PositionInGrid = new Vector2Int(PositionInGrid.x, value); } }

    public delegate void TileClicked(Tile tile);
    public static TileClicked s_OnTileClicked;

    private Tower m_Tower; //The tower on this tile
    public Tower Tower { get; set; }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) 
        {
            if (s_OnTileClicked != null) s_OnTileClicked(this);
            switch (CurrentState)
        	{
            	case TileState.OPEN:
                    if(MenuManager.s_Instance != null)
                	    MenuManager.s_Instance.ShowMenu(MenuNames.TOWER_SHOP_MENU);
                	//Open tower shop menu
                	break;
            	case TileState.OCCUPIED:
                    //Open tower menu and shows the stats of the tower on this tile
                    if (MenuManager.s_Instance != null)
                        MenuManager.s_Instance.ShowMenu(MenuNames.TOWER_MENU);
                	break;
        	}
		}
    }

    public void SetHighlightState(bool state)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = (state ? Color.yellow : Color.white);
    }

    public void SetAsPath()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}