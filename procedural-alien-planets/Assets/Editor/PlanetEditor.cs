using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Planet planet = (Planet)target;
        DrawSettingEditor(planet.shapeSettings);
        DrawSettingEditor(planet.colorSettings);

        if (GUILayout.Button("Generate Planet")) {
            planet.Generate();
        }
    }

     void DrawSettingEditor(UnityEngine.Object settings)
    {
        Editor editor = CreateEditor(settings);
        editor.OnInspectorGUI();
    }
}
