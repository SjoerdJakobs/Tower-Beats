using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridPathCreator))]
public class GridPathCreatorEditor : Editor
{
    private void OnSceneGUI()
    {
        Debug.Log("Test");
        if (Event.current.type == EventType.MouseUp)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(worldRay, out hitInfo))
            {
                Debug.Log(hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.CompareTag("HexTile"))
                {
                    Tile.s_OnTileClicked(hitInfo.transform.gameObject.GetComponent<Tile>());
                }
            }

        }
        Event.current.Use();
    }
}
