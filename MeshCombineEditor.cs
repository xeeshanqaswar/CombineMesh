using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CombineMesh))]
public class MeshCombineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CombineMesh _combineMesh = (CombineMesh) target;
        if ( GUILayout.Button("CombineMesh") )
        {
            _combineMesh.CombineMeshInitiate();
        }
    }
}
