// Copyright 2017 Matt Tytel

using UnityEditor;
using UnityEngine;

namespace AudioHelm
{
    [CustomEditor(typeof(HelmAudioInit))]
    class HelmAudioInitUI : Editor
    {
        SerializedObject serialized;
        SerializedProperty spatialize;
        SerializedProperty spatializeWithPlugin;
        SerializedProperty synthesizerGroup;
        SerializedProperty spatializerGroup;

        const float kMinBpm = 20.0f;
        const float kMaxBpm = 400.0f;

        void OnEnable()
        {
            serialized = new SerializedObject(target);
            spatialize = serialized.FindProperty("spatialize");
            spatializeWithPlugin = serialized.FindProperty("spatializeWithPlugin");
            synthesizerGroup = serialized.FindProperty("synthesizerMixerGroup");
            spatializerGroup = serialized.FindProperty("spatializerMixerGroup");
        }

        public override void OnInspectorGUI()
        {
            serialized.Update();
            HelmAudioInit audioInit = target as HelmAudioInit;
            EditorGUILayout.PropertyField(spatialize);

            if (audioInit.spatialize)
            {
                EditorGUILayout.PropertyField(spatializeWithPlugin);
                EditorGUILayout.PropertyField(synthesizerGroup);
                EditorGUILayout.PropertyField(spatializerGroup);
            }
            serialized.ApplyModifiedProperties();
        }
    }
}
