using System.Collections.Generic;
using UnityEngine;

namespace MText
{
    public class MText_UI_Toggle : MonoBehaviour
    {
        public bool active = true;
        public List<GameObject> activeGraphic = new List<GameObject>();
        public List<GameObject> inactiveGraphic = new List<GameObject>();


        public void Set(bool set)
        {
            active = set;
            VisualUpdate();
        }

        public void Toggle()
        {
            active = !active;
            VisualUpdate();
        }

        void VisualUpdate()
        {
            if (active) ActiveVisualUpdate();
            else InactiveVisualUpdate();
        }


        public void ActiveVisualUpdate()
        {
            ToggleGraphic(inactiveGraphic, false);
            ToggleGraphic(activeGraphic, true);
        }
        public void InactiveVisualUpdate()
        {
            ToggleGraphic(inactiveGraphic, true);
            ToggleGraphic(activeGraphic, false);
        }

        void ToggleGraphic(List<GameObject> list, bool enable)
        {
            for (int i = 0; i < list.Count; i++)
            {
#if UNITY_EDITOR
                //If user forgot to attach graphic. Notify them
                if (!list[i])
                {
                    string listName = !enable ? "inActive" : "Active";
                    Debug.Log(gameObject + " has a missing graphic in it's " + listName + "graphic list. Item number :" + i + ".", gameObject);
                }
#endif

                list[i].SetActive(enable);

            }
        }

#if UNITY_EDITOR
        public void AddEventToButton()
        {
            UnityEditor.Events.UnityEventTools.AddPersistentListener(GetComponent<MText_UI_Button>().onClickEvents, delegate { Toggle(); });
        }
#endif
    }
}