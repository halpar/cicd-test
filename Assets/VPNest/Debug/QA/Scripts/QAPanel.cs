using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QA
{
    public class QAPanel : Singleton<QAPanel>
    {
        public QAMoneyButton QAMoneyButton { get; set; }
        public QATimeScaleButton QATimeScaleButton { get; set; }
        
        private void Awake()
        {
            QAMoneyButton = GetComponentInChildren<QAMoneyButton>(true);
            QATimeScaleButton = GetComponentInChildren<QATimeScaleButton>(true);
        }
    }
    
}
