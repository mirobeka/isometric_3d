/// Created by Ferdowsur Asif @ Tiny Giant Studios


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Linq;

namespace MText
{
    public class Modular3DText : MonoBehaviour
    {
        public MText_Settings settings = null;

        [TextArea]
        public string text = "";
        string ProcessedText => GetProcessedText();
        string oldText = "";

        [SerializeField] List<string> lineList = new List<string>();
        [SerializeField] List<string> oldLineList = new List<string>();
        public List<GameObject> characterObjectList = new List<GameObject>();

        //Creation settings--------------------------------------------------------------------------------------
        public bool autoCreateInEditor = true;
        public bool autoCreateInPlayMode = true;
        [Tooltip("only prefabs need mesh to be saved")]
        public bool autoSaveMesh = false;

        //Main Settings------------------------------------------------------------------------------------------
        public MText_Font font = null;
        [SerializeField] MText_Font oldFont = null;

        public Material material = default;
        [SerializeField] Material oldMaterial = null;

        public Vector3 fontSize = new Vector3(8, 8, 5);
        [SerializeField] Vector3 oldFontSize = Vector3.zero;

        public int textDirection = 1; //Left to right or right to left
        public float characterSpacingInput = 1;
        [SerializeField] float oldCharacterSpacingInput = 0;
        private float characterSpacing => characterSpacingInput * 0.1f * fontSize.x;

        public float lineSpacingInput = 1;
        [SerializeField] float oldLinespacingInput = 0;
        private float lineSpacing => lineSpacingInput * 0.13f * fontSize.y;
        public bool capitalize = false;
        public bool lowercase = false;


        //Layout Settings----------------------------------------------------------------------------------------
        public int layoutStyle = 0;
        int oldLayoutStyle = 0;

        //Alignment Horizontal
        public bool alignCenter = true;
        [SerializeField] bool oldAlignCenter = false;
        public bool alignLeft = false;
        [SerializeField] bool oldAlignLeft = false;
        public bool alignRight = false;
        [SerializeField] bool oldAlignRight = false;

        //Alignment Vertical
        public bool alignMiddle = true;
        [SerializeField] bool oldAlignMiddle = true;
        public bool alignBottom = false;
        [SerializeField] bool oldAlignBottom = false;
        public bool alignTop = false;
        [SerializeField] bool oldAlignTop = false;

        //Alignment Circular
        public float circularAlignmentRadius = 5;
        float oldCircularAlignmentRadius = 5;
        public float circularAlignmentSpreadAmount = 360;
        float oldCircularAlignmentSpreadAmount = 360;
        public Vector3 circularAlignmentAngle = new Vector3(0, 0, 0);
        [SerializeField] Vector3 oldCircularAlignmentAngle = new Vector3(0, 0, 0);

        public float height = 2;
        float oldHeight = 0;
        public float length = 15;
        private float adjustedLength = 0; //adjustedForMistakes like 0 length and height
        [SerializeField] float oldLength = 0;
        public float depth = 1;

        //Spawn effects
        public List<MText_ModuleContainer> typingEffects = new List<MText_ModuleContainer>();
        public List<MText_ModuleContainer> deletingEffects = new List<MText_ModuleContainer>();
        public bool customDeleteAfterDuration = false;
        public float deleteAfter = 1;

        //advanced settings-----------------------------------------------------------------------------------------------
        [Tooltip("When text is updated, old characters are moved to their correct position if their position is moved by something like module.")]
        public bool repositionOldCharacters = true;
        public bool reApplyModulesToOldCharacters = false;
        //public bool activateChildObjects = true;

        [Tooltip("Pooling increases performence if you are changing lots of text when game is running.")]
        public bool pooling = false;
        public MText_Pool pool = null;

        [Tooltip("Uses unity's Mesh.Combine method.\n" +
            "Unity has a limit of verticies one mesh can have which causes the bugs on large texts")]
        public bool combineMeshInEditor = false;
        public bool dontCombineInEditorAnyway = false;
        [Tooltip("There is no reason to turn this on unless you really need this for something. \nOtherwise, wasted resource on combining")]
        public bool combineMeshDuringRuntime = false;
        [Tooltip("Don't let letters show up in hierarchy in play mode. They are still there but not visible.")]
        public bool hideLettersInHierarchyInPlayMode = true;
        [Tooltip("If combine mesh is turned off")]
        public bool hideLettersInHierarchyInEditMode = false;

        [Tooltip("Breaks prefab connection while saving prefab location, can replace prefab at that location with a click")]
        public bool canBreakOutermostPrefab = false;
        //bool reconnectingPrefab = false;

        public string assetPath = string.Empty;
        [SerializeField] List<string> meshPaths = new List<string>();

        #region remember inspector layout
#if UNITY_EDITOR
        [HideInInspector] public bool showCreationettingsInEditor = false;
        [HideInInspector] public bool showMainSettingsInEditor = true;
        [HideInInspector] public bool showModuleSettingsInEditor = false;
        [HideInInspector] public bool showAdvancedSettingsInEditor = false;
#endif
        #endregion remember inspector layout

        //data
        int charInOneLine;
        float x, y, z; //TODO: z 
        bool createChilds;

        ////onvalidate stuff
        //bool onValidateWasCalled = false;
        //bool onValidateEditorWasCalled = false;

        public void EmptyEffect(List<MText_ModuleContainer> moduleList)
        {
            MText_ModuleContainer module = new MText_ModuleContainer();
            module.duration = 0.5f;
            moduleList.Add(module);
        }
        public void NewEffect(List<MText_ModuleContainer> moduleList, MText_Module newModule)
        {
            MText_ModuleContainer module = new MText_ModuleContainer();
            module.duration = 0.5f;
            module.module = newModule;
            moduleList.Add(module);
        }
        public void ClearAllEffects()
        {
            typingEffects.Clear();
            deletingEffects.Clear();
        }


