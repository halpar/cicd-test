using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QA
{
    public class QATimeScaleButton : MonoBehaviour
    {
        public Button timeScaleButton;
        private TextMeshProUGUI timeScaleTmp;
        private int clickCount;

        private void Awake()
        {
            timeScaleButton.onClick.AddListener(ChangeTimeScale);
            timeScaleTmp = GetComponentInChildren<TextMeshProUGUI>();
            timeScaleTmp.text = "Time Scale x1";
        }


        private void ChangeTimeScale()
        {
            switch (clickCount % 3)
            {
                case 0:
                    Time.timeScale = 1.5f;
                    timeScaleTmp.text = "Time Scale x1.5";
                    break;
                case 1:
                    Time.timeScale = 2f;
                    timeScaleTmp.text = "Time Scale x2";
                    break;
                case 2:
                    Time.timeScale = 1f;
                    timeScaleTmp.text = "Time Scale x1";
                    break;
            }

            clickCount++;
        }
    }
    
}
