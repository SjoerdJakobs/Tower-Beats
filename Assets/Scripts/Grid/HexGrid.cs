using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public enum OffsetAxis
    {
        X_AXIS,
        Y_AXIS
    }

    [Header("Prefab")]
    [SerializeField] private Tile m_TilePrefab;
    [Space]
    [Header("Grid Settings")]
    [SerializeField] private int m_GridWidth;
    [SerializeField] private int m_GridHeight;
    [Space]
    [SerializeField] private float m_TileOffsetX;
    [SerializeField] private float m_TileOffsetY;
    [Space]
    [SerializeField] private OffsetAxis m_OffsetAxis;
    [SerializeField] private float m_OffRowOffset;

    [SerializeField] private Tile[,] m_Grid;


    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        float lastX = 0;
        float lastY = 0;

        // Fill the Grid.
        m_Grid = new Tile[m_GridWidth, m_GridHeight];

        for (int x = 0; x < m_GridWidth; x++)
        {
            GameObject row = new GameObject();
            row.transform.parent = transform;
            row.name = "HexRow | Row: " + x;

            for (int y = 0; y < m_GridHeight; y++)
            {
                // Create Tile Component.
                Tile tile = Instantiate(m_TilePrefab, row.transform, false) as Tile;

                // Set random color for testing purposes.
                tile.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

                // Set Position on map.
                switch(m_OffsetAxis)
                {
                    case OffsetAxis.X_AXIS:
                        tile.transform.localPosition = new Vector2((y % 2 == 0 ? lastX : lastX + m_OffRowOffset), lastY);
                        break;
                    case OffsetAxis.Y_AXIS:
                        tile.transform.localPosition = new Vector2(lastX, (x % 2 == 0 ? lastY : lastY + m_OffRowOffset));
                        break;
                }


                // Set the name of the object.
                tile.name = "HexTile | GridPos(x: " + x + " y: " + y + ")";

                // Set the Tile's values;
                tile.PositionInGrid = new Vector2Int(x, y);

                // Set the Tile's state.
                tile.CurrentState = TileState.OPEN;

                // Add Tile to the Grid;
                m_Grid[x, y] = tile;

                // Update Y position.
                lastY += m_TileOffsetY;
            }

            // Update X position.
            lastX += m_TileOffsetX;

            // Reset Y because X got updated, meaning that a new row will be made.
            lastY = 0;
        }

        // Hide Prefab.
        m_TilePrefab.gameObject.SetActive(false);
    }

    public Tile GetTile(int x, int y)
    {
        return m_Grid[x, y];
    }
}
