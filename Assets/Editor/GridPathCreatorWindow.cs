using UnityEngine;
using UnityEditor;

public class GridPathCreatorWindow : EditorWindow
{
    private HexGrid m_HexGrid;
    private GridPathCreator m_GridPathCreator;

    private Vector2Int m_GridSize;
    private Vector2 m_TileOffset;
    private OffsetAxis m_OffsetAxis;
    private float m_OffRowOffset;

    private string m_PathName;


    [MenuItem("Window/Grid Path Creator")]
    public static void ShowWindow()
    {
        GetWindow<GridPathCreatorWindow>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
        m_GridSize = EditorGUILayout.Vector2IntField("Grid Size:", m_GridSize);
        m_TileOffset = EditorGUILayout.Vector2Field("Tile Offset:", m_TileOffset);
        m_OffsetAxis = (OffsetAxis)EditorGUILayout.EnumPopup("Offset Axis:", m_OffsetAxis);
        m_OffRowOffset = EditorGUILayout.FloatField("Off-Row Offset", m_OffRowOffset);
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Grid"))
        {
            if (m_HexGrid == null)
                m_HexGrid = FindObjectOfType<HexGrid>();

            if (m_GridPathCreator == null)
                m_GridPathCreator = FindObjectOfType<GridPathCreator>();

            if (m_HexGrid.GridCreated)
                m_HexGrid.DestroyGrid(true);

            m_HexGrid.CreateGrid(m_GridSize.x, m_GridSize.y, m_TileOffset.x, m_TileOffset.y, m_OffsetAxis, m_OffRowOffset);
        }
        if(GUILayout.Button("Remove Grid"))
        {
            m_HexGrid.DestroyGrid(true);
        }

        EditorGUILayout.Space();

        GUILayout.Label("Path", EditorStyles.boldLabel);
        GUILayout.Label("Press on a Selected Tile to Deselect it.", EditorStyles.helpBox);
        if (GUILayout.Button("Start Selection"))
        {
            m_GridPathCreator.StartSelection();
        }
        EditorGUILayout.Space();
        m_PathName = EditorGUILayout.TextField("Path Name: ", m_PathName);
        EditorGUILayout.Space();

        if (GUILayout.Button("Save Path"))
        {
            m_GridPathCreator.FinishSelection();
        }
        if (GUILayout.Button("Load Path"))
        {
            Debug.Log("Load Path");
        }
    }
}
