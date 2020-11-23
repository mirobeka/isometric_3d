using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyControlAsset))]
public class AICommandInspector : Editor
{
	private void OnEnable()
	{
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	public override void OnInspectorGUI()
	{
		EditorGUILayout.PropertyField(serializedObject.FindProperty("targetPosition")); //position
		serializedObject.ApplyModifiedProperties();
	}

	private void OnDisable()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}


	private void OnSceneGUI(SceneView v)
	{
        EditorGUI.BeginChangeCheck();
        Vector3 gizmoPos = Handles.PositionHandle(serializedObject.FindProperty("targetPosition").vector3Value, Quaternion.identity);
        
        if(EditorGUI.EndChangeCheck())
        {
            serializedObject.FindProperty("targetPosition").vector3Value = gizmoPos;
            serializedObject.ApplyModifiedProperties();
            
            Repaint();
        }
	}
}
