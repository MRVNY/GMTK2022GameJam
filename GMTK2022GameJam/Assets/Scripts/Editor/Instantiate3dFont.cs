using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(LevelSelectTrigger))]
public class Instantiate3dFont : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate 3d Generate3dNumbers"))
        {
            Debug.Log("It's alive: " + target.name);
            ((LevelSelectTrigger)target).Generate3dNumbers();
        }
    }
}
