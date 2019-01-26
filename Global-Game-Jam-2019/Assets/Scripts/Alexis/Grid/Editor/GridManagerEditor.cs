using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    GridManager p_target;

    private void OnEnable()
    {
        p_target = (GridManager)target; 
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Grid"))
            p_target.GenerateGrid();
        if (GUILayout.Button("Clear Grid"))
            p_target.ClearGrid(); 

    }
}
