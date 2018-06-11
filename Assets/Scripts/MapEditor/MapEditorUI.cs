using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour
{
    [SerializeField] private Text m_CurrentSelectedTileInfo;
    [SerializeField] private GameObject m_TypeSelector;
    [SerializeField] private GameObject m_PropSelector;
    [SerializeField] private GameObject m_TileUtilities;
    [SerializeField] private GameObject m_Reset;
    [SerializeField] private GameObject m_SaveAndLoad;
    [SerializeField] private GameObject m_Saved;
    [SerializeField] private GameObject m_CopiedToClipboard;
    [Space]
    [SerializeField] private Text m_CurrentLayerText;
    [SerializeField] private InputField m_MapNameInput;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tileInfo"></param>
    private void SetTileInfo(string tileInfo)
    {
        m_CurrentSelectedTileInfo.text = tileInfo;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowTypeSelector()
    {
        m_TypeSelector.SetActive(true);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
        m_SaveAndLoad.SetActive(false);
        m_Saved.SetActive(false);
        m_CopiedToClipboard.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowPropSelector()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(true);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
        m_SaveAndLoad.SetActive(false);
        m_Saved.SetActive(false);
        m_CopiedToClipboard.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowReset()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(true);
        m_SaveAndLoad.SetActive(false);
        m_Saved.SetActive(false);
        m_CopiedToClipboard.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowTileUtilities()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(true);
        m_Reset.SetActive(false);
        m_SaveAndLoad.SetActive(false);
        m_Saved.SetActive(false);
        m_CopiedToClipboard.SetActive(false);

        m_CurrentLayerText.text = MapEditor.s_Instance.CurrentSelectedTile.GetLayer(TileVisualState.PROP).ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowSaveAndLoad()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
        m_SaveAndLoad.SetActive(true);
        m_Saved.SetActive(false);
        m_CopiedToClipboard.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowSaved()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
        m_SaveAndLoad.SetActive(false);
        m_Saved.SetActive(true);
        m_CopiedToClipboard.SetActive(false);
    }

    /// <summary>
    /// Sets the layer of the prop higher (1 layer)
    /// </summary>
    public void UpPropLayer()
    {
        int layer = MapEditor.s_Instance.CurrentSelectedTile.GetLayer(TileVisualState.PROP);
        layer++;
        MapEditor.s_Instance.CurrentSelectedTile.SetLayer(TileVisualState.PROP, layer);
        MapEditor.s_Instance.GetTileDataFromMap(MapEditor.s_Instance.CurrentSelectedTile.PositionInGrid).LayerIndex = layer;

        m_CurrentLayerText.text = layer.ToString();
    }

    /// <summary>
    /// Sets the layer of the prop lower (1 layer)
    /// </summary>
    public void DownPropLayer()
    {
        int layer = MapEditor.s_Instance.CurrentSelectedTile.GetLayer(TileVisualState.PROP);
        layer--;
        MapEditor.s_Instance.CurrentSelectedTile.SetLayer(TileVisualState.PROP, layer);
        MapEditor.s_Instance.GetTileDataFromMap(MapEditor.s_Instance.CurrentSelectedTile.PositionInGrid).LayerIndex = layer;

        m_CurrentLayerText.text = layer.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetMapEditorUI()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
        m_SaveAndLoad.SetActive(false);
        m_CurrentSelectedTileInfo.text = "Select a Tile";
    }

    /// <summary>
    /// Save a map with the name given in the input text field
    /// </summary>
    public void SaveMap()
    {
        MapEditor.s_Instance.SetMapName(m_MapNameInput.text);
        MapEditor.s_Instance.SaveMap();
    }

    /// <summary>
    /// Loads a map with the input given in the corresponding input text field
    /// </summary>
    public void LoadMap()
    {
        MapLoader.s_Instance.LoadMap(m_MapNameInput.text, false);
    }


    public void ShowCopiedToClipboard()
    {
        m_TypeSelector.SetActive(false);
        m_PropSelector.SetActive(false);
        m_TileUtilities.SetActive(false);
        m_Reset.SetActive(false);
        m_SaveAndLoad.SetActive(false);
        m_Saved.SetActive(false);
        m_CopiedToClipboard.SetActive(true);
    }
}