        void Start()
        {
            oldText = ProcessedText;
        }

        //public for some editor scripts calling it once.
        void Update()
        {
            if (autoCreateInPlayMode)
            {
                UpdateTextWithCheck();
            }
            else this.enabled = false;
        }

        public void UpdateTextWithCheck()
        {
            if (!font)
                return;

            FontChangeCheck();

            if (DoesTextNeedToBeUpdated())
                UpdateText();
        }

        bool DoesTextNeedToBeUpdated()
        {
            if (
                    oldText != ProcessedText
                || oldMaterial != material
                || oldFontSize != fontSize
                || oldLayoutStyle != layoutStyle
                || oldCharacterSpacingInput != characterSpacingInput
                || oldLinespacingInput != lineSpacingInput
                || oldAlignCenter != alignCenter
                || oldAlignLeft != alignLeft
                || oldAlignRight != alignRight
                || oldAlignMiddle != alignMiddle
                || oldAlignTop != alignTop
                || oldAlignBottom != alignBottom
                || oldHeight != height
                || oldLength != length
                || oldCircularAlignmentRadius != circularAlignmentRadius
                || oldCircularAlignmentSpreadAmount != circularAlignmentSpreadAmount
                || oldCircularAlignmentAngle != circularAlignmentAngle
                )
                return true;
            else
                return false;
        }

        string GetProcessedText()
        {
            if (capitalize)
                return text.ToUpper();
            if (lowercase)
                return text.ToLower();

            return text;
        }

        void FontChangeCheck()
        {
            if (font != oldFont)
            {
                oldFont = font;
                oldText = "";
                oldLineList.Clear();
            }
        }
        void UpdateOldValues()
        {
            oldMaterial = material;
            oldFontSize = fontSize;

            oldLayoutStyle = layoutStyle;

            oldCharacterSpacingInput = characterSpacingInput;
            oldLinespacingInput = lineSpacingInput;
            oldAlignCenter = alignCenter;
            oldAlignLeft = alignLeft;
            oldAlignRight = alignRight;
            oldAlignMiddle = alignMiddle;
            oldAlignTop = alignTop;
            oldAlignBottom = alignBottom;
            oldLength = length;
            oldHeight = height;

            oldCircularAlignmentRadius = circularAlignmentRadius;
            oldCircularAlignmentSpreadAmount = circularAlignmentSpreadAmount;
            oldCircularAlignmentAngle = circularAlignmentAngle;
        }

#if UNITY_EDITOR
        #region OnValidate
        //public void OnValidate()
        //{
        //    onValidateWasCalled = true;
        //    OnValidateConfirm();
        //}
        //public void OnValidateEditor()
        //{
        //    onValidateEditorWasCalled = true;
        //    OnValidateConfirm();
        //}
        //void OnValidateConfirm()
        //{
        //    if (onValidateEditorWasCalled && onValidateWasCalled)
        //        if (onValidateWasCalled)
        //        {
        //            onValidateWasCalled = false;
        //            onValidateEditorWasCalled = false;

        //            if (autoCreateInEditor)
        //        {
        //            if (!EditorApplication.isPlaying & !reconnectingPrefab)
        //            {
        //                FontChangeCheck();
        //                if (font)
        //                {
        //                    if (DoesTextNeedToBeUpdated())
        //                    {
        //                        UpdateText();
        //                    }
        //                }
        //                else
        //                {
        //                    Debug.Log("No font selected for :" + gameObject.name, gameObject);
        //                }
        //            }
        //            reconnectingPrefab = false;
        //        }
        //    }
        //}
        #endregion OnValidate
        public bool PrefabBreakable()
        {
            if (!EditorApplication.isPlaying)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
                {
                    if (!PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject))
                        return false;
                    if (PrefabUtility.IsPartOfVariantPrefab(gameObject))
                        return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }
        void RemovePrefabConnection()
        {
            assetPath = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromSource(gameObject));
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }
        public void ReconnectPrefabs()
        {
            //reconnectingPrefab = true;
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, assetPath, InteractionMode.AutomatedAction);
        }
#endif

        public void UpdateText(string str)
        {
            if (!font)
            {
                Debug.Log("No font assigned on " + gameObject.name, gameObject);
                return;
            }

            text = str;
            UpdateText();
        }
        public void UpdateText(float number)
        {
            if (!font)
            {
                Debug.Log("No font assigned on " + gameObject.name, gameObject);
                return;
            }

            text = number.ToString();
            UpdateText();
        }
        public void UpdateText(int number)
        {
            if (!font)
            {
                Debug.Log("No font assigned on " + gameObject.name, gameObject);
                return;
            }

            text = number.ToString();
            UpdateText();
        }

        public void UpdateText()
        {
            if (!font)
                return;

            FontChangeCheck(); //if font is different recreate all texts
            UpdateOldValues(); //for change check
            FixInvalidInputs(); //checks for mistakes like 0 length/height

            if (layoutStyle == 0)
            {
                charInOneLine = CharacterInOneLineUpdate();
                if (charInOneLine < 1) charInOneLine = 1;
            }
            x = 0;

            createChilds = ShouldItCreateChild();

            SplitStuff();

            int newCharStartsFrom = 0;

            if (createChilds)
            {
                //text had combined mesh before
                if (GetComponent<MeshRenderer>())
                {
                    newCharStartsFrom = 0;
                    DestroyMeshRenderAndMeshFilter();
                }
                else
                {
                    newCharStartsFrom = CompareNewTextWithOld();
                }
            }

            oldLineList = new List<string>(lineList);
            DeleteReplacedChars(newCharStartsFrom);
            if (layoutStyle == 0)
                GetPositionAtStart();

            CheckIfPoolExistsAndRequired();

            //linear
            if (layoutStyle == 0)
            {
                if (repositionOldCharacters)
                {
                    PositionOldChars(newCharStartsFrom);
                }
                CreateNewChars(newCharStartsFrom);
            }
            else
            {
                CircularListProcessOldChars(newCharStartsFrom);
                //lineList.Clear();
                //lineList.Add(processedText);
                CreateNewCharsForCircularList(newCharStartsFrom);
            }



            oldText = ProcessedText;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (layoutStyle == 1)
                    EditorApplication.delayCall += () => CircularPositioning();
                if (!createChilds)
                    EditorApplication.delayCall += () => CombineMeshes();
            }
            else
            {
                if (layoutStyle == 1)
                    CircularPositioning();
                if (!createChilds)
                    CombineMeshes();
            }
