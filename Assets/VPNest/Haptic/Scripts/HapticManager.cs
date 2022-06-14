using System;
using UnityEngine;
using MoreMountains.NiceVibrations;
using VP.Nest.Haptic;

namespace VP.Nest.Haptic
{
    public class HapticManager : MonoBehaviour
    {
        private static bool isLocked = false;
        private float currentTime = 0;
        public float maxTime = 0.1f;

        private static bool isContinuousHapticContinue;
        private static float continuousDurationTime = 1;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            MMNViOS.iOSInitializeHaptics();
        }

        private void Update()
        {
            if (isLocked)
            {
                currentTime += Time.unscaledDeltaTime;
                if (currentTime >= maxTime)
                {
                    currentTime = 0;
                    isLocked = false;
                }
            }

            if (isContinuousHapticContinue)
            {
                continuousDurationTime -= Time.unscaledDeltaTime;
                if (continuousDurationTime <= 0)
                {
                    isContinuousHapticContinue = false;
                }
            }
        }

        private void OnDisable()
        {
            MMNViOS.iOSReleaseHaptics();
        }

        public static void Haptic(HapticType hapticType)
        {
            if (isContinuousHapticContinue)
                return;
            if (isLocked)
                return;

            isLocked = true;

            var hapticTypeIndex = (int)hapticType;
            var mmvHaptic = (HapticTypes)hapticTypeIndex;

            if (Application.platform == RuntimePlatform.Android)
            {
                mmvHaptic = HapticTypes.Selection;
            }

            MMVibrationManager.Haptic(mmvHaptic);
#if UNITY_EDITOR
            //   Debug.Log($"{hapticType} Triggered!");
#endif
        }
        
        public static void StartContinuousHaptic(float intensity, float sharpness, float duration = 1)
        {
            if (isContinuousHapticContinue)
                return;

            continuousDurationTime = duration;
            isContinuousHapticContinue = true;
            MMVibrationManager.ContinuousHaptic(intensity, sharpness, duration);
        }

        public static void StopContinuousHaptic()
        {
            if (isContinuousHapticContinue)
            {
                isContinuousHapticContinue = false;
                MMVibrationManager.StopContinuousHaptic();
            }
        }
    }
}