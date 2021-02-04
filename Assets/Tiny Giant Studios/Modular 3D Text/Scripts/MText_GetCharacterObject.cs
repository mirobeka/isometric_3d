using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MText
{
    public class MText_GetCharacterObject
    {
        public static GameObject GetObject(char c, Modular3DText text)
        {
            GameObject obj = null;
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                if (text.pooling && text.pool)
                    obj = text.pool.GetPoolItem(text.font, c);
                else
                {
                    Mesh meshPrefab = text.font.RetrievePrefab(c);
                    if (meshPrefab)
                    {
                        obj = new GameObject { name = c.ToString() };
                        obj.AddComponent<MeshFilter>();
                        obj.GetComponent<MeshFilter>().sharedMesh = meshPrefab;

                    }
                }
            }
            else
            {
                Mesh meshPrefab = text.font.RetrievePrefab(c);
                if (meshPrefab)
                {
                    obj = new GameObject { name = c.ToString() };
                    obj.AddComponent<MeshFilter>();
                    obj.GetComponent<MeshFilter>().sharedMesh = meshPrefab;
                }
            }
#else
            if(text.pooling && text.pool) 
                obj = text.pool.GetPoolItem(text.font, c);                                      
            else
            {
                Mesh meshPrefab = text.font.RetrievePrefab(c);
                if (meshPrefab)
                {
                    obj = new GameObject();
                    obj.AddComponent<MeshFilter>();
                    obj.GetComponent<MeshFilter>().sharedMesh = meshPrefab;
                    obj.name = c.ToString();
                }
            } 
#endif
            if (obj == null)
            {
                obj = new GameObject { name = "space" };
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (text.combineMeshInEditor || text.hideLettersInHierarchyInEditMode)
                    obj.hideFlags = HideFlags.HideInHierarchy;
            }
            else
            {
                if (text.hideLettersInHierarchyInPlayMode)
                    obj.hideFlags = HideFlags.HideInHierarchy;
            }
#endif

            //if (text.activateChildObjects) 
                obj.SetActive(true);
            //else obj.SetActive(false);
            return obj;
        }
    }
}