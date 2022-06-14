using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VP.Nest.UI.LevelScene
{
    public class LevelUI : MonoBehaviour
    {
        private int levelID;
        private LevelUIController controller;

        private TextMeshProUGUI levelText;
        private GameObject successPanel;
        private GameObject currentLevelPanel;
        private GameObject lockedPanel;

        private Button button;

        // Start is called before the first frame update
        void Awake()
        {
            levelText = GetComponentInChildren<TextMeshProUGUI>();
            button = GetComponentInChildren<Button>();

            Transform panelHolder = transform.Find("StatusHolder");

            successPanel = panelHolder.Find("SuccessPanel").gameObject;
            currentLevelPanel = panelHolder.Find("CurrentLevelPanel").gameObject;
            lockedPanel = panelHolder.Find("LockedPanel").gameObject;

            button.onClick.AddListener(() => { controller.OnLevelUIClicked(levelID); });
        }

        public void Setup(int levelID, LevelUIController controller, LevelUIStage stage)
        {
            this.levelID = levelID;
            this.controller = controller;

            successPanel.SetActive(false);
            currentLevelPanel.SetActive(false);
            lockedPanel.SetActive(false);

            levelText.SetText(levelID.ToString());

            switch (stage)
            {
                case LevelUIStage.Success:
                    button.interactable = true;
                    successPanel.SetActive(true);
                    break;
                case LevelUIStage.CurrentLevel:
                    button.interactable = true;
                    currentLevelPanel.SetActive(true);
                    break;
                case LevelUIStage.Locked:
                    button.interactable = false;
                    lockedPanel.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
            }
        }
    }
}