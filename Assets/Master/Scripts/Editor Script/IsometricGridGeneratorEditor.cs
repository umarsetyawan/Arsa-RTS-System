using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Arsa.RTSSystem.MapGeneration;

#if UNITY_EDITOR
[CustomEditor(typeof(IsometricGridGenerator))]
public class IsometricGridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IsometricGridGenerator gridGenerator = (IsometricGridGenerator)target;

        DrawDefaultInspector();

        EditorGUILayout.Space(2f);

        if (GUILayout.Button("Generate"))
        {
            gridGenerator.GenerateGrid();
        }

    }
}
#endif

