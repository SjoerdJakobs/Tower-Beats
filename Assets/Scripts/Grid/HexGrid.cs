using System;
using UnityEngine;

#region Enums

/// <summary>
/// Axis of the row offset
/// </summary>
public enum OffsetAxis
{
    X_AXIS,
    Y_AXIS
}

#endregion

public class HexGrid : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Instance of the HexGrid. (Example: "HexGrid.s_Instance.CreateGrid()")
    /// </summary>
    public static HexGrid s_Instance;

    public static Action s_OnHexGridCreated;
    public static Action s_OnHexGridDestroyed;

    [Header("Prefab")]
    [SerializeField] private Tile m_TilePrefab;
    [Space]
    [Header("Grid Settings")]
    [SerializeField] private int m_GridWidth;
    [SerializeField] private int m_GridHeight;
    public Vector2Int GridSize { get { return new Vector2Int(m_GridWidth, m_GridHeight); } }
    [Space]
    [SerializeField] private float m_TileOffsetX;
    [SerializeField] private float m_TileOffsetY;
    [Space]
    [SerializeField] private OffsetAxis m_OffsetAxis;
    [SerializeField] private float m_OffRowOffset;

    private Tile[,] m_Grid;

    public Tile SelectedTile { get; set; }
    private bool m_GridCreated;

    #endregion

    #region Monobehaviour functions

    private void Awake()
    {
        Init();
        CreateGrid();
    }

    private void OnDestroy()
    {
        Tile.s_OnTileClicked -= delegate (Tile tile) { SelectedTile = tile; };
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Gets called upon Initialization (Awake)
    /// </summary>
    private void Init()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Tile.s_OnTileClicked += delegate (Tile tile) { SelectedTile = tile; };
        CreateGrid();
    }

    #endregion

    #region Grid

    /// <summary>
    /// Creates a Grid with the values entered in the inspector
    /// </summary>
    public void CreateGrid()
    {
        CreateGrid(m_GridWidth, m_GridHeight, m_TileOffsetX, m_TileOffsetY, m_OffsetAxis, m_OffRowOffset);
    }

    /// <summary>
    /// Creates a Grid with the values entered in the inspector and overwrites the width and the height
    /// </summary>
    /// <param name="width">Width of the Grid (x-axis)</param>
    /// <param name="height">Height of the Grid (y-axis)</param>
    public void CreateGrid(int width, int height)
    {
        CreateGrid(width, height, m_TileOffsetX, m_TileOffsetY, m_OffsetAxis, m_OffRowOffset);
    }

    /// <summary>
    /// Creates a Grid
    /// </summary>
    /// <param name="width">Width of the Grid (x-axis)</param>
    /// <param name="height">Height of the Grid (y-axis)</param>
    /// <param name="tileOffsetX">Offset of the tile on the x-axis</param>
    /// <param name="tileOffsetY">Offset of the tile on the y-axis</param>
    /// <param name="offsetAxis">The axis of the row offset</param>
    /// <param name="offRowOffset">The offset of the off-row (uneven rows)</param>
    public void CreateGrid(int width, int height, float tileOffsetX, float tileOffsetY, OffsetAxis offsetAxis, float offRowOffset)
    {
        if (!m_GridCreated)
            m_GridCreated = true;
        else
            return;

        float lastX = 0;
        float lastY = 0;

        // Fill the Grid
        m_Grid = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            // Create Row GameObject
            GameObject row = new GameObject();

            // Parent the Row to the Grid
            row.transform.parent = transform;

            // Set the name of the Row
            row.name = "HexRow | Row: " + x;

            for (int y = 0; y < height; y++)
            {
                // Create Tile Component
                Tile tile = Instantiate(m_TilePrefab, row.transform, false) as Tile;

                // Set random color for testing purposes
                //tile.GetComponent<SpriteRenderer>().color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

                // Set Position on map
                tile.transform.localPosition = new Vector2((offsetAxis == OffsetAxis.X_AXIS ? // Is the offset axis the X axis?
                                                                (y % 2 == 0 ? // Is the current Y an even number?
                                                                    lastX : lastX + offRowOffset) : lastX), // True : False
                                                           (offsetAxis == OffsetAxis.Y_AXIS ? // Is the offset axis the Y axis?
                                                                (x % 2 == 0 ? // Is the current X an even number?
                                                                    lastY : lastY + offRowOffset) : lastY)); // True : False

                // Set the name of the object
                tile.name = "HexTile | GridPos(x: " + x + " y: " + y + ")";

                // Set the Tile's values
                tile.PositionInGrid = new Vector2Int(x, y);
                
                // Add Tile to the Grid
                m_Grid[x, y] = tile;

                // Update Y position
                lastY += tileOffsetY;
            }

            // Update X position
            lastX += tileOffsetX;

            // Reset Y because X got updated, meaning that a new row will be made
            lastY = 0;
        }

        // Hide Prefab
        m_TilePrefab.gameObject.SetActive(false);

        // Invoke delegate
        if (s_OnHexGridCreated != null) s_OnHexGridCreated();
    }

    /// <summary>
    /// Destroy the Grid
    /// </summary>
    /// <param name="immediate">Does it need to be destroyed immediately? (For editor only)</param>
    public void DestroyGrid(bool immediate)
    {
        // Loop through all the Rows
        for (int i = transform.childCount - 1; i > 0; i--)
        {
            // Get the Row object
            GameObject objToDestroy = transform.GetChild(i).gameObject;

            // Destroy it
            if (immediate)
                DestroyImmediate(objToDestroy);
            else
                Destroy(objToDestroy);
        }

        // Set values
        m_GridCreated = false;
        m_Grid = null;
        m_TilePrefab.gameObject.SetActive(true);

        // Invoke delegate
        if (s_OnHexGridDestroyed != null) s_OnHexGridDestroyed();
    }

    #endregion

    #region Tile Utils

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

    /// <summary>
    /// Clears all the path tiles
    /// </summary>
    public void ClearPathTiles()
    {
        for (int i = 0; i < m_Grid.GetLength(0); i++)
        {
            for (int j = 0; j < m_Grid.GetLength(1); j++)
            {
                m_Grid[i, j].SetTileVisualsState(TileVisualState.BASE);
            }
        }
    }

    #endregion
}
