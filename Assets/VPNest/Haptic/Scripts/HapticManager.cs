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
        private float maxTime = 0.4f;

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
                currentTime += Time.deltaTime;
                if (currentTime >= maxTime)
                {
                    currentTime = 0;
                    isLocked = false;
                }
            }


            if (isContinuousHapticContinue)
            {
                continuousDurationTime -= Time.deltaTime;
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

            var hapticTypeIndex = (int) hapticType;
            var mmvHaptic = (HapticTypes) hapticTypeIndex;
            MMVibrationManager.Haptic(mmvHaptic);
#if UNITY_EDITOR
            //   Debug.Log($"{hapticType} Triggered!");
#endif
        }

        public static void Haptic(AdvancedHapticType hapticType)
        {
            if (isContinuousHapticContinue)
                return;

            if (isLocked)
                return;

            isLocked = true;


            string hapticTypeName = hapticType.ToString();
            var jsonFile = Resources.Load<TextAsset>("AHAP/NV" + hapticTypeName);
            MMVibrationManager.AdvancedHapticPatternIOS(jsonFile.text);
#if UNITY_EDITOR
            Debug.Log($"{hapticType} Triggered!");
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