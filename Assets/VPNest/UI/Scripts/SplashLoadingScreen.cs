using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VP.Nest;

public class SplashLoadingScreen : MonoBehaviour
{
    [SerializeField] private RawImage gameIconAnimation;
    [SerializeField] private TextMeshProUGUI productNameText;
    [SerializeField] private TextMeshProUGUI loadingText;

    private string[] loadingTextArray = new string[] { "LOADING.", "LOADING..", "LOADING...", "LOADING" };

    private int currentIndex;

    [SerializeField] private float duration;
    [SerializeField] private AnimationCurve animationCurve;

    private void Awake()
    {
        currentIndex = 0;
        loadingText.SetText(loadingTextArray[currentIndex]);
        StartCoroutine(LoadingAnimation());

        StartCoroutine(IconAnimation());
    }


    public void Setup()
    {
        var gameConfig = GameConfigsSO.GetGameConfigsSO();
        gameIconAnimation.texture = gameConfig.icon;
        productNameText.SetText(gameConfig.productName.ToUpper());
    }

    IEnumerator IconAnimation()
    {
        float a = 1;

        while (true)
        {
            a = 1;
            gameIconAnimation.material.SetFloat("_Progression", a);
            yield return DOTween.To(() => a, x =>
            {
                a = x;
                gameIconAnimation.material.SetFloat("_Progression", a);
            }, 3, duration).SetDelay(0.5f).SetEase(animationCurve).WaitForCompletion();

            a = -0.5f;
            gameIconAnimation.material.SetFloat("_Progression", a);
            yield return DOTween.To(() => a, x =>
            {
                a = x;
                gameIconAnimation.material.SetFloat("_Progression", a);
            }, 0.5f, duration).SetDelay(0.5f).SetEase(animationCurve).WaitForCompletion();
        }
    }


    IEnumerator LoadingAnimation()
    {
        while (true)
        {
            yield return BetterWaitForSeconds.Wait(0.5f);
            currentIndex++;

            if (currentIndex >= loadingTextArray.Length)
                currentIndex = 0;

            loadingText.SetText(loadingTextArray[currentIndex]);
        }
    }

    private void OnDisable()
    {
        gameIconAnimation.material.SetFloat("_Progression", 3);
    }

    private void OnValidate()
    {
        if (gameIconAnimation && gameIconAnimation.material)
            gameIconAnimation.material.SetFloat("_Progression", 3);
    }


#if UNITY_EDITOR

    public static void SetupPrefab()
    {
        GameObject contentsRoot =
            UnityEditor.PrefabUtility.LoadPrefabContents("Assets/VPNest/UI/Prefabs/SplashLoadingScreen.prefab");

        SplashLoadingScreen splashLoadingScreen = contentsRoot.GetComponent<SplashLoadingScreen>();
        splashLoadingScreen.Setup();

        UnityEditor.PrefabUtility.SaveAsPrefabAsset(contentsRoot,
            "Assets/VPNest/UI/Prefabs/SplashLoadingScreen.prefab");
        UnityEditor.PrefabUtility.UnloadPrefabContents(contentsRoot);
    }

#endif
}