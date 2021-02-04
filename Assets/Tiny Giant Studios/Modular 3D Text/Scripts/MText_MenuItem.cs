/// Created by Ferdowsur Asif @ Tiny Giant Studios


using UnityEngine;
using UnityEditor;

namespace MText
{
    public class MText_MenuItem : MonoBehaviour
    {
#if UNITY_EDITOR
        static void CheckForRayCaster()
        {
            MText_UI_RaycastSelector mText_UI_RaycastSelector = (MText_UI_RaycastSelector)GameObject.FindObjectOfType(typeof(MText_UI_RaycastSelector));
            if (!mText_UI_RaycastSelector)
            {
                GameObject go = new GameObject("M3D Raycast Selector");
                go.AddComponent<MText_UI_RaycastSelector>();
                go.GetComponent<MText_UI_RaycastSelector>().myCamera = Camera.main;
            }
        }



        [MenuItem("GameObject/3D Object/Modular 3D Text/Text", false, 10)]
        static void CreateText(MenuCommand menuCommand)
        {
            // Create a custom game object
            GameObject go = new GameObject("Text (M3D)");
            go.AddComponent<Modular3DText>();
            go.AddComponent<MText_TextUpdater>();
            go.GetComponent<Modular3DText>().UpdateText("Modular 3D Text.");
            go.GetComponent<Modular3DText>().LoadDefaultSettings();

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;

        }

