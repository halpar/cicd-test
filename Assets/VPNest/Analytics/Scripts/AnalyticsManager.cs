using UnityEngine;
using VP.Nest.SceneManagement;
using System.Collections;
using NotImplementedException = System.NotImplementedException;

#if VP_FB_EXISTS
using Facebook.Unity;
#endif

#if VP_ELEPHANT_EXISTS
using ElephantSDK;
#endif


namespace VP.Nest.Analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
        private void Awake()
        {
#if VP_FB_EXISTS
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                FB.Init(InitCallback);
            }
#endif

            DontDestroyOnLoad(gameObject);
        }


        private void Start()
        {
#if VP_ELEPHANT_EXISTS || VP_BYRD_EXISTS
            StartCoroutine(LoadLevelScene());
#else
            GameManager.Instance.LoadCurrentLevelScene();
#endif
        }

        private IEnumerator LoadLevelScene()
        {
            float timer = 2;
#if VP_ELEPHANT_EXISTS || VP_BYRD_EXISTS
            float timeoutSec = 999;

#endif

#if VP_ELEPHANT_EXISTS
            while (!ElephantUI.isSDKReady)
            {
                timeoutSec -= Time.deltaTime;
                timer -= Time.deltaTime;
                if (timeoutSec < 0)
                {
                    break;
                }

                yield return null;
            }
#endif

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            yield return null;
            GameManager.Instance.LoadCurrentLevelScene();
        }

        public static void LogLevelStartEvent(int level)
        {
#if VP_ELEPHANT_EXISTS
            Elephant.LevelStarted(level);
#endif
        }

        public static void LogLevelFailEvent(int level, int? score = null)
        {
            if (score != null)
            {
#if VP_ELEPHANT_EXISTS
                Elephant.LevelFailed(level, Params.New().Set("score", score.Value));
#endif
            }
            else
            {
#if VP_ELEPHANT_EXISTS
                Elephant.LevelFailed(level);
#endif
            }
        }

        public static void LogLevelCompleteEvent(int level, int? score = null)
        {
            if (score != null)
            {
#if VP_ELEPHANT_EXISTS
                Elephant.LevelCompleted(level, Params.New().Set("score", score.Value));
#endif
            }
            else
            {
#if VP_ELEPHANT_EXISTS
                Elephant.LevelCompleted(level);
#endif
            }
        }

#if VP_FB_EXISTS
        private static void InitCallback()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }
#endif
        public static void LogStoreClickedEvent()
        {
        }

        public static void LogRandomSkinUnlockEvent(string unlockableClusterID, int unlockPrice = 0)
        {
        }

        public static void LogMoneyRewardedEvent()
        {
        }
    }
}