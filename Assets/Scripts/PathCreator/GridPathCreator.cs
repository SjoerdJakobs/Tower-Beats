using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridPathCreator : MonoBehaviour
{
    #region Variables

    private List<Tile> m_SelectedTiles = new List<Tile>();
    private InputField m_InputField;
    private Text m_SelectingStateText;
    private bool m_IsSelectingTiles;
    private GridPathCreatorNotification m_Notification;

    #endregion

    #region Monobehaviour functions

    private void Awake()
    {
        m_InputField = transform.GetComponentInChildren<InputField>();
        m_Notification = transform.GetComponentInChildren<GridPathCreatorNotification>();
        m_SelectingStateText = GameObject.Find("StartStopSelectionText").GetComponent<Text>();

        HexGrid.s_OnHexGridDestroyed += ClearSelectedTiles;
    }

    private void OnDestroy()
    {
        HexGrid.s_OnHexGridDestroyed -= ClearSelectedTiles;
    }

    #endregion

    #region Selection

    /// <summary>
    /// Toggles the selection
    /// </summary>
    public void ToggleSelection()
    {
        m_IsSelectingTiles = !m_IsSelectingTiles;

        UpdateSelectionState();
    }

    /// <summary>
    /// Updates the selection state
    /// </summary>
    private void UpdateSelectionState()
    {
        if (m_IsSelectingTiles)
        {
            m_SelectingStateText.text = "Stop Selection";
            Tile.s_OnTileClicked += TileClicked;
        }
        else
        {
            m_SelectingStateText.text = "Start Selection";
            Tile.s_OnTileClicked -= TileClicked;
        }
    }

    /// <summary>
    /// Finishes the selection and saves the path
    /// </summary>
    public void FinishSelection()
    {
        m_IsSelectingTiles = false;
        UpdateSelectionState();
        ShowPath();

        if(string.IsNullOrEmpty(m_InputField.text))
        {
            m_Notification.ShowNotification(GridPathCreatorNotification.NotificationType.ERROR, "Please enter a path name.");
            return;
        }

        if(PathManager.s_Instance.PathNameExists(m_InputField.text))
        {
            m_Notification.ShowNotification(GridPathCreatorNotification.NotificationType.ERROR, "A path with the name: <b>" + m_InputField.text + "</b> already exists.");
            return;
        }

        List<Vector2Int> tilePositions = new List<Vector2Int>();
        for (int i = 0; i < m_SelectedTiles.Count; i++)
            tilePositions.Add(m_SelectedTiles[i].PositionInGrid);

        PathManager.s_Instance.SavePath(new GridPath(m_InputField.text, HexGrid.s_Instance.GridSize, tilePositions));
        m_Notification.ShowNotification(GridPathCreatorNotification.NotificationType.LOG, "Succesfully saved <b>" + m_InputField.text + "</b>.");
    }

    /// <summary>
    /// Clears the selected tiles list
    /// </summary>
    private void ClearSelectedTiles()
    {
        m_SelectedTiles.Clear();
    }

    #endregion

    #region Path

    /// <summary>
    /// Shows the path
    /// </summary>
    private void ShowPath()
    {
        for (int i = 0; i < m_SelectedTiles.Count; i++)
            m_SelectedTiles[i].SetTileVisualsState(TileVisualState.PATH);
    }

    /// <summary>
    /// Loads a path for the Path Creator
    /// </summary>
    public void LoadPath()
    {
        PathManager.s_Instance.LoadPath(m_InputField.text);
    }

    #endregion

    #region Reset

    /// <summary>
    /// Resets the Path Creator to its default values
    /// </summary>
    public void ResetPathCreator()
    {
        ClearSelectedTiles();
        m_IsSelectingTiles = false;
        m_InputField.text = "";
        UpdateSelectionState();

        HexGrid.s_Instance.DestroyGrid(false);
        HexGrid.s_Instance.CreateGrid();
    }

    #endregion

    #region Callbacks

    /// <summary>
    /// Gets called when a Tile is clicked on the map
    /// </summary>
    /// <param name="tile">The Tile that is clicked</param>
    private void TileClicked(Tile tile)
    {
        if (m_IsSelectingTiles)
        {
            if (!AlreadySelected(tile))
            {
                m_SelectedTiles.Add(tile);
                tile.SetTileVisualsState(TileVisualState.PATH);
                m_Notification.ShowNotification(GridPathCreatorNotification.NotificationType.LOG, "<b>" + tile.name + "</b> has been added to the path.");
            }
            else
            {
                int positionInList = m_SelectedTiles.IndexOf(tile);
                List<Tile> tilesToRemove = m_SelectedTiles.GetRange(positionInList, (m_SelectedTiles.Count - positionInList));
                for (int i = 0; i < tilesToRemove.Count; i++)
                {
                    tilesToRemove[i].SetTileVisualsState(TileVisualState.BASE);
                    m_SelectedTiles.Remove(tilesToRemove[i]);
                }
                m_Notification.ShowNotification(GridPathCreatorNotification.NotificationType.WARNING, "Succesfully removed Tile(s) in path: <b>" + PrintPathList(tilesToRemove) + "</b>");
            }
        }
    }

    #endregion

    #region Utils

    /// <summary>
    /// Returns a string of all the tiles' positions in the list
    /// </summary>
    /// <param name="list">The list of tiles that need to be converted</param>
    /// <returns>A string of all the tiles' positions in the list</returns>
    private string PrintPathList(List<Tile> list)
    {
        string temp = "";
        for (int i = 0; i < list.Count; i++)
            temp += list[i].PositionInGrid + (i != list.Count - 1 ? ", " : "");

        return temp;
    }

    /// <summary>
    /// Returns if a Tile is already selected or not
    /// </summary>
    /// <param name="tile">The tile to check</param>
    /// <returns>If a Tile is already selected or not</returns>
    private bool AlreadySelected(Tile tile)
    {
        return m_SelectedTiles.Contains(tile);
    }

    #endregion
}
