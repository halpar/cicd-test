using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using VP.Nest.Utilities;

namespace VP.Nest.SceneManagement
{
    public class GameManager : Singleton<GameManager>
    {
        private SceneLoadSettingsSO sceneLoadSettingsSo;

        private AsyncOperation _asyncOperation;

        public static UnityAction OnGameSceneLoaded;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            sceneLoadSettingsSo = GameSettings.SelectedSceneLoadSettings;
            Application.targetFrameRate = 60;
            
            SceneManager.sceneLoaded +=
                (arg0, arg1) =>
                {
                    if (arg0.name.Contains("GameScene"))
                    {
                        OnNewGameSceneLoaded();
                        OnGameSceneLoaded?.Invoke();
                    }
                };
        }

        private void OnNewGameSceneLoaded()
        {
            // if (PlayerPrefKeys.ReachedLevel >= 2)
            // {
            //     RequestStoreReviewPrompt();
            // }
        }

        public void LoadLevelSelectScene(bool showLoadingScene = false)
        {
            if (showLoadingScene)
            {
                StartCoroutine(StartLoadSceneProcessWithLoadingScene(sceneLoadSettingsSo.levelSelectScene.Name));
            }
            else
            {
                StartCoroutine(StartLoadSceneProcess(sceneLoadSettingsSo.levelSelectScene.Name));
            }
        }

        public void LoadCurrentLevelScene(bool showLoadingScene = false)
        {
            DOTween.CompleteAll();  
            DOTween.Clear();
            
            var level = PlayerPrefKeys.CurrentLevel;
            int levelCount;

            //   Debug.Log("level " + level); 

            if (level <= sceneLoadSettingsSo.tutorialScenes.Count)
                levelCount = sceneLoadSettingsSo.tutorialScenes.Count;
            else
            {
                levelCount = sceneLoadSettingsSo.gameSceneList.Count;
                level -= sceneLoadSettingsSo.tutorialScenes.Count;
            }

            //   Debug.Log("levelcount " + levelCount);

            var currentGameSceneIndexForLevel = level % levelCount;

            //    Debug.Log("currentGameSceneIndexForLevel " + currentGameSceneIndexForLevel);

            var selectedGameSceneIndex =
                currentGameSceneIndexForLevel == 0 ? levelCount : currentGameSceneIndexForLevel;
            string selectedGameSceneName;


            if (PlayerPrefKeys.CurrentLevel <= sceneLoadSettingsSo.tutorialScenes.Count)
            {
                //       Debug.Log("selectedGameSceneIndex " + (selectedGameSceneIndex - 1));
                selectedGameSceneName = sceneLoadSettingsSo.tutorialScenes[selectedGameSceneIndex - 1].Name;
            }
            else
            {
                //      Debug.Log("selectedGameSceneIndex " +
                //                (selectedGameSceneIndex - sceneLoadSettingsSo.tutorialScenes.Count - 1));

                selectedGameSceneName = sceneLoadSettingsSo
                    .gameSceneList[selectedGameSceneIndex - 1].Name;
            }

            //    Debug.Log("selectedGameSceneName " + selectedGameSceneName);

            if (showLoadingScene)
            {
                StartCoroutine(StartLoadSceneProcessWithLoadingScene(selectedGameSceneName));
            }
            else
            {
                StartCoroutine(StartLoadSceneProcess(selectedGameSceneName));
            }
        }

        public void LoadLevelSceneByLevelNo(int levelNo, bool showLoadingScene = false)
        {
            PlayerPrefKeys.CurrentLevel = levelNo;

            LoadCurrentLevelScene(showLoadingScene);
        }

        private IEnumerator StartLoadSceneProcessWithLoadingScene(string sceneName)
        {
            yield return null;

            var minDurationForSplash = SceneManager.GetActiveScene().buildIndex == 0 ? 0f : 0f;

            SceneManager.LoadSceneAsync(sceneLoadSettingsSo.loadingScene.Name, LoadSceneMode.Single);

            _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            _asyncOperation.allowSceneActivation = false;

            while (!_asyncOperation.isDone || minDurationForSplash > 0f)
            {
                minDurationForSplash -= Time.deltaTime;
                if (_asyncOperation.progress >= 0.9f && minDurationForSplash <= 0f)
                {
                    _asyncOperation.allowSceneActivation = true;
                    SceneManager.UnloadSceneAsync(sceneLoadSettingsSo.loadingScene.Name);
                }

                yield return null;
            }

            yield return null;
        }

        private IEnumerator StartLoadSceneProcess(string sceneName)
        {
            yield return null;

            var minDurationForSplash = SceneManager.GetActiveScene().buildIndex == 0 ? 0f : 0f;

            _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            _asyncOperation.allowSceneActivation = false;

            while (!_asyncOperation.isDone || minDurationForSplash > 0f)
            {
                minDurationForSplash -= Time.deltaTime;
                if (_asyncOperation.progress >= 0.9f && minDurationForSplash <= 0f)
                {
                    _asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            yield return null;
        }

        // Default is set to Level 1
        public static void RequestStoreReviewPrompt()
        {
#if UNITY_IOS
            if (PlayerPrefs.HasKey("Rated")) return;

            UnityEngine.iOS.Device.RequestStoreReview();
            PlayerPrefs.SetInt("Rated", 1);
#endif
        }
    }
}