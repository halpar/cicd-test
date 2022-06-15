using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VP.Nest;
using VP.Nest.SceneManagement;
using VP.Nest.UI;

namespace VP.Nest.UI.InGame
{
	public class InGameUI : MonoBehaviour
	{
		private Transform InGameUIPanel;

		private EventTrigger tapToStartEventTrigger;
		private Button tapToRetryBtn;
		private Button tapToContinueBtn;
		private GameObject successPanel;
		private GameObject failPanel;

		private TextMeshProUGUI levelText;

		private FillBar fillBar;

		private bool isStarted, isContinue, isRetry, isTyringLoadScene;

		public UnityAction OnLevelStart;

		public FillBar FillBar {
			get {
				if (fillBar == null)
					fillBar = GetComponentInChildren<FillBar>();
				if (fillBar == null)
					Debug.Log("FillBar is missing! . Try SetActive UISystems/InGameUI/Fillbar gameobject");
				return fillBar;
			}
		}

		// Start is called before the first frame update
		void Start()
		{
			InGameUIPanel = transform;

			levelText = InGameUIPanel.Find("LevelBar").GetComponentInChildren<TextMeshProUGUI>();
			tapToStartEventTrigger = InGameUIPanel.Find("FullscreenTapToStart").GetComponent<EventTrigger>();
			failPanel = InGameUIPanel.Find("FullscreenFail").gameObject;
			tapToRetryBtn = failPanel.GetComponentInChildren<Button>();
			successPanel = InGameUIPanel.Find("FullscreenSuccess").gameObject;
			tapToContinueBtn = successPanel.GetComponentInChildren<Button>();

			levelText.SetText("LEVEL " + PlayerPrefKeys.CurrentLevel.ToString());

			tapToContinueBtn.onClick.AddListener(TapToContinue);
			tapToRetryBtn.onClick.AddListener(TapToRetry);

			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((eventData) => { TapToStart(); });
			tapToStartEventTrigger.triggers.Add(entry);

			successPanel.SetActive(false);
			failPanel.SetActive(false);
			tapToStartEventTrigger.gameObject.SetActive(true);
		}

		private void Update()
		{
#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.S))
				SuccessGame();
			if (Input.GetKeyDown(KeyCode.F))
				FailGame();
#endif
		}


		private void TapToContinue()
		{
			if (!isTyringLoadScene) {
				isTyringLoadScene = true;

				GameManager.Instance.LoadCurrentLevelScene();
			}
		}

		private void TapToRetry()
		{
			if (!isTyringLoadScene) {
				isTyringLoadScene = true;

				GameManager.Instance.LoadCurrentLevelScene();
			}
		}

		private void TapToStart()
		{
			if (!isStarted) {
				isStarted = true;
				OnLevelStart?.Invoke();
				tapToStartEventTrigger.gameObject.SetActive(false);
				LevelManager.StartLevel();
			}
		}

		public void SuccessGame()
		{
			if (!isContinue) {
				successPanel.SetActive(true);
				LevelManager.InitLevelComplete();
				isContinue = true;
			}
		}

		public void FailGame()
		{
			if (!isRetry) {
				failPanel.SetActive(true);
				LevelManager.InitLevelFail();
				isRetry = true;
			}
		}
	}
}