using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshCombiner))]
public class MeshCombinerEditor : Editor
{
    private void OnSceneGUI()
    {
        MeshCombiner mc = target as MeshCombiner;
        if(Handles.Button(mc.transform.position + Vector3.up * 4,Quaternion.LookRotation(Vector3.up),.5f,.5f,Handles.CylinderHandleCap))
        {
            mc.CombineMeshes();
        }
    }

    public override void OnInspectorGUI()
    {
        MeshCombiner mc = target as MeshCombiner;
        if (GUILayout.Button("Combine"))
        {
            mc.CombineMeshes();
        }
    }
}
