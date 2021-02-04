/// Created by Ferdowsur Asif @ Tiny Giant Studio
using UnityEngine;
using UnityEditor;

namespace MText
{
    [CustomEditor(typeof(MText_Font))]
    public class MText_FontEditor : Editor
    {
        MText_Font myTarget;
        SerializedObject soTarget;

        SerializedProperty rotationFix;
        SerializedProperty positionFix;
        SerializedProperty scaleFix;
        SerializedProperty fontSet;
        SerializedProperty overwriteOldSet;
        SerializedProperty characters;
        SerializedProperty monoSpaceFont;
        SerializedProperty useUpperCaseLettersIfLowerCaseIsMissing;
        SerializedProperty emptySpaceSpacing;
        SerializedProperty characterSpacing;
        SerializedProperty fallbackFonts;

        SerializedProperty enableKerning;
        SerializedProperty kerningMultiplier;

        void OnEnable()
        {
            myTarget = (MText_Font)target;
            soTarget = new SerializedObject(target);

            rotationFix = soTarget.FindProperty("rotationFix");
            positionFix = soTarget.FindProperty("positionFix");
            scaleFix = soTarget.FindProperty("scaleFix");

            fontSet = soTarget.FindProperty("fontSet");
            overwriteOldSet = soTarget.FindProperty("overwriteOldSet");
            monoSpaceFont = soTarget.FindProperty("monoSpaceFont");
            useUpperCaseLettersIfLowerCaseIsMissing = soTarget.FindProperty("useUpperCaseLettersIfLowerCaseIsMissing");
            emptySpaceSpacing = soTarget.FindProperty("emptySpaceSpacing");
            characterSpacing = soTarget.FindProperty("characterSpacing");

            characters = soTarget.FindProperty("characters");
            fallbackFonts = soTarget.FindProperty("fallbackFonts");

            enableKerning = soTarget.FindProperty("enableKerning");
            kerningMultiplier = soTarget.FindProperty("kerningMultiplier");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            GetFontSet();
            GUILayout.Space(5);
            FallBackFont();
            GUILayout.Space(10);
            FixSettings();
            GUILayout.Space(20);
            CreateCharacterList();
            GUILayout.Space(20);
            KerningSettings();


            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
            }
        }

       

        void FixSettings()
        {
            EditorGUILayout.LabelField("Incase this specific font doesnt have proper transform");

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rotation Fix", GUILayout.MaxWidth(70));
            EditorGUILayout.PropertyField(rotationFix, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Position Fix", GUILayout.MaxWidth(70));
            EditorGUILayout.PropertyField(positionFix, GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scale Fix", GUILayout.MaxWidth(70));
            EditorGUILayout.PropertyField(scaleFix, GUIContent.none);
            GUILayout.EndHorizontal();
        }

        void GetFontSet()
        {
            EditorGUILayout.LabelField("Font - Set object");
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.PropertyField(fontSet);
            EditorGUILayout.PropertyField(overwriteOldSet);


            if (GUILayout.Button("Create/Recreate characters"))
            {
                myTarget.UpdateCharacterList();
                EditorUtility.SetDirty(target);
            }
            GUILayout.Space(5);

            EditorGUILayout.PropertyField(monoSpaceFont);
            GUIContent useUpperCase = new GUIContent("Use UpperCase If LowerCase Is Missing", "Use UpperCase If LowerCase Is Missing");
            EditorGUILayout.PropertyField(useUpperCaseLettersIfLowerCaseIsMissing, useUpperCase);
            EditorGUILayout.PropertyField(emptySpaceSpacing);
            EditorGUILayout.PropertyField(characterSpacing);
        }

        private void FallBackFont()
        {
            //EditorGUILayout.PropertyField(fallbackFonts);
            EditorGUILayout.LabelField(new GUIContent("Fallback font", "If this font has missing characters, it will try to get the character from font"));

            for (int i = 0; i < myTarget.fallbackFonts.Count; i++)
            {
                GUILayout.BeginHorizontal();

                if (fallbackFonts.arraySize > i)
                {
                    if (myTarget.fallbackFonts[i] == myTarget)
                    {
                        Debug.LogError("Unnecessary self reference found on fallback font :" + i, myTarget);
                        myTarget.fallbackFonts.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(fallbackFonts.GetArrayElementAtIndex(i), GUIContent.none);

                        if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                        {
                            myTarget.fallbackFonts.RemoveAt(i);
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("+"))
            {
                myTarget.fallbackFonts.Add(null);
                EditorUtility.SetDirty(target);
            }
        }

        void CreateCharacterList()
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Char -", GUILayout.MaxWidth(45));
            EditorGUILayout.LabelField("Spacing -", GUILayout.MaxWidth(65));
            EditorGUILayout.LabelField("Prefab -", GUILayout.MaxWidth(55));
            EditorGUILayout.LabelField("or Mesh Asset");

            GUILayout.EndHorizontal();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            for (int i = 0; i < myTarget.characters.Count; i++)
            {
                GUILayout.BeginHorizontal();

                //if (characters.arraySize > 0)
                if (characters.arraySize > i)
                {
                    EditorGUILayout.PropertyField(characters.GetArrayElementAtIndex(i), GUIContent.none);

                    if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                    {
                        myTarget.characters.RemoveAt(i);
                    }
                }

                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("+"))
            {
                MText_Character character = new MText_Character();
                myTarget.characters.Add(character);
                EditorUtility.SetDirty(target);
            }
        }

        private void KerningSettings()
        {
            if (myTarget.kernTable.Count > 0)
            {
                EditorGUILayout.LabelField(myTarget.kernTable.Count + " kern table");
                EditorGUILayout.PropertyField(enableKerning);
                EditorGUILayout.PropertyField(kerningMultiplier);

                if(GUILayout.Button("Clear kern table"))
                {
                    myTarget.kernTable.Clear();
                }
            }
        }
    }
}