        [MenuItem("GameObject/3D Object/Modular 3D Text/List", false, 10)]
        static void CreateList(MenuCommand menuCommand)
        {
            CheckForRayCaster();

            // Create a custom game object
            GameObject go = new GameObject("List (M3D)");
            go.AddComponent<MText_UI_List>();

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/3D Object/Modular 3D Text/Button", false, 10)]
        static void CreateButton(MenuCommand menuCommand)
        {
            CheckForRayCaster();

            GameObject text = new GameObject("Text");
            text.AddComponent<Modular3DText>();
            text.GetComponent<Modular3DText>().LoadDefaultSettings();
            text.GetComponent<Modular3DText>().UpdateText("Button");

            GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bg.name = "Background";
            bg.transform.localScale = new Vector3(15, 2, 1);
            if (bg.GetComponent<BoxCollider>())
                DestroyImmediate(bg.GetComponent<BoxCollider>());

            // Create a custom game object
            GameObject go = new GameObject("Button (M3D)");

            bg.transform.SetParent(go.transform);
            bg.transform.localPosition = new Vector3(0, 0, 0.55f);
            text.transform.SetParent(go.transform);

            go.AddComponent<MText_UI_Button>();
            go.GetComponent<MText_UI_Button>().background = bg.GetComponent<Renderer>();
            go.GetComponent<MText_UI_Button>().text = text.GetComponent<Modular3DText>();
            go.GetComponent<MText_UI_Button>().LoadDefaultSettings();
            bg.GetComponent<Renderer>().material = go.GetComponent<MText_UI_Button>().normalBackgroundMaterial;


            go.AddComponent<BoxCollider>();
            go.GetComponent<BoxCollider>().size = new Vector3(15, 2, 1);
            go.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.5f);

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/3D Object/Modular 3D Text/Input Field", false, 10)]
        static void CreateInputField(MenuCommand menuCommand)
        {
            CheckForRayCaster();

            GameObject text = new GameObject("Text");
            text.AddComponent<Modular3DText>();
            text.GetComponent<Modular3DText>().LoadDefaultSettings();
            text.GetComponent<Modular3DText>().UpdateText("Input Field");

            GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bg.name = "Background";
            bg.transform.localScale = new Vector3(15, 2, 1);
            if (bg.GetComponent<BoxCollider>())
                DestroyImmediate(bg.GetComponent<BoxCollider>());

            // Create a custom game object
            GameObject go = new GameObject("Input Field (M3D)");

            bg.transform.SetParent(go.transform);
            bg.transform.localPosition = new Vector3(0, 0, 0.55f);
            text.transform.SetParent(go.transform);


            go.AddComponent<Mtext_UI_InputString>();
            go.GetComponent<Mtext_UI_InputString>().background = bg.GetComponent<Renderer>();
            bg.GetComponent<Renderer>().material = go.GetComponent<Mtext_UI_InputString>().outOfFocusBackgroundMat;
            go.GetComponent<Mtext_UI_InputString>().text = text.GetComponent<Modular3DText>();
            go.GetComponent<Mtext_UI_InputString>().UpdateText();



            go.AddComponent<BoxCollider>();
            go.GetComponent<BoxCollider>().size = new Vector3(15, 2, 1);
            go.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.5f);

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/3D Object/Modular 3D Text/Slider", false, 10)]
        static void CreateSlider(MenuCommand menuCommand)
        {
            CheckForRayCaster();

            // Create a custom game object
            GameObject go = new GameObject("Slider (M3D)");
            go.AddComponent<MText_UI_Slider>();
            MText_UI_Slider slider = go.GetComponent<MText_UI_Slider>();


            GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            handle.name = "Handle";
            handle.AddComponent<MText_UI_SliderHandle>();
            handle.GetComponent<MText_UI_SliderHandle>().slider = slider;
            handle.transform.SetParent(go.transform);
            handle.transform.localPosition = Vector3.zero;
            slider.handle = handle.GetComponent<MText_UI_SliderHandle>();
            slider.handleGraphic = handle.GetComponent<Renderer>();
            handle.GetComponent<Renderer>().material = slider.unSelectedHandleMat;

            GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bg.name = "Background";
            bg.transform.SetParent(go.transform);
            bg.transform.localPosition = Vector3.zero;
            bg.transform.localScale = new Vector3(10, 0.25f, 0.25f);
            slider.background = bg.transform;

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/3D Object/Modular 3D Text/Toggle", false, 10)]
        static void CreateToggle(MenuCommand menuCommand)
        {
            CheckForRayCaster();

            GameObject text = new GameObject("Text");
            text.AddComponent<Modular3DText>();
            text.GetComponent<Modular3DText>().LoadDefaultSettings();
            text.GetComponent<Modular3DText>().fontSize = new Vector3(20, 20, 8);
            text.GetComponent<Modular3DText>().UpdateText("x");

            GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bg.name = "Background";
            bg.transform.localScale = new Vector3(3, 3, 1);
            if (bg.GetComponent<BoxCollider>())
                DestroyImmediate(bg.GetComponent<BoxCollider>());

            GameObject go = new GameObject("Toggle (M3D)");

            go.AddComponent<MText_UI_Toggle>();

            bg.transform.SetParent(go.transform);
            bg.transform.localPosition = new Vector3(0, 0, 0.55f);
            text.transform.SetParent(go.transform);

            go.AddComponent<MText_UI_Button>();
            go.GetComponent<MText_UI_Button>().background = bg.GetComponent<Renderer>();
            go.GetComponent<MText_UI_Button>().pressedBackgroundMaterial = go.GetComponent<MText_UI_Button>().selectedBackgroundMaterial;
            go.GetComponent<MText_UI_Button>().text = text.GetComponent<Modular3DText>();
            go.GetComponent<MText_UI_Button>().normalFontSize = new Vector3(20, 20, 8);
            go.GetComponent<MText_UI_Button>().selectedFontSize = new Vector3(20, 20, 8.2f);
            go.GetComponent<MText_UI_Button>().pressedFontSize = new Vector3(20, 20, 8.5f);
            go.GetComponent<MText_UI_Button>().LoadDefaultSettings();

            bg.GetComponent<Renderer>().material = go.GetComponent<MText_UI_Button>().normalBackgroundMaterial;

            go.AddComponent<BoxCollider>();
            go.GetComponent<BoxCollider>().size = new Vector3(3, 3, 1);

            go.GetComponent<MText_UI_Toggle>().activeGraphic.Add(text);
            go.GetComponent<MText_UI_Toggle>().AddEventToButton();

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/3D Object/Modular 3D Text/Progress bar", false, 10)]
        static void CreateProgressBar(MenuCommand menuCommand)
        {
            CheckForRayCaster();

            // Create a custom game object
            GameObject go = new GameObject("ProgressBar (M3D)");
            go.AddComponent<MText_UI_Slider>();
            MText_UI_Slider slider = go.GetComponent<MText_UI_Slider>();


            GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            handle.name = "Handle";
            handle.AddComponent<MText_UI_SliderHandle>();
            handle.GetComponent<MText_UI_SliderHandle>().slider = slider;
            handle.transform.SetParent(go.transform);
            handle.transform.localPosition = Vector3.zero;
            slider.handle = handle.GetComponent<MText_UI_SliderHandle>();
            slider.handleGraphic = handle.GetComponent<Renderer>();
            handle.GetComponent<Renderer>().material = slider.unSelectedHandleMat;

            GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bg.name = "Background";
            bg.transform.SetParent(go.transform);
            bg.transform.localPosition = Vector3.zero;
            bg.transform.localScale = new Vector3(10, 0.25f, 0.25f);
            slider.background = bg.transform;

            if (slider.progressBarPrefab)
            {
                GameObject progressBarGraphic = Instantiate(slider.progressBarPrefab);
                slider.progressBar = progressBarGraphic.transform;
                progressBarGraphic.transform.SetParent(go.transform);
                progressBarGraphic.transform.localPosition = new Vector3(-5,0,0);
                progressBarGraphic.transform.localScale = new Vector3(5,0.8f,0.8f);
            }
            else
            {
                Debug.Log("No progress bar prefab found. Please create one and assign it to Progressbar field");
            }

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
#endif
    }
}