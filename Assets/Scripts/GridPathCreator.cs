using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridPathCreator : MonoBehaviour
{
    [SerializeField] private List<Tile> m_SelectedTiles = new List<Tile>();
    private InputField m_InputField;
    private bool m_IsSelectingTiles;

    private void Awake()
    {
        m_InputField = transform.GetComponentInChildren<InputField>();
    }

    public void StartSelection()
    {
        Tile.s_OnTileClicked += TileClicked;
        m_IsSelectingTiles = true;
    }

    public void FinishSelection()
    {
        Tile.s_OnTileClicked -= TileClicked;
        m_IsSelectingTiles = false;
        ShowPath();
        //PathManager.s_Instance.SavePath(m_InputField.text, m_SelectedTiles, HexGrid.s_Instance.GridSize);
    }

    private void ShowPath()
    {
        for (int i = 0; i < m_SelectedTiles.Count; i++)
        {
            m_SelectedTiles[i].SetAsPath();
        }
    }

    private void TileClicked(Tile tile)
    {
        if(m_IsSelectingTiles)
        {
            if (!AlreadySelected(tile))
            { 
                m_SelectedTiles.Add(tile);
                tile.SetHighlightState(true);
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
            }
        }
    }

    private bool AlreadySelected(Tile tile)
    {
        return m_SelectedTiles.Contains(tile);
    }

    public void LoadPath()
    {
        if(!InputFieldContainsCharacters(3))
        {
            Debug.LogWarning("Please enter atleast 3 characters");
            return;
        }

        HexGrid.s_Instance.DestroyGrid(false);

        GridPath path = PathManager.s_Instance.LoadPath(m_InputField.text);

        for (int i = 0; i < path.Path.Count; i++)
        {
            Tile tile = HexGrid.s_Instance.GetTile(path.Path[i].x, path.Path[i].y);
            tile.SetAsPath();
        }
    }

    private bool InputFieldContainsCharacters(int minAmount)
    {
        return m_InputField.text.ToCharArray().Length > minAmount;
    }
}
