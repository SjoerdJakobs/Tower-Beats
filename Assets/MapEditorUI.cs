using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour
{
    [SerializeField] private Text m_CurrentSelectedTileInfo;
    [SerializeField] private GameObject m_TypeSelector;
    [SerializeField] private GameObject m_PropSelector;
    [SerializeField] private GameObject m_TileUtilities;
    [SerializeField] private GameObject m_Reset;
    [Space]
    [SerializeField] private Text m_CurrentLayerText;

    private void Awake()
    {
        Tile.s_OnTileClicked += TileClicked;
        MapEditor.s_OnTileDataAdded += ResetMapEditorUI;
    }

    private void OnDestroy()
    {
        Tile.s_OnTileClicked -= TileClicked;
        MapEditor.s_OnTileDataAdded -= ResetMapEditorUI;
    }

    private void TileClicked(Tile tile)
    {
        SetTileInfo(tile.name);
        TileData data = MapEditor.s_Instance.GetTileDataFromMap(tile.PositionInGrid);
        if (data != null)
        {
            if (data.State != TileState.NOT_USABLE)
                ShowReset();
            else
                ShowTileUtilities();
        }
        else
            ShowTypeSelector();
    }

    private void SetTileInfo(string tileInfo)
    {
        m_CurrentSelectedTileInfo.text = tileInfo;
    }

    public void ShowTypeSelector()
    {
        m_TypeSelector.SetActive(true);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
    }

    public void ShowPropSelector()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(true);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
    }

    public void ShowReset()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(true);
    }

    public void ShowTileUtilities()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(true);
        m_Reset.SetActive(false);

        m_CurrentLayerText.text = MapEditor.s_Instance.CurrentSelectedTile.GetPropLayer().ToString();
    }

    public void UpPropLayer()
    {
        int layer = MapEditor.s_Instance.CurrentSelectedTile.GetPropLayer();
        layer++;
        MapEditor.s_Instance.CurrentSelectedTile.SetPropLayer(layer);
        MapEditor.s_Instance.GetTileDataFromMap(MapEditor.s_Instance.CurrentSelectedTile.PositionInGrid).LayerIndex = layer;

        m_CurrentLayerText.text = layer.ToString();
    }

    public void DownPropLayer()
    {
        int layer = MapEditor.s_Instance.CurrentSelectedTile.GetPropLayer();
        layer--;
        MapEditor.s_Instance.CurrentSelectedTile.SetPropLayer(layer);
        MapEditor.s_Instance.GetTileDataFromMap(MapEditor.s_Instance.CurrentSelectedTile.PositionInGrid).LayerIndex = layer;

        m_CurrentLayerText.text = layer.ToString();
    }

    public void ResetMapEditorUI()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
        m_CurrentSelectedTileInfo.text = "Select a Tile";
    }
}
