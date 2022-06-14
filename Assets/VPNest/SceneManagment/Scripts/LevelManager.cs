using System;
using UnityEngine;
using VP.Nest.Analytics;

namespace VP.Nest.SceneManagement
{
    public static class LevelManager
    {
        public static event Action LevelStart;
        public static event Action LevelComplete;
        public static event Action LevelFail;

        public static bool IsLevelPlaying;

        public static void StartLevel()
        {
            var level = PlayerPrefKeys.ReachedLevel;
            AnalyticsManager.LogLevelStartEvent(level);
            LevelStart?.Invoke();
            IsLevelPlaying = true;
        }

        public static void InitLevelComplete(int? score = null)
        {
            IsLevelPlaying = false;

            var level = PlayerPrefKeys.CurrentLevel;
            AnalyticsManager.LogLevelCompleteEvent(level, score);
            LevelComplete?.Invoke();

            level++;

            if (PlayerPrefKeys.CurrentLevel >= PlayerPrefKeys.ReachedLevel)
            {
                PlayerPrefKeys.ReachedLevel = level;
            }

            PlayerPrefKeys.CurrentLevel = level;
        }

        public static void InitLevelFail(int? score = null)
        {
            IsLevelPlaying = false;

            var level = PlayerPrefKeys.ReachedLevel;
            AnalyticsManager.LogLevelFailEvent(level, score);
            LevelFail?.Invoke();
        }
    }
}