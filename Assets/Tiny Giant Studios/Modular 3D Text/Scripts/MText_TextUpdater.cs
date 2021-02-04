using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace MText
{
    [RequireComponent(typeof(Modular3DText))]
    [ExecuteAlways]
    public class MText_TextUpdater : MonoBehaviour
    {
        [ExecuteAlways]
        void Awake()
        {
            //Debug.Log("Test");
            Modular3DText text = gameObject.GetComponent<Modular3DText>();
            if (!text)
                return;

            bool empty = true;
            if (text.characterObjectList.Count>0)
            {
                for(int i = 0; i < text.characterObjectList.Count; i++)
                {
                    if (text.characterObjectList[i])
                        empty = false;
                }
                if (gameObject.GetComponent<MeshFilter>())
                {
                    if (gameObject.GetComponent<MeshFilter>().sharedMesh != null)
                        empty = false;
                }
            }

            if (empty)
                text.UpdateText();
        }

#if UNITY_EDITOR
        [ExecuteInEditMode]
        void OnEnable()
        {
            if (!Application.isPlaying)
            {
                Undo.undoRedoPerformed += UpdateText;
            }
        }

        void UpdateText()
        {
            if (!this)
                return;

            if (gameObject.GetComponent<Modular3DText>())
            {
                if (gameObject.GetComponent<Modular3DText>().autoCreateInEditor)
                    gameObject.GetComponent<Modular3DText>().UpdateText();
            }
            else
            {
                //There shouldn't be almost any case where this called as long as RequiredComponentExist. "Almost"
                Debug.LogWarning("MText_TextUpdater component attached to gameobject without Modular3DText component.\nAdd the Modulur3DText component or remove this TextUpdater");
            }
        }
#endif
    }
}
