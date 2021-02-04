/// Created by Ferdowsur Asif @ Tiny Giant Studios

using System.Collections;
using UnityEngine.Events;
using UnityEngine;

namespace MText
{
    public class Mtext_UI_InputString : MonoBehaviour
    {

        [Tooltip("if not in a list")]
        public bool autoFocusOnGameStart = true;
        public bool interactable = true;

        [SerializeField] private int maxCharacter = 20;
        [SerializeField] private string typingSymbol = "|";

        public string input = string.Empty;
        public string placeHolderText = "Enter Text...";

        public Modular3DText text = null;
        public Renderer background = null;

        public bool enterKeyEndsInput = true;

        public Material placeHolderTextMat = null;

        public Material inFocusTextMat = null;
        public Material inFocusBackgroundMat = null;

        public Material outOfFocusTextMat = null;
        public Material outOfFocusBackgroundMat = null;

        public Material disabledTextMat = null;
        public Material disabledBackgroundMat = null;

        Material currentTextMaterial = null;

        [SerializeField] AudioClip typeSound = null;
        [SerializeField] AudioSource audioSource = null;

        public UnityEvent onInput = null;
        public UnityEvent onBackspace = null;
        public UnityEvent onInputEnd = null;


        #region remember inspector layout
#if UNITY_EDITOR
        [HideInInspector] public bool showMainSettings = true;
        [HideInInspector] public bool showStyleSettings = false;
        [HideInInspector] public bool showAudioSettings = false;
        [HideInInspector] public bool showUnityEventSettings = false;
#endif
        #endregion remember inspector layout



        void Awake()
        {
            if (!GetParentList())
                Focus(autoFocusOnGameStart);
        }


        void Update()
        {
            foreach (char c in Input.inputString)
            {
                if (c == '\b') // has backspace/delete been pressed?
                {
                    if (input.Length != 0)
                    {
                        input = input.Substring(0, input.Length - 1);
                        UpdateText(true);
                        onBackspace.Invoke();
                    }
                }
                else if (((c == '\n') || (c == '\r')) && enterKeyEndsInput) // enter/return
                {
                    InputComplete();
                }
                else
                {
                    if (input.Length < maxCharacter)
                    {
                        input += c;
                        UpdateText(true);
                        onInput.Invoke();
                    }
                }
            }
        }

        public void InputComplete()
        {
            onInputEnd.Invoke();
            this.enabled = false;
        }

        public void UpdateText()
        {
            UpdateText(false);
        }        
        public void UpdateText(string newText)
        {
            input = newText;
            UpdateText(false);
        }        
        public void UpdateText(int newTextInt)
        {
            input = newTextInt.ToString();
            UpdateText(false);
        }        
        public void UpdateText(float newTextFloat)
        {
            input = newTextFloat.ToString();
            UpdateText(false);
        }

        public void UpdateText(bool sound)
        {
            if (!text)
                return;

            TouchScreenKeyboard.Open(input);


            if (!string.IsNullOrEmpty(input))
            {
                text.material = currentTextMaterial;
                text.UpdateText(string.Concat(input, typingSymbol));
            }
            else
            {
                text.material = placeHolderTextMat;
                text.UpdateText(placeHolderText);
            }

            if (typeSound && sound && audioSource)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(typeSound);
            }
        }

        public void Select()
        {
            Focus(true);

            transform.parent?.GetComponent<MText_UI_List>()?.SelectItem(transform.GetSiblingIndex());
        }

        //coroutine to delay a single frame to avoid pressing "enter" key in one list to apply to another UI just getting enabled
        public void Focus(bool enable)
        {
            StartCoroutine(FocusRoutine(enable));
        }

        //coroutine to delay a single frame to avoid pressing "enter" key in one list to apply to another UI just getting enabled
        IEnumerator FocusRoutine(bool enable)
        {
            yield return null;
            FocusFunction(enable);
        }

        public void Focus(bool enable, bool delay)
        {
            if (!delay)
                FocusFunction(enable);
            else
                FocusRoutine(enable);
        }

        void FocusFunction(bool enable)
        {
            if (interactable)
            {
                this.enabled = enable;

                if (enable)
                    SelectedVisual();
                else
                    UnselectedVisual();
            }
            else
            {
                DisableVisual();
            }

            UpdateText(false);
        }


        public void Interactable()
        {
            Focus(false, false);
        }
        public void Uninteractable()
        {
            this.enabled = false;

            DisableVisual();
            UpdateText(false);
        }





        #region Visuals
        void SelectedVisual()
        {
            //item1 = applyStyleFromParent
            var applySelectedItemMaterial = ApplySelectedStyleFromParent();

            //apply parent list mat
            if (applySelectedItemMaterial.Item1)
                UpdateMaterials(applySelectedItemMaterial.Item2.selectedItemFontMaterial, applySelectedItemMaterial.Item2.selectedItemBackgroundMaterial);
            //apply self mat
            else
                UpdateMaterials(inFocusTextMat, inFocusBackgroundMat);
        }

        void UnselectedVisual()
        {
            //item1 = applyStyleFromParent
            var applyNormalStyle = ApplyNormalStyleFromParent();

            //apply parent list mat
            if (applyNormalStyle.Item1)            
                UpdateMaterials(applyNormalStyle.Item2.normalItemFontMaterial, applyNormalStyle.Item2.normalItemBackgroundMaterial);            
            //apply self mat
            else            
                UpdateMaterials(outOfFocusTextMat, outOfFocusBackgroundMat);                    
        }

        public void DisableVisual()
        {
            //item1 = applyStyleFromParent
            var applySelectedItemMaterial = ApplySelectedStyleFromParent();

            //apply parent list mat
            if (applySelectedItemMaterial.Item1)
                UpdateMaterials(applySelectedItemMaterial.Item2.disabledItemFontMaterial, applySelectedItemMaterial.Item2.disabledItemBackgroundMaterial);
            //apply self mat
            else
                UpdateMaterials(disabledTextMat, disabledBackgroundMat);
        }

        void UpdateMaterials(Material textMat, Material backgroundMat)
        {
            if (text)
                text.material = textMat;
            if (background)
                background.material = backgroundMat;

            currentTextMaterial = textMat;
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
        #endregion Visual
    }
}