#else
            if (layoutStyle == 1) CircularPositioning();            
            if (!createChilds) CombineMeshes();
#endif
        }

        void DestroyMeshRenderAndMeshFilter()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.delayCall += () => DestroyImmediate(GetComponent<MeshRenderer>());
                EditorApplication.delayCall += () => DestroyImmediate(GetComponent<MeshFilter>());
            }
            else
            {
                Destroy(GetComponent<MeshRenderer>());
                Destroy(GetComponent<MeshFilter>());
            }
#else
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshFilter>());
#endif
        }


        bool ShouldItCreateChild()
        {
            bool createChilds = false;

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (!combineMeshInEditor)
                {
                    if (!PrefabUtility.IsPartOfPrefabInstance(gameObject) || (PrefabUtility.IsPartOfPrefabInstance(gameObject) && dontCombineInEditorAnyway))
                    {
                        createChilds = true;
                    }
                    else if (canBreakOutermostPrefab && PrefabBreakable())
                    {
                        RemovePrefabConnection();
                        createChilds = true;
                    }
                }
            }
            else if (!combineMeshDuringRuntime)
            {
                createChilds = true;
            }
#else
            if (!combineMeshDuringRuntime)
            {
                createChilds = true;
            }
#endif
            return createChilds;
        }

        void CheckIfPoolExistsAndRequired()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            if (pooling)
            {
                if (!pool)
                {
                    pool = MText_Pool.Instance;
                    if (!pool)
                        CreatePool();
                }
            }
        }
        void CreatePool()
        {
            GameObject newPool = new GameObject();
            newPool.name = "Modular 3D Text Pool";
            newPool.AddComponent<MText_Pool>();
            pool = newPool.GetComponent<MText_Pool>();
            MText_Pool.Instance = pool;
        }

        void SplitStuff()
        {
            string delimiterChars = "([ \r\n])";
            string[] wordArray = Regex.Split(ProcessedText, delimiterChars);

            //Adds . , etc to the last word instead of a separate word       
            List<string> wordList = RemoveSpacesFromWordArray(wordArray).ToList();

            //Organized words to lines
            GetLineList(wordList);
        }
        string[] RemoveSpacesFromWordArray(string[] wordArray)
        {
            List<string> wordList = new List<string>();
            foreach (string str in wordArray)
            {
                if (str != " ")
                    wordList.Add(str);
            }

            return wordList.ToArray();
        }
        void GetLineList(List<string> wordList)
        {
            lineList = new List<string>();

            if (layoutStyle == 0)
                GetLinearLineList(wordList);
            else
                GetCircularLineList(wordList);
        }

        private void GetLinearLineList(List<string> wordList)
        {
            float totalSpacinginCurrentLine = 0;
            string newText = "";

            for (int i = 0; i < wordList.Count; i++)
            {
                float currentWordSpacing = TotalSpacingRequiredFor(wordList[i]);

                //pressed enter 
                if (wordList[i].Contains("\n"))
                {
                    lineList.Add(newText);
                    if (wordList[i] == "\n") newText = "";
                    else
                    {
                        //This shouldn't be happening. Ever. I don't remember why I added this but keeping it until further test is done
                        newText = wordList[i];
                    }
                    totalSpacinginCurrentLine = 0;
                    currentWordSpacing = 0;
                }
                //if the word can be placed in current line
                else if (totalSpacinginCurrentLine + currentWordSpacing < adjustedLength)
                {
                    //not the first word
                    if (totalSpacinginCurrentLine != 0) newText = string.Concat(newText, " ", wordList[i]);
                    //first word in line
                    else newText = string.Concat(newText, wordList[i]);
                }

                //the word is too big to fit in one line
                else if (currentWordSpacing > adjustedLength)
                {
                    if (font.monoSpaceFont)
                    {
                        int alreadyAdded = 0;
                        int lineRequiredForTheWord = Mathf.CeilToInt(wordList[i].Length / charInOneLine);
                        for (int j = 0; j < lineRequiredForTheWord; j++)
                        {
                            string part = wordList[i].Substring(alreadyAdded, charInOneLine);
                            lineList.Add(part);
                            alreadyAdded += charInOneLine;
                        }

                        newText = wordList[i].Substring(alreadyAdded);
                        totalSpacinginCurrentLine = TotalSpacingRequiredFor(newText);
                    }
                    else
                    {
                        if (newText != string.Empty)
                            lineList.Add(newText);

                        char[] chars = wordList[i].ToCharArray();
                        newText = string.Empty;
                        totalSpacinginCurrentLine = 0;

                        for (int j = 0; j < chars.Length; j++)
                        {
                            float mySpacing;
                            if (j == 0) mySpacing = font.Spacing(chars[j]) * characterSpacing;
                            else mySpacing = font.Spacing(chars[j - 1], chars[j]) * characterSpacing;

                            if (totalSpacinginCurrentLine + mySpacing <= adjustedLength)
                            {
                                totalSpacinginCurrentLine += mySpacing;
                                newText += chars[j];
                            }
                            else
                            {
                                lineList.Add(newText);

                                newText = chars[j].ToString();
                                totalSpacinginCurrentLine = mySpacing;
                            }
                        }
                    }
                    currentWordSpacing = 0;
                }
                //new line
                else
                {
                    string s = newText;
                    if (s != "") lineList.Add(s);
                    newText = "";
                    totalSpacinginCurrentLine = 0;
                    newText = wordList[i];
                }

                totalSpacinginCurrentLine += currentWordSpacing;

                //last word
                if (i == wordList.Count - 1)
                {
                    lineList.Add(newText);
                }
            }
        }
        private void GetCircularLineList(List<string> wordList)
        {
            //this is the else
            float totalSpacinginCurrentLine = 0;
            string newText = "";

            float arcLength = 2 * Mathf.PI * circularAlignmentRadius * (circularAlignmentSpreadAmount / 360);


            for (int i = 0; i < wordList.Count; i++)
            {
                float currentWordSpacing = TotalSpacingRequiredFor(wordList[i]);

                //pressed enter 
                if (wordList[i].Contains("\n"))
                {
                    lineList.Add(newText);
                    if (wordList[i] == "\n") newText = "";
                    else
                    {
                        //This shouldn't be happening. Ever. I don't remember why I added this but keeping it until further test is done
                        newText = wordList[i];
                    }
                    totalSpacinginCurrentLine = 0;
                    currentWordSpacing = 0;
                }
                //if the word can be placed in current line
                else if (totalSpacinginCurrentLine + currentWordSpacing < arcLength)
                {
                    //not the first word
                    if (totalSpacinginCurrentLine != 0) newText = string.Concat(newText, " ", wordList[i]);
                    //first word in line
                    else newText = string.Concat(newText, wordList[i]);
                }

                //the word is too big to fit in one line
                else if (currentWordSpacing > arcLength)
                {
                    if (font.monoSpaceFont)
                    {
                        int alreadyAdded = 0;
                        float charInCircle = arcLength / characterSpacing;
                        int lineRequiredForTheWord = Mathf.CeilToInt(wordList[i].Length / charInCircle);
                        for (int j = 0; j < lineRequiredForTheWord; j++)
                        {
                            string part = wordList[i].Substring(alreadyAdded, charInOneLine);
                            lineList.Add(part);
                            alreadyAdded += charInOneLine;
                        }

                        newText = wordList[i].Substring(alreadyAdded);
                        totalSpacinginCurrentLine = TotalSpacingRequiredFor(newText);
                    }
                    else
                    {
                        if (newText != string.Empty)
                            lineList.Add(newText);

                        char[] chars = wordList[i].ToCharArray();
                        newText = string.Empty;
                        totalSpacinginCurrentLine = 0;

                        for (int j = 0; j < chars.Length; j++)
                        {
                            float mySpacing;
                            if (j == 0) mySpacing = font.Spacing(chars[j]) * characterSpacing;
                            else mySpacing = font.Spacing(chars[j - 1], chars[j]) * characterSpacing;

                            if (totalSpacinginCurrentLine + mySpacing <= arcLength)
                            {
                                totalSpacinginCurrentLine += mySpacing;
                                newText += chars[j];
                            }
                            else
                            {
                                lineList.Add(newText);

                                newText = chars[j].ToString();
                                totalSpacinginCurrentLine = mySpacing;
                            }
                        }
                    }
                    currentWordSpacing = 0;
                }
                //new line
                else
                {
                    string s = newText;
                    if (s != "") lineList.Add(s);
                    newText = "";
                    totalSpacinginCurrentLine = 0;
                    newText = wordList[i];
                }

                totalSpacinginCurrentLine += currentWordSpacing;

                //last word
                if (i == wordList.Count - 1)
                {
                    lineList.Add(newText);
                }
            }
        }


        void FixInvalidInputs()
        {
            if (length != 0) adjustedLength = length;
            else adjustedLength = 10;

            if (layoutStyle == 1)
            {
                adjustedLength = 50 * (circularAlignmentSpreadAmount / 360);
            }
        }

        int CompareNewTextWithOld()
        {
            int newCharStartsFrom = 0;

            for (int i = 0; i < lineList.Count; i++)
            {
                //new line
                if (oldLineList.Count <= i)
                    return (newCharStartsFrom);

                char[] newChars = lineList[i].ToCharArray();
                char[] oldChars = oldLineList[i].ToCharArray();

                //Empty line was added. 
                if (newChars.Length == 0 || oldChars.Length == 0)
                    return (newCharStartsFrom);

                for (int j = 0; j < newChars.Length; j++)
                {
                    //less character than before
                    if (j >= oldChars.Length)
                        return (newCharStartsFrom); //was newCharStartsFrom - 1//testing

                    //character got replaced
                    if (newChars[j] != oldChars[j])
                        return (newCharStartsFrom);

                    newCharStartsFrom++;
                }
            }
            return (newCharStartsFrom);
        }

        void DeleteReplacedChars(int startingFrom)
        {
            //TODO
            //delete them straight from characterObjectList instead of storing in toDelete
            List<GameObject> toDelete = new List<GameObject>();
            for (int i = startingFrom; i < characterObjectList.Count; i++)
            {
                if (i >= startingFrom)
                {
                    toDelete.Add(characterObjectList[i]);
                }
            }
            if (toDelete.Count > 0)
            {
                foreach (GameObject child in toDelete)
                {
                    DestroyObject(child);
                    characterObjectList.Remove(child);
                }
            }
        }
        void DeleteAllChildObjects()
        {
            if (characterObjectList.Count == 0)
                return;

            for (int i = 0; i < characterObjectList.Count; i++)
            {
                if (!characterObjectList[i])
                    return;

#if UNITY_EDITOR
                if (!EditorApplication.isPlaying)
                {
                    if (!PrefabUtility.IsPartOfAnyPrefab(characterObjectList[i]))
                    {
                        characterObjectList[i].transform.SetParent(null);
                        GameObject objToDestroy = characterObjectList[i];
                        EditorApplication.delayCall += () => DestroyImmediate(objToDestroy);
                    }
                    else characterObjectList[i].SetActive(false);
                }
                else
                {
                    Destroy(characterObjectList[i]);
                }
#else
                Destroy(characterObjectList[i]);
#endif
            }
            characterObjectList.Clear();
        }
        void DestroyObject(GameObject obj)
        {
            if (!obj)
                return;

            if (characterObjectList.Contains(obj))
                characterObjectList.Remove(obj);

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                if (gameObject.activeInHierarchy)
                    StartCoroutine(RunTimeDestroyObjectRoutine(obj));
                else
                    RunTimeDestroyObjectOnDisabledText(obj);
            }
            else
            {
                if (PrefabUtility.IsPartOfAnyPrefab(obj))
                {
                    obj.SetActive(false);
                }
                else
                {
                    EditorApplication.delayCall += () =>
               {
                   try { DestroyImmediate(obj); }
                   catch { }
               };
                }
            }
