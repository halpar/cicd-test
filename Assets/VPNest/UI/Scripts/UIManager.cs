using System;
using System.Collections;
using UnityEngine;
using VP.Nest.UI.Currency;
using VP.Nest.UI.InGame;
using VP.Nest.SceneManagement;

namespace VP.Nest.UI
{
    public class UIManager : Singleton<UIManager>
    {
        #region References

        private InGameUI inGameUI;

        public InGameUI InGameUI
        {
            get
            {
                if (inGameUI == null)
                    inGameUI = GetComponentInChildren<InGameUI>(true);
                if (inGameUI == null)
                {
                    Debug.Log("InGameUI missing !");
                }

                return inGameUI;
            }
        }

        private CurrencyUI currencyUI;

        public CurrencyUI CurrencyUI
        {
            get
            {
                if (currencyUI == null)
                    currencyUI = GetComponentInChildren<CurrencyUI>(true);
                if (currencyUI == null)
                {
                    Debug.Log("CurrencyUI missing !");
                }

                return currencyUI;
            }
        }

        private FtueUI ftueUI;

        public FtueUI FtueUI
        {
            get
            {
                if (ftueUI == null)
                    ftueUI = GetComponentInChildren<FtueUI>(true);
                if (ftueUI == null)
                {
                    Debug.Log("FtueUI missing !");
                }

                return ftueUI;
            }
        }

        // private StoreSystem storeSystem;
        //
        // public StoreSystem StoreSystem
        // {
        //     get
        //     {
        //         if (storeSystem == null)
        //             storeSystem = GetComponentInChildren<StoreSystem>(true);
        //         if (storeSystem == null)
        //         {
        //             Debug.Log("StoreSystem missing !");
        //         }
        //
        //         if (storeSystem)
        //             storeSystem.Init();
        //
        //         return storeSystem;
        //     }
        // }

        // private LeaderboardUI leaderboard;
        //
        // public LeaderboardUI Leaderboard
        // {
        //     get
        //     {
        //         if (leaderboard == null)
        //             leaderboard = GetComponentInChildren<LeaderboardUI>(true);
        //         if (leaderboard == null)
        //         {
        //             Debug.Log("LeaderboardUI missing !");
        //         }
        //
        //         return leaderboard;
        //     }
        // }

        #endregion


        public SortedEnumerator PromptList = new SortedEnumerator();
        private bool isContinueOrFailed = false;

        public void SuccessGame()
        {
            if (!isContinueOrFailed)
            {
                LevelManager.InitLevelComplete();
                isContinueOrFailed = true;

                if (!PromptList.IsNull)
                {
                    StartCoroutine(SuccessGameCor());
                }
                else
                {
                    InGameUI.OpenSuccessPanel();
                }
            }
        }

        private IEnumerator SuccessGameCor()
        {
            IEnumerator[] func = PromptList.GetInvocationList();
            for (int i = 0; i < func.Length; i++)
            {
                yield return StartCoroutine(func[i]);
            }

            PromptList.SetNull();
            InGameUI.OpenSuccessPanel();
        }

        public void FailGame()
        {
            if (!isContinueOrFailed)
            {
                LevelManager.InitLevelFail();
                isContinueOrFailed = true;
                InGameUI.OpenFailPanel();
            }
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
    }
}