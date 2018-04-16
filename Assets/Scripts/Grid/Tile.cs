using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private TileState m_CurrentState;
    public TileState CurrentState { get; set; }

    [SerializeField]private Vector2Int m_PositionInGrid;
    public Vector2Int PositionInGrid { get { return m_PositionInGrid; } set { m_PositionInGrid = value; } }
    public int X { get { return m_PositionInGrid.x; } set { m_PositionInGrid.x = value; } }
    public int Y { get { return m_PositionInGrid.y; } set { m_PositionInGrid.y = value; } }

    public delegate void TileClicked(Tile tile);
    public static TileClicked s_OnTileClicked;

    private Tower m_Tower; //The tower on this tile
    public Tower Tower { get; set; }

	void Start () {
        m_CurrentState = TileState.OPEN;
	}

    void OnMouseDown()
    {
        if (s_OnTileClicked != null) s_OnTileClicked(this);
        PlayerData.s_Instance.SelectedTile = this;
        switch (m_CurrentState)
        {
            case TileState.OPEN:
                MenuManager.s_Instance.ShowMenu(MenuNames.TOWER_SHOP_MENU);

                //Open tower shop menu
                break;
            case TileState.OCCUPIED:
                //Open tower menu and shows the stats of the tower on this tile
                MenuManager.s_Instance.ShowMenu(MenuNames.TOWER_MENU);
                TowerMenu.s_Instance.Tower = m_Tower;
                TowerMenu.s_Instance.ShowTowerMenu();
                break;
        }

        Debug.Log("Tile(Row: " + X + ", Position: " + Y + ") got pressed.");
    }

    public void SetHighlightState(bool state)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = (state ? Color.grey : new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)));
    }

    public void SetAsPath()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}