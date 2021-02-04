using UnityEngine;

namespace MText
{
    public class MText_UI_RaycastSelector : MonoBehaviour
    {
        [Tooltip("If not assigned, it will automatically get Camera.main on Start")]
        public Camera myCamera;
        [SerializeField] LayerMask UILayer = ~0;
        [SerializeField] float maxDistance = 5000;
        [Space]
        [Tooltip("True = How normal UI works. It toggles if clicking a inputfield enables it " +
            "and clicking somewhere else disables it")]
        public bool onlyOneTargetFocusedAtOnce = true;

        Transform target = null;
        Transform clickedTarget = null;
        bool dragging = false;

        void Start()
        {
            if (!myCamera)
                myCamera = Camera.main;
        }

        void Update()
        {
            if (!myCamera)
                return;

            if (dragging)
            {
                Dragging(target);
                DetectDragEnd();
            }
            else
            {
                Transform hit = WhoDareStandInMyWay(); //laser pew pew
                if (hit != target)
                {
                    UnSelectOldTarget(target);
                }
                if (hit)
                {
                    if (hit != target)
                        SelectNewTarget(hit);

                    if (Input.GetMouseButtonDown(0))
                    {
                        PressTarget(hit);
                        dragging = true;
                    }
                }
                target = hit;
            }
        }

        void DetectDragEnd()
        {
            if (Input.touchCount > 0)
            {
                if(Input.touches[0].phase == TouchPhase.Ended)
                {
                    dragging = false;
                    DragEnded(target, WhoDareStandInMyWay());
                }
            }

            if (Input.GetMouseButtonUp(0) && dragging)
            {
                dragging = false;
                DragEnded(target, WhoDareStandInMyWay());
            }
        }

        Transform WhoDareStandInMyWay()
        {
            RaycastHit hit;
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, maxDistance, UILayer))
                return hit.transform;
            else
                return null;
        }

        void PressTarget(Transform hit)
        {
            if (onlyOneTargetFocusedAtOnce)
                UnFocusPreviouslySelectedItems(hit);

            PressInputString(hit);
            PressButton(hit);
            PressSlider(hit);
        }
        void PressInputString(Transform hit)
        {
            Mtext_UI_InputString inputString = hit.gameObject.GetComponent<Mtext_UI_InputString>();
            if (!InteractWithInputString(inputString))
                return;

            inputString.Select();
            clickedTarget = hit;
        }
        void PressSlider(Transform hit)
        {
            MText_UI_SliderHandle sliderHandle = hit.gameObject.GetComponent<MText_UI_SliderHandle>();
            if (!InteractWithSlider(sliderHandle))
                return;

            hit.gameObject.GetComponent<MText_UI_SliderHandle>().slider.ClickedVisual();
        }
        void PressButton(Transform hit)
        {
            MText_UI_Button button = hit.gameObject.GetComponent<MText_UI_Button>();
            if (!InteractWithButton(button))
                return;

            button.PressButtonClick();
        }

        void UnFocusPreviouslySelectedItems(Transform hit)
        {
            if (hit != clickedTarget)
            {
                if (clickedTarget)
                {
                    if (clickedTarget.gameObject.GetComponent<Mtext_UI_InputString>())
                    {
                        if (clickedTarget.gameObject.GetComponent<Mtext_UI_InputString>().interactable)
                        {
                            clickedTarget.gameObject.GetComponent<Mtext_UI_InputString>().Focus(false);
                        }
                    }
                }
            }
        }

        void SelectNewTarget(Transform hit)
        {
            SelectButton(hit);
            SelectSlider(hit);
        }
        void SelectSlider(Transform hit)
        {
            MText_UI_SliderHandle sliderHandle = hit.gameObject.GetComponent<MText_UI_SliderHandle>();
            if (!InteractWithSlider(sliderHandle))
                return;

            sliderHandle.slider.SelectedVisual();
        }
        void SelectButton(Transform hit)
        {
            MText_UI_Button button = hit.gameObject.GetComponent<MText_UI_Button>();
            if (!InteractWithButton(button))
                return;

            button.SelectButton();
        }

        void UnSelectOldTarget(Transform hit)
        {
            if (!hit)
                return;

            UnselectButton(hit);
            UnselectSlider(hit);
        }
        void UnselectSlider(Transform hit)
        {
            MText_UI_SliderHandle sliderHandle = hit.gameObject.GetComponent<MText_UI_SliderHandle>();
            if (!InteractWithSlider(sliderHandle))
                return;

            sliderHandle.slider.UnSelectedVisual();
        }
        void UnselectButton(Transform hit)
        {
            MText_UI_Button button = hit.gameObject.GetComponent<MText_UI_Button>();
            if (!InteractWithButton(button))
                return;

            button.UnselectButton();
        }

        void Dragging(Transform hit)
        {
            DragSlider(hit);
            DragButton(hit);
        }
        void DragSlider(Transform hit)
        {
            MText_UI_SliderHandle sliderHandle = hit.gameObject.GetComponent<MText_UI_SliderHandle>();
            if (!InteractWithSlider(sliderHandle))
                return;

            Vector3 screenPoint = myCamera.WorldToScreenPoint(hit.position);
            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = myCamera.ScreenToWorldPoint(cursorScreenPoint);
            float size = sliderHandle.slider.backgroundSize;
            Vector3 localPosition = hit.InverseTransformPoint(cursorPosition);
            localPosition = new Vector3(localPosition.x, 0, 0);
            localPosition.x = Mathf.Clamp(localPosition.x, -size / 2, size / 2);
            hit.localPosition = localPosition;

            sliderHandle.GetCurrentValueFromHandle();
            sliderHandle.slider.ValueChanged();
        }
        void DragButton(Transform hit)
        {
            MText_UI_Button button = hit.gameObject.GetComponent<MText_UI_Button>();
            if (!InteractWithButton(button))
                return;
            button.ButtonBeingPressed();
        }

        void DragEnded(Transform hit, Transform currentTarget)
        {
            DragEndOnSlider(hit);
            DragEndOnButton(hit, currentTarget);
        }
        void DragEndOnSlider(Transform hit)
        {
            MText_UI_SliderHandle sliderHandle = hit.gameObject.GetComponent<MText_UI_SliderHandle>();
            if (!InteractWithSlider(sliderHandle))
                return;

            sliderHandle.slider.ValueChangeEnded();
        }
        void DragEndOnButton(Transform hit, Transform currentTarget)
        {
            MText_UI_Button button = hit.gameObject.GetComponent<MText_UI_Button>();
            if (!InteractWithButton(button))
                return;

            if (currentTarget != hit) button.selectedVisual = false;
            button.PressedButtonClickStopped();
        }

        bool InteractWithButton(MText_UI_Button button)
        {
            if (!button)
                return false;
            if (button.interactable && button.interactableByMouse)
                return true;

            return false;
        }
        bool InteractWithSlider(MText_UI_SliderHandle sliderHandle)
        {
            if (!sliderHandle)
                return false;
            if (sliderHandle.slider && sliderHandle.slider.interactable)
                return true;

            return false;
        }
        bool InteractWithInputString(Mtext_UI_InputString inputString)
        {
            if (!inputString)
                return false;
            if (inputString.interactable)
                return true;

            return false;
        }
    }
}