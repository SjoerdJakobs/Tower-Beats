using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridPathCreator : MonoBehaviour
{
    [SerializeField] private List<Tile> m_SelectedTiles = new List<Tile>();
    private InputField m_InputField;
    private Text m_SelectingStateText;
    private bool m_IsSelectingTiles;
    private GridPathCreatorNotification m_Notification;

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

    public void ToggleSelection()
    {
        m_IsSelectingTiles = !m_IsSelectingTiles;

        UpdateSelectionState();
    }

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

    private void ShowPath()
    {
        for (int i = 0; i < m_SelectedTiles.Count; i++)
            m_SelectedTiles[i].SetAsPath();
    }

    private void TileClicked(Tile tile)
    {
        if(m_IsSelectingTiles)
        {
            if (!AlreadySelected(tile))
            { 
                m_SelectedTiles.Add(tile);
                tile.SetHighlightState(true);
                m_Notification.ShowNotification(GridPathCreatorNotification.NotificationType.LOG, "<b>" + tile.name + "</b> has been added to the path.");
            }
            else
            {
                int positionInList = m_SelectedTiles.IndexOf(tile);
                List<Tile> tilesToRemove = m_SelectedTiles.GetRange(positionInList, (m_SelectedTiles.Count - positionInList));
                for (int i = 0; i < tilesToRemove.Count; i++)
                {
                    tilesToRemove[i].SetHighlightState(false);
                    m_SelectedTiles.Remove(tilesToRemove[i]);
                }
                m_Notification.ShowNotification(GridPathCreatorNotification.NotificationType.WARNING, "Succesfully removed Tile(s) in path: <b>" + PrintPathList(tilesToRemove) + "</b>");
            }
        }
    }

    private string PrintPathList(List<Tile> list)
    {
        string temp = "";
        for (int i = 0; i < list.Count; i++)
            temp += list[i].PositionInGrid + (i != list.Count - 1 ? ", " : "");

        return temp;
    }

    private bool AlreadySelected(Tile tile)
    {
        return m_SelectedTiles.Contains(tile);
    }

    private void ClearSelectedTiles()
    {
        m_SelectedTiles.Clear();
    }
}
