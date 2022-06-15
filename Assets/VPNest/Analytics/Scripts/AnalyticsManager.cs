using UnityEngine;
using VP.Nest.SceneManagement;
using System.Collections;
using System.Collections.Generic;

#if VP_FB_EXISTS
using Facebook.Unity;
#endif
#if VP_ELEPHANT_EXISTS
using ElephantSDK;
#endif

#if VP_BYRD_EXISTS
using ByrdSDK;

#endif


namespace VP.Nest.Analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
        private void Awake()
        {
#if VP_FB_EXISTS
			if (FB.IsInitialized) {
				FB.ActivateApp();
			} else {
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
#if VP_ELEPHANT_EXISTS || VP_BYRD_EXISTS
            float timeoutSec = 5;
#endif

#if VP_ELEPHANT_EXISTS
			while (!ElephantUI.isSDKReady) {

			timeoutSec -= Time.deltaTime;

			if(timeoutSec<0){
			break;
			}
			
				yield return null;
			}
#endif

#if VP_BYRD_EXISTS
            while (!Byrd.IsInit)
            {
                timeoutSec -= Time.deltaTime;

                if (timeoutSec < 0)
                {
                    break;
                }

                yield return null;
            }
#endif
            yield return null;

            GameManager.Instance.LoadCurrentLevelScene();
        }

        public static void LogLevelStartEvent(int level)
        {
#if VP_ELEPHANT_EXISTS
			Elephant.LevelStarted(level);
#endif

#if VP_BYRD_EXISTS
            Byrd.SendProgressionData(LevelEvent.Start);
#endif
        }

        public static void LogLevelFailEvent(int level, int? score = null)
        {
            if (score != null)
            {
#if VP_ELEPHANT_EXISTS
				Elephant.LevelFailed(level, Params.New().Set("score", score.Value));
#endif

#if VP_BYRD_EXISTS

                var customData = new ByrdSDK.DataModels.CustomData();

                customData.AddObject("score", score.Value);

                Byrd.SendProgressionData(LevelEvent.Fail, customData);
#endif
            }
            else
            {
#if VP_ELEPHANT_EXISTS
				Elephant.LevelFailed(level);
#endif


#if VP_BYRD_EXISTS

                Byrd.SendProgressionData(LevelEvent.Fail);
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


#if VP_BYRD_EXISTS

                var customData = new ByrdSDK.DataModels.CustomData();

                customData.AddObject("score", score.Value);

                Byrd.SendProgressionData(LevelEvent.Complete, customData);
#endif
            }
            else
            {
#if VP_ELEPHANT_EXISTS
				Elephant.LevelCompleted(level);
#endif


#if VP_BYRD_EXISTS

                Byrd.SendProgressionData(LevelEvent.Complete);
#endif
            }
        }

#if VP_FB_EXISTS
		private static void InitCallback()
		{
			if (FB.IsInitialized) {
				FB.ActivateApp();
			} else {
				Debug.Log("Failed to Initialize the Facebook SDK");
			}
		}
#endif
    }
}