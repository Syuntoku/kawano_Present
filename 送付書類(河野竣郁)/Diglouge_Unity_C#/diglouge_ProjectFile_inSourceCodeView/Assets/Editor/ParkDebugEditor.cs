using Syuntoku.DEBUG;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParkDebugPresenter))]
public class ParkDebugEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("AddPark"))
        {
            if (EditorApplication.isPlaying)
            {
                ParkDebugPresenter yourScript = (ParkDebugPresenter)target;
                yourScript.AddPark();
            }
        }
    }
}
