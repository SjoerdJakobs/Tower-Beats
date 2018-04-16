using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HexGrid hexGrid = (HexGrid)target;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Grid"))
        {
            hexGrid.CreateGrid();
        }
        if (GUILayout.Button("Remove Grid"))
        {
            hexGrid.DestroyGrid(true);
        }

    }
}
