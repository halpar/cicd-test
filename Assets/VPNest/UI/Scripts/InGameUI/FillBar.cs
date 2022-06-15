using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using Image = UnityEngine.UI.ProceduralImage.Image;


namespace VP.Nest.UI.InGame
{
    public class FillBar : MonoBehaviour
    {
        [SerializeField] Image fillImage;
        private bool isFill;
        private float startFillAmount = 0;
        private float endFillAmount = 1;

        [SerializeField] Color startColor = Color.red;
        [SerializeField] Color endColor = Color.green;
        [SerializeField] private AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private float maxValue = 0;

        public UnityAction OnComplate;

        private float currentAmount;

        private void Awake()
        {
            if (fillImage)
            {
                fillImage.fillAmount = startFillAmount;
                fillImage.color = startColor;
            }
        }

        public void SetupFillBar(float maxValue, bool isReverse = false)
        {
            if (isReverse)
            {
                startFillAmount = 1;
                endFillAmount = 0;
            }
            else
            {
                startFillAmount = 0;
                endFillAmount = 1;
            }

            this.maxValue = maxValue;
            isFill = false;
            fillImage.fillAmount = startFillAmount;
            fillImage.color = startColor;
            currentAmount = 0;
        }

        public void ChangeFillBar(float value)
        {
            currentAmount += value;
            UpdateFillBar(currentAmount);
        }

        public void SetFillBar(float value)
        {
            currentAmount = value;
            UpdateFillBar(currentAmount);
        }

        private void UpdateFillBar(float value)
        {
            if (maxValue != 0 && !isFill)
            {
                float targetValue = value / maxValue;
                fillImage.fillAmount = Mathf.Lerp(startFillAmount, endFillAmount, targetValue);
                fillImage.color = Color.Lerp(startColor, endColor, animationCurve.Evaluate(targetValue));
                if (targetValue >= 1)
                {
                    isFill = true;
                    OnComplate?.Invoke();
                }
            }
        }
    }
}