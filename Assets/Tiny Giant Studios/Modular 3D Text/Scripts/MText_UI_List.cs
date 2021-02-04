using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MText
{
    [RequireComponent(typeof(MText_UI_ListEditorHelper))]
    public class MText_UI_List : MonoBehaviour
    {
        public int alignmentChoice = 0; //0 = top, 1 = bottom, 2 = verticle middle, 3 = left, 4 = right, 5 = Horizontal Middle, 6 = circular

        [Tooltip("List is scrollable with keyboard when focused")]
        [SerializeField] private bool autoFocusOnStart = true;
        [Tooltip("Selects first item when focused")]
        [SerializeField] private bool autoFocusFirstItem = true;

        public float spacing = 3;
        [Tooltip("Set Rotation to always 0,0,0")]
        public bool lockRotation = true;
        //circular list
        public int circularListStyle = 0;
        public float spread = 360;
        public float radius = 5;

        public bool keyboardControl = false;
        [SerializeField] private KeyCode scrollUp = KeyCode.UpArrow;
        [SerializeField] private KeyCode scrollDown = KeyCode.DownArrow;
        [SerializeField] private KeyCode pressItemKey = KeyCode.Return;

        public AudioSource audioSource = null;
        public AudioClip keyScrollingSoundEffect = null;
        public AudioClip itemSelectionSoundEffect = null;

        public bool controlChildVisuals = false;

        public bool customNormalItemVisual = false;
        public Vector3 normalItemFontSize = new Vector3(10, 10, 1f);
        public Material normalItemFontMaterial = null;
        public Material normalItemBackgroundMaterial = null;

        public bool customSelectedItemVisual = false;
        public Vector3 selectedItemFontSize = new Vector3(10.5f, 10.5f, 5f);
        public Material selectedItemFontMaterial = null;
        public Material selectedItemBackgroundMaterial = null;
        [SerializeField] private Vector3 selectedItemPositionChange = new Vector3(0, 0, -0.5f);
        [SerializeField] private float selectedItemMoveTime = 0.25f;

        public bool customPressedItemVisual = false;
        public Vector3 pressedItemFontSize = new Vector3(10.5f, 10.5f, 5f);
        public Material pressedItemFontMaterial = null;
        public Material pressedItemBackgroundMaterial = null;
        [SerializeField] private Vector3 pressedItemPositionChange = new Vector3(0, 0, -0.5f);
        [SerializeField] private float pressedItemMoveTime = 0.1f;
        public bool pressedItemReturnToSelectedVisual = true;
        [SerializeField] private float pressedItemReturnToSelectedTime = 0.1f;

        public bool customDisabledItemVisual = false;
        public Vector3 disabledItemFontSize = new Vector3(9, 9, 1);
        public Material disabledItemFontMaterial = null;
        public Material disabledItemBackgroundMaterial = null;


        public bool applyModules = true;
        public bool ignoreChildModules = false;

        public bool ignoreChildUnSelectModuleContainers = false;
        public bool applyUnSelectModuleContainers = true;
        public List<MText_ModuleContainer> unSelectModuleContainers = new List<MText_ModuleContainer>();

        public bool ignoreChildOnSelectModuleContainers = false;
        public bool applyOnSelectModuleContainers = true;
        public List<MText_ModuleContainer> onSelectModuleContainers = new List<MText_ModuleContainer>();

        public bool ignoreChildOnPressModuleContainers = false;
        public bool applyOnPressModuleContainers = true;
        public List<MText_ModuleContainer> onPressModuleContainers = new List<MText_ModuleContainer>();

        public bool ignoreChildOnClickModuleContainers = false;
        public bool applyOnClickModuleContainers = true;
        public List<MText_ModuleContainer> onClickModuleContainers = new List<MText_ModuleContainer>();

        public int selectedItem = 0;

        //for circular list
        Quaternion expectedRotation = Quaternion.Euler(0, 0, 0);
        Quaternion originalRotation = Quaternion.Euler(0, 0, 0);
        public float speed = 1;

        Vector3 originalPosition = Vector3.zero;
        Vector3 selectedPosition = Vector3.zero;
        float startTime = 0;
        bool selected = false;
        bool pressed = false;
        float returnToSelectedTime = 0;
        int counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop;
        int previousSelection = 0;

        #region remember inspector layout
#if UNITY_EDITOR
        [HideInInspector] public bool showKeyboardSettings = false;
        [HideInInspector] public bool showChildVisualSettings = false;
        [HideInInspector] public bool showNormalItemSettings = false;
        [HideInInspector] public bool showSelectedItemSettings = false;
        [HideInInspector] public bool showPressedItemSettings = false;
        [HideInInspector] public bool showDisabledItemSettings = false;
        [HideInInspector] public bool showAnimationSettings = false;
        [HideInInspector] public bool showModuleSettings = false;
#endif
        #endregion remember inspector layout


        void Awake()
        {
            SetRequiredFields();

            if (!autoFocusOnStart)
            {
                this.enabled = false;
                return;
            }

            this.enabled = true;

            if (autoFocusFirstItem)
                SelectTheFirstSelectableItem();
            else
                UnselectEverything();
        }

        private void SetRequiredFields()
        {
            if (transform.childCount > 0)
                originalPosition = transform.GetChild(0).localPosition;


            if (alignmentChoice == 6)
            {
                expectedRotation = transform.rotation;
            }

            if (pressedItemReturnToSelectedTime == 0)
                pressedItemReturnToSelectedTime = 0.01f;
            if (pressedItemMoveTime == 0)
                pressedItemMoveTime = 0.01f;
        }

        void Start()
        {
            originalRotation = transform.rotation;
        }

        void Update()
        {
            if (keyboardControl)
            {
                PressItemCheckInUpdate();
                ScrollList();
            }

            Animation();
        }

        void Animation()
        {
            //circular list animation
            if (alignmentChoice == 6)
            {
                if (!lockRotation)
                    transform.rotation = Quaternion.Lerp(transform.rotation, expectedRotation, Time.time * speed / 100);

                if (transform.childCount > selectedItem)
                {
                    if (pressed)
                    {
                        if (pressedItemReturnToSelectedVisual)
                        {
                            if (Time.time > returnToSelectedTime)
                            {
                                pressed = false;
                                transform.GetChild(selectedItem).GetComponent<MText_UI_Button>()?.SelectedButtonVisualUpdate();

                            }
                        }
                    }
                }
            }
            else if (alignmentChoice != 7)
            {
                if (transform.childCount > selectedItem)
                {
                    if (pressed)
                    {
                        float fracComplete = (Time.time - startTime) / pressedItemMoveTime;
                        transform.GetChild(selectedItem).localPosition = Vector3.Slerp(transform.GetChild(selectedItem).localPosition, selectedPosition + pressedItemPositionChange, fracComplete);
                        if (pressedItemReturnToSelectedVisual)
                        {
                            if (Time.time > returnToSelectedTime)
                            {
                                pressed = false;
                                transform.GetChild(selectedItem).GetComponent<MText_UI_Button>()?.SelectedButtonVisualUpdate();
                            }
                        }
                    }
                    else if (selected)
                    {
                        float fracComplete = (Time.time - startTime) / selectedItemMoveTime;
                        transform.GetChild(selectedItem).localPosition = Vector3.Slerp(transform.GetChild(selectedItem).localPosition, selectedPosition, fracComplete);
                    }
                }
            }
        }

        void PressItemCheckInUpdate()
        {
            if (Input.GetKeyDown(pressItemKey) && selected)
            {
                PresstItem(selectedItem);
            }
        }

        void ScrollList()
        {
            if (Scrolled())
            {
                selected = true;

                SelectItem(selectedItem);
                AlertSelectedItem(selectedItem);

                if (selectedItem != previousSelection)
                    UnselectItem(previousSelection);
            }
        }

        //checks for input and processes the input
        bool Scrolled()
        {
            bool scrolled = false;

            if (Input.GetKeyDown(scrollUp))
            {
                scrolled = true;
                previousSelection = selectedItem;

                selectedItem--;
                if (selectedItem < 0)
                    selectedItem = transform.childCount - 1;

                while (!IsItemSelectable(selectedItem) && counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop < transform.childCount)
                {
                    counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop++;

                    selectedItem--;
                    if (selectedItem < 0)
                        selectedItem = transform.childCount - 1;
                }
                counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop = 0;
            }
            else if (Input.GetKeyDown(scrollDown))
            {
                scrolled = true;
                previousSelection = selectedItem;

                selectedItem++;
                if (selectedItem > transform.childCount - 1)
                    selectedItem = 0;

                while (!IsItemSelectable(selectedItem) && counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop < transform.childCount)
                {
                    counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop++;

                    selectedItem++;
                    if (selectedItem > transform.childCount - 1)
                        selectedItem = 0;
                }
                counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop = 0;
            }

            return scrolled;
        }


        [ContextMenu("Update List")]
        public void UpdateList()
        {
            //0 = top, 1 = bottom, 2 = verticle middle, 3 = left, 4 = right, 5 = Horizontal Middle, 6 = circular 7 = Free

            if (alignmentChoice != 7)
            {
                float x = 0;
                float xIncreament = 0;
                float y = 0;
                float yIncreament = 0;

                //Top
                if (alignmentChoice == 0)
                {
                    y = 0;
                    yIncreament = -spacing;
                }
                //Bottom
                else if (alignmentChoice == 1)
                {
                    y = transform.childCount * spacing - spacing / 2;
                    yIncreament = -spacing;
                }
                //Verticle Middle
                else if (alignmentChoice == 2)
                {
                    y = (transform.childCount * spacing - spacing) / 2;
                    yIncreament = -spacing;
                }
                //Left
                else if (alignmentChoice == 3)
                {
                    x = 0;
                    xIncreament = spacing;
                }
                //Right
                else if (alignmentChoice == 4)
                {
                    x = -transform.childCount * spacing + spacing / 2;
                    xIncreament = spacing;
                }
                //Horizontal Middle
                else if (alignmentChoice == 5)
                {
                    x = -(transform.childCount * spacing - spacing) / 2;
                    xIncreament = spacing;
                }

                if (alignmentChoice != 6)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).localPosition = new Vector3(x, y, 0);
                        if (lockRotation)
                            transform.GetChild(i).localRotation = Quaternion.Euler(0, 0, 0);

                        x += xIncreament;
                        y += yIncreament;
                    }
                }
                //circular list
                else
                {
                    CircularListStyle();
                }
            }
        }

        private void CircularListStyle()
        {
            float angle = 0;
            if (transform.childCount > 1)
            {
                angle = (-(spread / 2) + (spread / transform.childCount) / 2);
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

                transform.GetChild(i).localPosition = new Vector3(x, y, 0);

                //centered
                if (circularListStyle == 0)
                    transform.GetChild(i).localRotation = Quaternion.Euler(angle - 90, 90, 0);
                else if (circularListStyle == 1)
                    transform.GetChild(i).localRotation = Quaternion.Euler(angle + 90, 90, 0);
                else if (circularListStyle == 2)
                    transform.GetChild(i).localRotation = Quaternion.Euler(angle + 90, 90, 90);
                else if (circularListStyle == 3)
                    transform.GetChild(i).localRotation = Quaternion.Euler(angle - 90, 90, 90);
                else
                {
                    Vector3 toTargetVector = Vector3.zero - transform.GetChild(i).localPosition;
                    float zRotation = Mathf.Atan2(toTargetVector.y, toTargetVector.x) * Mathf.Rad2Deg;
                    transform.GetChild(i).localRotation = Quaternion.Euler(new Vector3(0, 0, zRotation));
                }

                angle += (spread / transform.childCount);
            }
        }

        public void Focus(bool enable)
        {
            pressed = false;
            selected = false;

            if (enable)
            {
                UnselectEverything();
                if (autoFocusFirstItem)
                    SelectTheFirstSelectableItem();

                StartCoroutine(FocusRoutine());
            }

            else this.enabled = enable;
        }
        public void Focus(bool enable, bool delay)
        {
            pressed = false;
            selected = false;

            if (enable)
            {
                UnselectEverything();

                if (autoFocusFirstItem)
                    SelectTheFirstSelectableItem();

                if (delay)
                    StartCoroutine(FocusRoutine());
                else
                    this.enabled = true;
            }

            else this.enabled = enable;
        }

        //coroutine to delay a single frame to avoid pressing "enter" key in one list to apply to another list just getting enabled
        IEnumerator FocusRoutine()
        {
            yield return null;
            this.enabled = true;
        }

        //Used by ScrollList() method only
        bool IsItemSelectable(int i)
        {
            bool selecable = true;

            if (transform.childCount > i)
            {
                //it's a button
                if (transform.GetChild(i).GetComponent<MText_UI_Button>())
                    selecable = transform.GetChild(i).GetComponent<MText_UI_Button>().interactable;

                //it's a input Field
                if (transform.GetChild(i).GetComponent<Mtext_UI_InputString>())
                    selecable = transform.GetChild(i).GetComponent<Mtext_UI_InputString>().interactable;

                //it's a slider
                if (transform.GetChild(i).gameObject.GetComponent<MText_UI_Slider>())
                    selecable = transform.GetChild(i).gameObject.GetComponent<MText_UI_Slider>().interactable;
            }
            else
            {
                selecable = false;
            }

            return selecable;
        }

        //Processes the select item for the list. Doesn't let the item's components know it was selected
        public void SelectItem(int number)
        {
            if (transform.childCount > number)
            {
                if (audioSource && keyScrollingSoundEffect)
                    audioSource.PlayOneShot(keyScrollingSoundEffect, Random.Range(0.5f, 0.75f));

                UpdateList();

                selectedItem = number;

                originalPosition = transform.GetChild(selectedItem).localPosition;
                selectedPosition = originalPosition + selectedItemPositionChange;
                startTime = Time.time;

                if (alignmentChoice == 6)
                {
                    expectedRotation = originalRotation * Quaternion.Euler(0, 0, (360f / transform.childCount) * number);
                }
            }
        }
        //Let the item's components know it was selected
        void AlertSelectedItem(int number)
        {
            if (transform.childCount > number)
            {
                transform.GetChild(number).GetComponent<MText_UI_Button>()?.SelectButton();
                transform.GetChild(number).GetComponent<Mtext_UI_InputString>()?.Focus(true);
                transform.GetChild(number).gameObject.GetComponent<MText_UI_Slider>()?.Focus(true);
            }
        }
        public void UnselectItem(int i)
        {
            if (transform.childCount > i)
            {
                if (transform.GetChild(i).GetComponent<MText_UI_Button>())
                {
                    if (transform.GetChild(i).GetComponent<MText_UI_Button>().interactable)
                        transform.GetChild(i).GetComponent<MText_UI_Button>().UnselectButton();
                    else
                        transform.GetChild(i).GetComponent<MText_UI_Button>().Uninteractable();
                }

                if (transform.GetChild(i).GetComponent<Mtext_UI_InputString>())
                {
                    transform.GetChild(i).GetComponent<Mtext_UI_InputString>().Focus(false);
                }

                if (transform.GetChild(i).gameObject.GetComponent<MText_UI_Slider>())
                {
                    if (transform.GetChild(i).gameObject.GetComponent<MText_UI_Slider>().interactable)
                        transform.GetChild(i).gameObject.GetComponent<MText_UI_Slider>()?.Focus(false);
                    else
                        transform.GetChild(i).gameObject.GetComponent<MText_UI_Slider>().Uninteractable();
                }
            }
        }

        public void UnselectEverything()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                UnselectItem(i);
            }
        }
        public void UnselectEverythingExceptSelected()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i != selectedItem)
                    UnselectItem(i);
            }
        }

        public void PresstItem(int i)
        {
            if (transform.childCount > i)
            {
                pressed = true;
                startTime = Time.time;

                if (pressedItemReturnToSelectedVisual)
                    returnToSelectedTime = Time.time + pressedItemReturnToSelectedTime + pressedItemMoveTime;

                if (audioSource && itemSelectionSoundEffect)
                    audioSource.PlayOneShot(itemSelectionSoundEffect);

                AlertPressedUIItem(i);
            }
        }
        void AlertPressedUIItem(int i)
        {
            transform.GetChild(selectedItem).GetComponent<MText_UI_Button>()?.PressButtonDontCallList();
        }


        private void SelectTheFirstSelectableItem()
        {
            selected = true;

            if (selectedItem > transform.childCount - 1)
                selectedItem = 0;

            while (!IsItemSelectable(selectedItem) && counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop < transform.childCount)
            {
                counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop++;

                selectedItem++;
                if (selectedItem > transform.childCount - 1)
                    selectedItem = 0;
            }
            counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop = 0;

            SelectItem(selectedItem);
            AlertSelectedItem(selectedItem);
        }

        public void EmptyEffect(List<MText_ModuleContainer> moduleList)
        {
            MText_ModuleContainer module = new MText_ModuleContainer();
            module.duration = 0.5f;
            moduleList.Add(module);
        }
    }
}