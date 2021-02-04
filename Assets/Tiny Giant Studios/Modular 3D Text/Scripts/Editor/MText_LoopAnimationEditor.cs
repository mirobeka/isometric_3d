﻿/// Created by Ferdowsur Asif @ Tiny Giant Studios

using UnityEditor;
using UnityEngine;

namespace MText
{
    [CustomEditor(typeof(MText_LoopAnimation))]
    public class MText_LoopAnimationEditor : Editor
    {
        MText_LoopAnimation myTarget;
        SerializedObject soTarget;

        void OnEnable()
        {
            myTarget = (MText_LoopAnimation)target;
            soTarget = new SerializedObject(target);
        }
        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();
            WarningCheck();

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
            }
        }

        void WarningCheck()
        {
            EditorGUI.indentLevel = 0;
            if (!myTarget.GetComponent<Modular3DText>()) { EditorGUILayout.HelpBox("Modular 3D Text is needed for this to work", MessageType.Error); }
            else
            {
                if (myTarget.GetComponent<Modular3DText>().combineMeshInEditor || myTarget.GetComponent<Modular3DText>().combineMeshDuringRuntime)
                {
                    EditorGUILayout.HelpBox("Turn off combine mesh for animation to work. The option is found in Modular 3D Text, Advanced Settings", MessageType.Warning);
                }
            }
            GUILayout.Space(5);
        }
    }
}