#else
            StartCoroutine(RunTimeDestroyObjectRoutine(obj));
#endif
        }
        IEnumerator RunTimeDestroyObjectRoutine(GameObject obj)
        {
            float maxDelay = 0;

            obj.transform.SetParent(null);
            if (obj.name != "space")
            {
                if (gameObject.activeInHierarchy)
                {
                    for (int i = 0; i < deletingEffects.Count; i++)
                    {
                        if (deletingEffects[i].module)
                        {
                            StartCoroutine(deletingEffects[i].module.ModuleRoutine(obj, deletingEffects[i].duration));
                            if (deletingEffects[i].duration > maxDelay) maxDelay = deletingEffects[i].duration;
                        }
                    }
                }
                if (!customDeleteAfterDuration)
                {
                    if (deletingEffects.Count > 0)
                    {
                        yield return new WaitForSeconds(maxDelay);
                    }
                }
                else
                    yield return new WaitForSeconds(deleteAfter);
            }

            if (pooling && pool)
                pool.returnPoolItem(obj);
            else Destroy(obj);
        }
        void RunTimeDestroyObjectOnDisabledText(GameObject obj)
        {
            if (pooling && pool)
                pool.returnPoolItem(obj);
            else Destroy(obj);
        }


        #region Positioning
        void PositionOldChars(int startingFrom)
        {
            int lastCharacterPosition = 0;

            for (int i = 0; i < lineList.Count; i++)
            {
                if (lineList.Count > i)
                {
                    x = StartingX(lineList[i]);
                }

                char[] chars = lineList[i].ToCharArray();
                for (int j = 0; j < chars.Length; j++)
                {
                    if (lastCharacterPosition >= startingFrom)
                    {
                        break;
                    }
                    else
                    {
                        float characterIndividualSpacing;
                        if (j == 0) characterIndividualSpacing = (font.Spacing(chars[0]));
                        else characterIndividualSpacing = (font.Spacing(chars[j - 1], chars[j]));

                        float halfSpace = characterSpacing * (characterIndividualSpacing / 2) * textDirection;

                        x += halfSpace;
                        //x += characterSpacing * (font.Spacing(chars[j]) / 2) * textDirection;

                        if (characterObjectList.Count > lastCharacterPosition)
                        {
                            PrepareCharacter(characterObjectList[lastCharacterPosition]);
#if UNITY_EDITOR
                            if (EditorApplication.isPlaying && reApplyModulesToOldCharacters)
                                ApplyEffects(characterObjectList[lastCharacterPosition]);
#else
                            if (reApplyModulesToOldCharacters)                                
                                    ApplyEffects(characterObjectList[lastCharacterPosition]);                                
#endif
                        }

                        //x += characterSpacing * (font.Spacing(chars[j]) / 2) * textDirection;
                        x += halfSpace;
                    }

                    lastCharacterPosition++;
                }

                y -= lineSpacing;
            }
        }
        void GetPositionAtStart()
        {
            x = StartingX(lineList[0]);

            y = StartingY();
            z = 0;
        }


        float StartingX(string myString)
        {
            if (alignCenter)
            {
                if (!font.monoSpaceFont)
                    return (-((TotalSpacingRequiredFor(myString) - (characterSpacing * font.emptySpaceSpacing)) / 2)) * textDirection;
                //return (-((TotalSpacingRequiredFor(myString) - (characterSpacing)) / 2)) * textDirection;
                else
                    return (-((myString.Length * characterSpacing) / 2)) * textDirection;
            }
            else if (alignLeft)
            {
                return (-adjustedLength / 2) * textDirection;
            }
            else
            {
                if (!font.monoSpaceFont)
                    return ((adjustedLength / 2) - (TotalSpacingRequiredFor(myString) - (characterSpacing * font.emptySpaceSpacing))) * textDirection;
                //return ((adjustedLength / 2) - (TotalSpacingRequiredFor(myString) - (characterSpacing))) * textDirection;
                else
                    return ((adjustedLength / 2) - (myString.Length * characterSpacing)) * textDirection;
            }
        }
        float TotalSpacingRequiredFor(string myString)
        {
            char[] chars = myString.ToCharArray();
            float totalSpace = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 0)
                    totalSpace += characterSpacing * font.Spacing(chars[0]);
                else
                    totalSpace += characterSpacing * font.Spacing(chars[i - 1], chars[i]);
            }
            totalSpace += characterSpacing * font.emptySpaceSpacing;
            return totalSpace;
        }
        float StartingY()
        {
            if (alignTop)
            {
                //return (height/2 - (fontSize.y) * 0.13f);
                return (height / 2 - (lineSpacing / 2));
            }
            else if (alignMiddle)
            {
                if (lineList.Count == 1)
                    return 0;
                else
                    return lineList.Count / 2f * lineSpacing - lineSpacing / 2;
            }
            //alignbottom
            else
            {
                return (-height / 2 + lineList.Count * lineSpacing - (lineSpacing / 2));
                //return (-height / 2 + lineList.Count * lineSpacing - fontSize.y * 0.13f);
            }
        }
        int CharacterInOneLineUpdate()
        {
            return Mathf.FloorToInt(adjustedLength / characterSpacing);
        }

        #endregion Circular
        void CircularPositioning()
        {
            float angle = 0;
            if (lineList.Count > 0)
            {
                if (lineList[0].Length == 1) angle = 0;
                else angle = (-(circularAlignmentSpreadAmount / 2) + (circularAlignmentSpreadAmount / lineList[0].Length) / 2) * textDirection;
            }
            int letterNumber = 0;
            int lineNumber = 0;
            float y = 0;

            for (int i = 0; i < characterObjectList.Count; i++)
            {

                if (letterNumber > lineList[lineNumber].Length - 1)
                {
                    y += lineSpacing;

                    letterNumber = 0;
                    lineNumber++;

                    angle = 0;
                    if (lineList.Count > lineNumber)
                    {
                        if (lineList[lineNumber].Length > 1)
                            angle = (-(circularAlignmentSpreadAmount / 2) + (circularAlignmentSpreadAmount / lineList[lineNumber].Length) / 2) * textDirection;
                    }
                }
                letterNumber++;


                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * circularAlignmentRadius;
                float z = Mathf.Cos(Mathf.Deg2Rad * angle) * circularAlignmentRadius;

                if (characterObjectList[i])
                {
                    characterObjectList[i].transform.localPosition = new Vector3(x, y, z);
                    characterObjectList[i].transform.localRotation = Quaternion.Euler(circularAlignmentAngle.x, angle - circularAlignmentAngle.y, circularAlignmentAngle.z);
                }

                if (lineList.Count > lineNumber) //this is only to avoid error when editor is lagging. Keeping it outside a #IFEDITOR until further testsing is done
                    angle += (circularAlignmentSpreadAmount / lineList[lineNumber].Length) * textDirection;
            }
        }
        void CircularListProcessOldChars(int startingFrom)
        {
            int myCounter = 0;
            for (int i = 0; i < lineList.Count; i++)
            {
                foreach (char c in lineList[i])
                {
                    if (myCounter < startingFrom)
                    {
                        if (characterObjectList.Count >= myCounter)
                            break;
                        if (characterObjectList[i])
                        {
                            ApplyStyle(characterObjectList[myCounter]);
#if UNITY_EDITOR
                            if (EditorApplication.isPlaying && reApplyModulesToOldCharacters)
                                ApplyEffects(characterObjectList[myCounter]);
#else
                            if (reApplyModulesToOldCharacters)                                
                                    ApplyEffects(characterObjectList[myCounter]);                                
#endif
                        }
                    }

                    else
                        break;

                    myCounter++;
                }

            }
        }
        void CreateNewCharsForCircularList(int startingFrom)
        {
            int myCounter = 0;
            for (int i = 0; i < lineList.Count; i++)
            {
                foreach (char c in lineList[i])
                {
                    if (myCounter >= startingFrom)
                    {
#if UNITY_EDITOR
                        if (!EditorApplication.isPlaying)
                        {
                            EditorApplication.delayCall += () => CreateCharForCircularList(c);
                        }
                        else
                        {
                            CreateCharForCircularList(c);
                        }
#else
                    CreateCharForCircularList(c);
#endif
                    }
                    myCounter++;
                }

            }
        }
        void CreateCharForCircularList(char c)
        {
            if (!this)
                return;

            //GameObject obj = GetObject(c);
            GameObject obj = MText_GetCharacterObject.GetObject(c, this);

            obj.transform.SetParent(transform);

            ApplyStyle(obj);
            AddCharacterToList(obj);
            ApplyEffects(obj);
        }

        //unused for now
        //float SpacingRequiredFor(string word)
        //{
        //    if (font)
        //    {
        //        float total = 0;
        //        for (int i = 0; i < word.Length; i++)
        //        {
        //            total += font.Spacing(word[i]);
        //        }
        //        return total;
        //    }
        //    else return 0;
        //}

        void CreateNewChars(int startingFrom)
        {
            int myCounter = 0;
            for (int i = 0; i < lineList.Count; i++)
            {
                if (myCounter + lineList[i].Length > startingFrom)
                {
                    x = StartingX(lineList[i]);
                    y = StartingY() - lineSpacing * i;

                    string newString = "";
                    if (myCounter > startingFrom)
                    {
                        newString = lineList[i];
                    }
                    else
                    {
                        if (font.monoSpaceFont)
                            x += characterSpacing * (startingFrom - myCounter) * textDirection;
                        else
                        {
                            char[] previousStringChars = lineList[i].Substring(0, startingFrom - myCounter).ToCharArray();
                            //foreach (char c in previousStringChars)
                            for (int j = 0; j < previousStringChars.Length; j++)
                            {
                                if (j == 0)
                                    x += characterSpacing * font.Spacing(previousStringChars[0]) * textDirection;
                                else
                                    x += characterSpacing * font.Spacing(previousStringChars[j - 1], previousStringChars[j]) * textDirection;
                            }
                        }
                        newString = lineList[i].Substring(startingFrom - myCounter);
                    }


                    //foreach (char c in newString)
                    for (int j = 0; j < newString.Length; j++)
                    {
                        float characterIndividualSpacing;
                        if (j == 0) characterIndividualSpacing = (font.Spacing(newString[0]));
                        else characterIndividualSpacing = (font.Spacing(newString[j - 1], newString[j]));

                        float halfSpace = characterSpacing * (characterIndividualSpacing / 2) * textDirection;

                        //x += characterSpacing * (font.Spacing(c) / 2) * textDirection;
                        x += halfSpace;
#if UNITY_EDITOR
                        if (!EditorApplication.isPlaying)
                        {
                            float X = x;
                            float Y = y;
                            float Z = z;
                            Transform tr = transform;
                            char c = newString[j];
                            EditorApplication.delayCall += () => CreateAndPrepareCharacter(c, X, Y, Z, tr);
                        }
                        else
                            CreateThisChar(newString[j]);
#else
                       //CreateThisChar(c);
                       CreateThisChar(newString[j]);
#endif
                        //x += characterSpacing * (font.Spacing(c) / 2) * textDirection;
                        x += halfSpace;
                    }
                }
                myCounter += lineList[i].Length;
            }
        }
        void CreateThisChar(char c)
        {
            GameObject obj = MText_GetCharacterObject.GetObject(c, this);

            AddCharacterToList(obj);
            PrepareCharacter(obj);
            ApplyEffects(obj);
        }

        //positioning and creating in playmode
        void PrepareCharacter(GameObject obj)
        {
            if (!this)
                return;

            if (obj)
            {
                obj.transform.SetParent(transform);
                obj.transform.localPosition = new Vector3(x + font.positionFix.x, y + font.positionFix.y, z + font.positionFix.z);
                obj.transform.localRotation = Quaternion.Euler(font.rotationFix.x, font.rotationFix.y, font.rotationFix.z);
                ApplyStyle(obj);
            }
        }
        #region Prepare Character
        //creating in editor
        void CreateAndPrepareCharacter(char c, float myX, float myY, float myZ, Transform tr)
        {
            if (!tr) return;

            GameObject obj = MText_GetCharacterObject.GetObject(c, this);
            if (obj)
            {
                AddCharacterToList(obj);

                obj.transform.SetParent(tr);
                obj.transform.localPosition = new Vector3(myX + font.positionFix.x, myY + font.positionFix.y, myZ + font.positionFix.z);
                obj.transform.localRotation = Quaternion.Euler(font.rotationFix.x, font.rotationFix.y, font.rotationFix.z);
                ApplyStyle(obj);
            }
        }

        void AddCharacterToList(GameObject obj) => characterObjectList.Add(obj);
        #endregion

        void ApplyEffects(GameObject obj)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (obj.name != "space")
            {
                for (int i = 0; i < typingEffects.Count; i++)
                {
                    if (typingEffects[i].module) StartCoroutine(typingEffects[i].module.ModuleRoutine(obj, typingEffects[i].duration));
                }
            }
        }
        void ApplyStyle(GameObject obj)
        {
            if (obj.GetComponent<MText_Letter>())
            {
                if (obj.GetComponent<MText_Letter>().model)
                {
                    obj.GetComponent<MText_Letter>().model.material = material;
                }
            }
            if (obj.GetComponent<MeshFilter>())
            {
                if (!obj.GetComponent<MeshRenderer>())
                    obj.AddComponent<MeshRenderer>();

                obj.GetComponent<MeshRenderer>().material = material;
            }



            obj.transform.localScale = new Vector3(fontSize.x, fontSize.y, fontSize.z / 2);

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
                obj.layer = gameObject.layer;
            else
            {
                try
                {
                    EditorApplication.delayCall += () => SetLayer(obj);
                }
                catch
                {

                }
            }
#else
            SetLayer(obj);
#endif
        }
        void SetLayer(GameObject obj)
        {
            if (obj)
                obj.layer = gameObject.layer;
        }

        [ContextMenu("Combine meshes")]
        void CombineMeshes()
        {
            if (!this) //TODO
                return;

            if (!gameObject)
                return;

            Vector3 oldPos = transform.position;
            Quaternion oldRot = transform.rotation;
            Vector3 oldScale = transform.localScale;

            transform.rotation = Quaternion.identity;
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;

            if (GetComponent<MeshFilter>())
            {
                //using shared mesh also clears any copy of the text. For example, made with duplicating
                //GetComponent<MeshFilter>().sharedMesh.Clear();
                GetComponent<MeshFilter>().mesh = null;
            }


            List<List<MeshFilter>> meshFiltersUpperList = new List<List<MeshFilter>>();
            int listNumber = 0;
            List<MeshFilter> firstList = new List<MeshFilter>();
            meshFiltersUpperList.Add(firstList);

            int verteciesCount = 0;
            for (int j = 0; j < characterObjectList.Count; j++)
            {
                if (characterObjectList[j])
                {
                    if (characterObjectList[j].GetComponent<MeshFilter>())
                    {
                        if (verteciesCount + characterObjectList[j].GetComponent<MeshFilter>().sharedMesh.vertices.Length < 65535)
                        {
                            verteciesCount += characterObjectList[j].GetComponent<MeshFilter>().sharedMesh.vertices.Length;
                            meshFiltersUpperList[listNumber].Add(characterObjectList[j].GetComponent<MeshFilter>());
                        }
                        else
                        {
                            verteciesCount = 0;
                            List<MeshFilter> newList = new List<MeshFilter>();
                            meshFiltersUpperList.Add(newList);
                            listNumber++;
                            verteciesCount += characterObjectList[j].GetComponent<MeshFilter>().sharedMesh.vertices.Length;
                            meshFiltersUpperList[listNumber].Add(characterObjectList[j].GetComponent<MeshFilter>());
                        }
                    }
                }
            }

            for (int k = 0; k < meshFiltersUpperList.Count; k++)
            {
                MeshFilter[] meshFilters = meshFiltersUpperList[k].ToArray();
                CombineInstance[] combine = new CombineInstance[meshFilters.Length];

                int i = 0;
                while (i < meshFilters.Length)
                {
                    combine[i].mesh = meshFilters[i].sharedMesh;
                    combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                    i++;
                }

                if (!GetComponent<MeshFilter>())
                    gameObject.AddComponent<MeshFilter>();

                List<CombineInstance> combinedList = new List<CombineInstance>();
                for (int j = 0; j < combine.Length; j++)
                {
                    if (combine[j].mesh != null)
                        combinedList.Add(combine[j]);
                }
                combine = combinedList.ToArray();

                Mesh finalMesh = new Mesh();
                finalMesh.CombineMeshes(combine);

                if (k == 0)
                {
                    GetComponent<MeshFilter>().mesh = finalMesh;
                    if (!GetComponent<MeshRenderer>())
                        gameObject.AddComponent<MeshRenderer>();
                    GetComponent<MeshRenderer>().material = material;
                }
                else
                {
                    GameObject combinedMeshHolder = new GameObject();
                    combinedMeshHolder.name = "Combined mesh 2";
                    combinedMeshHolder.transform.SetParent(transform);
                    combinedMeshHolder.transform.rotation = Quaternion.identity;
                    combinedMeshHolder.transform.localPosition = Vector3.zero;

                    combinedMeshHolder.AddComponent<MeshFilter>();
                    combinedMeshHolder.GetComponent<MeshFilter>().mesh = finalMesh;

                    combinedMeshHolder.AddComponent<MeshRenderer>();
                    combinedMeshHolder.GetComponent<MeshRenderer>().material = material;
#if UNITY_EDITOR
                    EditorApplication.delayCall += () => AddToList(combinedMeshHolder);
#endif
                }
            }

            transform.position = oldPos;
            transform.rotation = oldRot;
            transform.localScale = oldScale;

            DeleteAllChildObjects();

#if UNITY_EDITOR
            if (autoSaveMesh) SaveMeshAsAsset(false);
#endif
        }

        void AddToList(GameObject combinedMeshHolder)
        {
            characterObjectList.Add(combinedMeshHolder);
        }

        public void OptimizeCombinedMesh(Mesh givenMesh)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                MeshUtility.Optimize(givenMesh);
            }
