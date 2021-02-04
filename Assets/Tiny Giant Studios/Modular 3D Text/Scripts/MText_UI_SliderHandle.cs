using UnityEngine;

namespace MText
{
    public class MText_UI_SliderHandle : MonoBehaviour
    {
        public MText_UI_Slider slider;

        public void GetCurrentValueFromHandle()
        {
            //int multiplier = 1;
            //if (slider.directionChoice == 1) multiplier = -1;
            //slider.currentValue = RangeConvertedValue(transform.localPosition.x, multiplier * (-slider.backgroundSize / 2), multiplier * (slider.backgroundSize / 2), slider.minValue, slider.maxValue);
            slider.currentValue = RangeConvertedValue(transform.localPosition.x, (-slider.backgroundSize / 2), (slider.backgroundSize / 2), slider.minValue, slider.maxValue);

            UpdateProgressBar();
        }



        public void GetHandlePositionFromSlider()
        {
            if (!slider)
            {
                Debug.LogWarning("Slider handle doesn't have a Slider assigned", gameObject);
                return;
            }

            int multiplier = -1;
            if (slider.directionChoice == 1) multiplier = 1;

            Vector3 pos = transform.localPosition;
            pos.x = multiplier * RangeConvertedValue(slider.currentValue, slider.minValue, slider.maxValue, slider.backgroundSize / 2, -slider.backgroundSize / 2);
            transform.localPosition = pos;

            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            if (!slider.progressBar)
                return;

            Vector3 scale = slider.progressBar.localScale;
            scale.x = ((slider.currentValue - slider.minValue) / (slider.maxValue - slider.minValue)) * slider.backgroundSize;
            slider.progressBar.localScale = scale;


            Vector3 pos = slider.progressBar.localPosition;
            pos.x = -slider.backgroundSize / 2;
            slider.progressBar.localPosition = pos;
        }

        float RangeConvertedValue(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
        }
    }
}