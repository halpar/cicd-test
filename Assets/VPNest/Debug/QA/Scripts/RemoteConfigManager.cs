using System;
using System.Collections;
using System.Collections.Generic;
using ElephantSDK;
using QA;
using UnityEngine;
using VP.Nest.SceneManagement;

namespace ElephantSDK
{
    public class RemoteConfigManager : MonoBehaviour
    {
        public bool isOnQA { get; set; } = false;

        private void OnEnable()
        {
            //GameManager.OnGameSceneLoaded += LoadRemoteConfig;
            ElephantCore.onRemoteConfigLoaded += LoadRemoteConfig;
        }

        private void OnDisable()
        {
            //GameManager.OnGameSceneLoaded -= LoadRemoteConfig;
            ElephantCore.onRemoteConfigLoaded -= LoadRemoteConfig;
        }

        private void LoadRemoteConfig()
        {
            RemoteConfig.GetInstance().GetBool("is_on_qa", isOnQA);
            //You should get isOnQA info from elephant here.
            if (isOnQA)
            {
                DontDestroyOnLoad(QAPanel.Instance.gameObject);
                QAButtons.InitQAButtons();
            }
        }
    }
}