#endif
        }

#if UNITY_EDITOR
        public void SaveMeshAsAsset(bool saveAsDifferent)
        {
            if (!EditorApplication.isPlaying)
            {
                bool canceledAction = false;

                //gets save path from explorer
                if (CanSavePath() || saveAsDifferent)
                {
                    canceledAction = GetPaths();
                }

                if (!canceledAction) SaveAsset();
            }
        }

        void SaveAsset()
        {
            if (GetComponent<MeshFilter>())
            {
                //not trying to overwrite with same mesh
                if (AssetDatabase.LoadAssetAtPath(meshPaths[0], typeof(Mesh)) == GetComponent<MeshFilter>().sharedMesh)
                {
                    //Debug.Log("<color=green>The current mesh is already the asset at selected location. No need to overwrite.</color>");
                }
                else
                {
                    AssetDatabase.CreateAsset(GetComponent<MeshFilter>().sharedMesh, meshPaths[0]);
                    AssetDatabase.SaveAssets();
                }
            }

            for (int i = 0; i < characterObjectList.Count; i++)
            {
                if (characterObjectList[i])
                {
                    if (!characterObjectList[i].GetComponent<MeshFilter>())
                        break;

                    //not trying to overwrite with same mesh
                    if (AssetDatabase.LoadAssetAtPath(meshPaths[i], typeof(Mesh)) == characterObjectList[i].GetComponent<MeshFilter>().sharedMesh)
                    {
                        //Debug.Log("<color=green>The current mesh is already the asset at selected location. No need to overwrite.</color>");
                    }
                    else
                    {
                        AssetDatabase.CreateAsset(characterObjectList[i].GetComponent<MeshFilter>().sharedMesh, meshPaths[i + 1]); //path i+1 because 0 is taken by main object
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }

        bool CanSavePath()
        {
            bool saveAs = true;
            if (meshPaths.Count > 0)
            {
                if (string.IsNullOrEmpty(meshPaths[0]))
                {
                    saveAs = false;
                }
            }
            return saveAs;
        }
        bool GetPaths()
        {
            meshPaths.Clear();
            for (int i = 0; i < characterObjectList.Count + 1; i++)
            {
                string meshNumber;
                if (i == 0) meshNumber = string.Empty;
                else meshNumber = "mesh " + i;

                string path = EditorUtility.SaveFilePanel("Save Separate Mesh" + i + " Asset", "Assets/", name + meshNumber, "asset");
               
                if (string.IsNullOrEmpty(path))                
                    return true;                
                else 
                    path = FileUtil.GetProjectRelativePath(path);

                meshPaths.Add(path);
            }
            return false;
        }
#endif

        void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(0, 0, 0, 1f);
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(length, height, depth));
        }

        public void LoadDefaultSettings()
        {
            if (settings)
            {
                font = settings.defaultFont;
                fontSize = settings.defaultTextSize;
                material = settings.defaultTextMaterial;
                autoCreateInEditor = settings.autoCreateInEditorMode;
                autoCreateInPlayMode = settings.autoCreateInPlayMode;
            }
        }
    }
}