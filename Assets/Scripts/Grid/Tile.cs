using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Open = buildable, Not_Usable = unusable tile, Occupied = Tile that contains tower
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

	void Start () {
        m_CurrentState = TileState.OPEN;
	}

    void OnMouseDown()
    {
        if (s_OnTileClicked != null) s_OnTileClicked(this);

        switch (m_CurrentState)
        {
            case TileState.OPEN:
                //Build tower if player has selected a tower and has enough money
                break;
            case TileState.OCCUPIED:
                //Open tower menu
                break;
        }

        print("Tile(Row: " + X + ", Position: " + Y + ") got pressed.");
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