
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace VP.Nest.UI
{
    public class LoadingSphereAnimation : MonoBehaviour
    {
        private List<RectTransform> rectTransforms;
        private List<Image> rectTransformImages;
        private List<Vector3> rectTransformStartPos;

        [SerializeField] private float speed = 5;

        [SerializeField] [ColorUsage(true)] private Color startColor;
        [SerializeField] [ColorUsage(true)] private Color endColor;

        private float maxDistance;

        private void Awake()
        {
            rectTransforms = new List<RectTransform>();
            rectTransformStartPos = new List<Vector3>();
            rectTransformImages = new List<Image>();
            for (int i = 0; i < transform.childCount; i++)
            {
                rectTransforms.Add(transform.GetChild(i).GetComponent<RectTransform>());
                rectTransformStartPos.Add(transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition);
                rectTransformImages.Add(transform.GetChild(i).GetComponent<Image>());
            }

            maxDistance = rectTransformStartPos[rectTransformStartPos.Count - 1].x - rectTransformStartPos[0].x;
        }

        private void Start()
        {
            StartCoroutine(LoadingAnimationCoroutine());
        }

        private void Update()
        {
            for (int i = 0; i < rectTransformImages.Count; i++)
            {
                float currentPosition = rectTransformStartPos[rectTransformStartPos.Count - 1].x -
                                        rectTransforms[i].anchoredPosition.x;

                float value = currentPosition / maxDistance;
                rectTransformImages[i].color = Color.Lerp(endColor, startColor, value);
            }
        }

        IEnumerator LoadingAnimationCoroutine()
        {
            while (true)
            {
                yield return null;
                rectTransforms[0].DOAnchorPos(rectTransformStartPos[rectTransformStartPos.Count - 1],
                    (1f / speed) * rectTransforms.Count);
                for (int i = 1; i < rectTransforms.Count; i++)
                {
                    yield return rectTransforms[i].DOJumpAnchorPos(rectTransformStartPos[i - 1], 100, 1, 1f / speed)
                        .WaitForCompletion();
                }

                rectTransforms[0].DOAnchorPos(rectTransformStartPos[0], (1f / speed) * rectTransforms.Count);

                for (int i = rectTransforms.Count - 1; i > 0; i--)
                {
                    yield return rectTransforms[i].DOJumpAnchorPos(rectTransformStartPos[i], -100, 1, 1f / speed)
                        .WaitForCompletion();
                }
            }
        }
    }
}