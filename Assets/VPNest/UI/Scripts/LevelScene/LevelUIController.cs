using UnityEngine;
using VP.Nest.SceneManagement;

namespace VP.Nest.UI.LevelScene
{
	public class LevelUIController : MonoBehaviour
	{
		[SerializeField] private LevelUI template;
		[SerializeField] bool UnlockAll;

		private Transform content;

		private bool isLoadingScene = false;

		void Start()
		{
			content = template.transform.parent;
			for (int i = 0; i < GameSettings.SelectedSceneLoadSettings.gameSceneList.Count * 2; i++) {
				LevelUIStage uiStage;

				if (UnlockAll) {
					uiStage = LevelUIStage.Success;
				} else {
					if (i + 1 < PlayerPrefKeys.ReachedLevel) {
						uiStage = LevelUIStage.Success;
					} else if (i + 1 > PlayerPrefKeys.ReachedLevel) {
						uiStage = LevelUIStage.Locked;
					} else {
						uiStage = LevelUIStage.CurrentLevel;
					}

				}

				GameObject gO = Instantiate(template.gameObject, content);
				gO.SetActive(true);
				gO.GetComponent<LevelUI>().Setup(i + 1, this, uiStage);
			}
		}

		public void OnLevelUIClicked(int buttonId)
		{
			if (!isLoadingScene) {
				GameManager.Instance.LoadLevelSceneByLevelNo(buttonId);
				isLoadingScene = true;
			}
		}
	}

	public enum LevelUIStage
	{
		Success,
		CurrentLevel,
		Locked
	}
}