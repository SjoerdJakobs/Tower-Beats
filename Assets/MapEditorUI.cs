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

    private void Awake()
    {
        Tile.s_OnTileClicked += TileClicked;
    }

    private void OnDestroy()
    {
        Tile.s_OnTileClicked -= TileClicked;
    }

    private void TileClicked(Tile tile)
    {
        SetTileInfo(tile.name);
        ShowTypeSelector();
    }

    private void SetTileInfo(string tileInfo)
    {
        m_CurrentSelectedTileInfo.text = tileInfo;
    }

    private void ShowTypeSelector()
    {
        m_TypeSelector.SetActive(true);
        m_PropSelector.SetActive(false);
    }
}
