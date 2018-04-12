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
            // Create Row GameObject
            GameObject row = new GameObject();

            // Parent the Row to the Grid
            row.transform.parent = transform;

            // Set the name of the Row
            row.name = "HexRow | Row: " + x;

            for (int y = 0; y < m_GridHeight; y++)
            {
                // Create Tile Component
                Tile tile = Instantiate(m_TilePrefab, row.transform, false) as Tile;

                // Set random color for testing purposes
                tile.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

                // Set Position on map
                tile.transform.localPosition = new Vector2((m_OffsetAxis == OffsetAxis.X_AXIS ? // Is the offset axis the X axis?
                                                                (y % 2 == 0 ? // Is the current Y an even number?
                                                                    lastX : lastX + m_OffRowOffset) : lastX), // True : False
                                                           (m_OffsetAxis == OffsetAxis.Y_AXIS ? // Is the offset axis the Y axis?
                                                                (x % 2 == 0 ? // Is the current X an even number?
                                                                    lastY : lastY + m_OffRowOffset) : lastY)); // True : False

                // Set the name of the object
                tile.name = "HexTile | GridPos(x: " + x + " y: " + y + ")";

                // Set the Tile's values
                tile.PositionInGrid = new Vector2Int(x, y);

                // Set the Tile's state
                tile.CurrentState = TileState.OPEN;

                // Add Tile to the Grid
                m_Grid[x, y] = tile;

                // Update Y position
                lastY += m_TileOffsetY;
            }

            // Update X position
            lastX += m_TileOffsetX;

            // Reset Y because X got updated, meaning that a new row will be made
            lastY = 0;
        }

        // Hide Prefab
        m_TilePrefab.gameObject.SetActive(false);
    }

    /// <summary>
    /// Get a Tile by Grid Position
    /// </summary>
    /// <param name="x">Position X in the Grid</param>
    /// <param name="y">Position Y in the Grid</param>
    /// <returns>Tile by Grid Position</returns>
    public Tile GetTile(int x, int y)
    {
        return m_Grid[x, y];
    }

    /// <summary>
    /// Get a Tile by Grid Position
    /// </summary>
    /// <param name="position">Position in the Grid</param>
    /// <returns>Tile by Grid Position</returns>
    public Tile GetTile(Vector2Int position)
    {
        return m_Grid[position.x, position.y];
    }
}
