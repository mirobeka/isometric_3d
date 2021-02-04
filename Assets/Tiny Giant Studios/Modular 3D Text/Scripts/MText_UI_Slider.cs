using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace MText
{
    public class MText_UI_Slider : MonoBehaviour
    {
        public bool autoFocusOnGameStart = true;
        public bool interactable = true;

        public float minValue = 0;
        public float maxValue = 100;
        public float currentValue = 50;

        public MText_UI_SliderHandle handle = null;
        public Transform progressBar = null;
        public GameObject progressBarPrefab = null;
        public Transform background = null;
        public float backgroundSize = 10;

        public int directionChoice;

        public bool keyControl = true;
        [Tooltip("How much to change on key press")]
        public float keyStep = 10;
        public KeyCode scrollUp = KeyCode.RightArrow;
        public KeyCode scrollDown = KeyCode.LeftArrow;

        public bool useEvents = true;
        public UnityEvent onValueChanged = null;
        [Tooltip("Mouse/touch dragging the slider ended")]
        public UnityEvent sliderDragEnded = null;

        //visual
        public Renderer handleGraphic = null;
        public Material selectedHandleMat = null;
        public Material unSelectedHandleMat = null;
        public Material clickedHandleMat = null;
        public Material disabledHandleMat = null;

        public bool useValueRangeEvents = true;
        public bool usePercentage = true;
        public List<ValueRange> valueRangeEvents = new List<ValueRange>();
        [HideInInspector] [SerializeField] int lastValue = 0;

        [System.Serializable]
        public class ValueRange
        {
            public float min = 0;
            public float max = 25;
            public GameObject icon;
            public bool triggeredAlready;
            public UnityEvent oneTimeEvents;
            public UnityEvent repeatEvents;
        }


        #region remember inspector layout
#if UNITY_EDITOR
        [HideInInspector] public bool showKeyboardSettings = false;
        [HideInInspector] public bool showVisualSettings = false;
        [HideInInspector] public bool showEventsSettings = false;
        [HideInInspector] public bool showValueRangeSettings = false;
#endif
        #endregion remember inspector layout



        void Awake()
        {
            if (interactable)
                Focus(autoFocusOnGameStart);
            else
            {
                DisabledVisual();
                this.enabled = false;
            }
        }

        void Update()
        {
            if (keyControl)
            {
                if (Input.GetKey(scrollUp))
                {
                    IncreaseValue();
                }

                if (Input.GetKey(scrollDown))
                {
                    DecreaseValue();
                }
            }
        }

        public void UpdateValue(int newValue)
        {
            currentValue = newValue;

            ValueChanged();

            if (handle)
                handle.GetHandlePositionFromSlider();
        }

        public void UpdateValue(float newValue)
        {
            currentValue = newValue;

            ValueChanged();

            if (handle)
                handle.GetHandlePositionFromSlider();
        }

        public void IncreaseValue()
        {
            currentValue += keyStep * Time.deltaTime;
            if (currentValue > maxValue)
                currentValue = maxValue;

            if (handle)
                handle.GetHandlePositionFromSlider();

            ValueChanged();
        }
        public void IncreaseValue(int amount)
        {
            currentValue += amount * Time.deltaTime;
            if (currentValue > maxValue)
                currentValue = maxValue;

            if (handle)
                handle.GetHandlePositionFromSlider();

            ValueChanged();
        }
        public void DecreaseValue()
        {
            currentValue -= keyStep * Time.deltaTime;
            if (currentValue < minValue)
                currentValue = minValue;

            if (handle)
                handle.GetHandlePositionFromSlider();

            ValueChanged();
        }
        public void DecreaseValue(int amount)
        {
            currentValue -= amount * Time.deltaTime;
            if (currentValue < minValue)
                currentValue = minValue;

            if (handle)
                handle.GetHandlePositionFromSlider();

            ValueChanged();
        }

        public void Focus(bool enable)
        {
            this.enabled = enable;

            if (enable)
                SelectedVisual();
            else
                UnSelectedVisual();
        }

        public void SelectedVisual()
        {
            var applySelectedStyle = ApplySelectedStyleFromParent();

            if (applySelectedStyle.Item1)
                ApplyVisual(applySelectedStyle.Item2.selectedItemBackgroundMaterial);
            else
                ApplyVisual(selectedHandleMat);
        }
        public void UnSelectedVisual()
        {
            var applyUnselectedStyle = ApplyNormalStyleFromParent();

            if (applyUnselectedStyle.Item1)
                ApplyVisual(applyUnselectedStyle.Item2.normalItemBackgroundMaterial);
            else
                ApplyVisual(unSelectedHandleMat);
        }
        public void ClickedVisual()
        {
            var applyPressedStyle = ApplyPressedStyleFromParent();

            if (applyPressedStyle.Item1)
                ApplyVisual(applyPressedStyle.Item2.pressedItemBackgroundMaterial);
            else
                ApplyVisual(clickedHandleMat);
        }
        public void DisabledVisual()
        {
            var applyDisabledStyle = ApplyDisabledStyleFromParent();

            if (applyDisabledStyle.Item1)
                ApplyVisual(applyDisabledStyle.Item2.disabledItemBackgroundMaterial);
            else
                ApplyVisual(disabledHandleMat);
        }

        void ApplyVisual(Material handleMaterial)
        {
            if (handleGraphic)
                handleGraphic.material = handleMaterial;
        }

        public MText_UI_List GetParentList()
        {
            if (transform.parent?.GetComponent<MText_UI_List>())
            {
                return transform.parent.GetComponent<MText_UI_List>();
            }
            else return null;
        }
        public (bool, MText_UI_List) ApplyNormalStyleFromParent()
        {
            MText_UI_List list = GetParentList();
            if (list)
            {
                if (list.controlChildVisuals && list.customNormalItemVisual)
                {
                    return (true, list);
                }
            }
            //don't apply from list
            return (false, list);
        }
        public (bool, MText_UI_List) ApplySelectedStyleFromParent()
        {
            //get style from parent list
            MText_UI_List list = GetParentList();
            if (list)
            {
                if (list.controlChildVisuals && list.customSelectedItemVisual)
                {
                    return (true, list);
                }
            }
            //don't apply from list
            return (false, list);
        }
        public (bool, MText_UI_List) ApplyPressedStyleFromParent()
        {
            //get style from parent list
            MText_UI_List list = GetParentList();
            if (list)
            {
                if (list.controlChildVisuals && list.customPressedItemVisual)
                {
                    return (true, list);
                }
            }
            //don't apply from list
            return (false, list);
        }
        public (bool, MText_UI_List) ApplyDisabledStyleFromParent()
        {
            MText_UI_List list = GetParentList();

            if (list)
            {
                if (list.controlChildVisuals && list.customDisabledItemVisual)
                    return (true, list);
            }
            return (false, list);
        }


        public void ValueChanged()
        {
            if (useEvents)
                onValueChanged.Invoke();
            if (useValueRangeEvents)
                ValueRangeEvents();
        }
        void ValueRangeEvents()
        {
            //two lines can be rewritten as one
            float valueToCheckAgainst = currentValue;
            if (usePercentage) valueToCheckAgainst = CurrentPercentage();

            bool newRange = false;
            int newValue = 0;
            for (int i = 0; i < valueRangeEvents.Count; i++)
            {
                //correct range
                if (valueToCheckAgainst >= valueRangeEvents[i].min && valueToCheckAgainst <= valueRangeEvents[i].max)
                {
                    newValue = i;
                    if (lastValue != i) newRange = true;

                    break;
                }
            }
            if (newRange && valueRangeEvents.Count > lastValue)
            {
                if (valueRangeEvents[lastValue].icon) valueRangeEvents[lastValue].icon.SetActive(false);
                lastValue = newValue;
            }
            ProcessSelectedValueRange(newValue, newRange);
        }

        void ProcessSelectedValueRange(int i, bool firstTime)
        {
            if (valueRangeEvents.Count <= i)
                return;

            if (valueRangeEvents[i].icon) valueRangeEvents[i].icon.SetActive(true);

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (firstTime) valueRangeEvents[i].oneTimeEvents.Invoke();
                valueRangeEvents[i].repeatEvents.Invoke();
            }
#else
            if (firstTime) valueRangeEvents[i].oneTimeEvents.Invoke();
            valueRangeEvents[i].repeatEvents.Invoke();
#endif
        }

        public float CurrentPercentage()
        {
            return (currentValue / maxValue) * 100;
        }

        public void ValueChangeEnded()
        {
            if (useEvents)
                sliderDragEnded.Invoke();
        }



        public void Uninteractable()
        {
            interactable = false;
            DisabledVisual();
        }
        public void Interactable()
        {
            interactable = true;
            UnSelectedVisual();
        }

        public void NewValueRange()
        {
            ValueRange valueRange = new ValueRange();
            valueRangeEvents.Add(valueRange);
        }
    }
}