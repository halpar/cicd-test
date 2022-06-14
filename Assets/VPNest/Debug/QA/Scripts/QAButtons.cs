using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VP.Nest.DebugSystems;
using VP.Nest.UI;
using VP.Nest.UI.InGame;

namespace  QA
{
    public static class QAButtons
    {
        public static void InitQAButtons()
        {
            QAPanel.Instance.QAMoneyButton.gameObject.SetActive(true);
            QAPanel.Instance.QATimeScaleButton.gameObject.SetActive(true);
        }
    }
    
}
