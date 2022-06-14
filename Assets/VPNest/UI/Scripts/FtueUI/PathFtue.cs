using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VP.Nest.UI;
using VP.Nest.UI.TapFeedback;
using VP.Nest.Utilities;

public class PathFtue : MonoBehaviour
{
    [SerializeField] private RectTransform handT;
    [SerializeField] private bool isLooped;
    private TapFeedbackController _tapFeedbackController;

    private Canvas canvas;
    private float xRotation = 30f;
    private float yRotation = 0f;
    private float zRotation = 15f;
    Tween movePosTween;
    public List<PathPoint> points;

    private void Awake()
    {
        _tapFeedbackController = FindObjectOfType<TapFeedbackController>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        handT.anchoredPosition = points[0].targetPos;
    }

    private void OnEnable()
    {
        StartCoroutine(DoHandMove());
        InitPoints();
    }

    private void InitPoints()
    {
        foreach (var point in points)
        {
            if (point.target.TryGetComponent(out RectTransform rt))
            {
                point.targetPos = rt.anchoredPosition;
            }
            else if (point.target.TryGetComponent(out Transform t))
            {
                point.targetPos = UsefulFunctions.WorldToCanvasPosition(t, canvas);
            }
        }
    }

    private void OnDisable()
    {
        DOTween.Kill("FingerMove");
        handT.anchoredPosition = points[0].targetPos;
        handT.eulerAngles = Vector3.zero;
    }

    IEnumerator DoHandMove()
    {
        bool hasHandPressed = false;
        while (true)
        {
            for (int i = 0; i < points.Count; i++)
            {
                yield return StartCoroutine(MovePosition(points[i].targetPos, points[i].handSpeed));
                if (hasHandPressed != points[i].shouldFingerPress)
                {
                    if (points[i].shouldFingerPress)
                    {
                        yield return StartCoroutine(Tap(0.4f));
                    }
                    else
                    {
                        yield return StartCoroutine(Untap(0.2f));
                    }

                    hasHandPressed = points[i].shouldFingerPress;
                }
            }
            if (!isLooped) handT.anchoredPosition = points[0].targetPos;
        }
    }

    IEnumerator Tap(float duration, bool useParticle = false)
    {
        handT.DORotate(new Vector3(xRotation, yRotation, zRotation), duration).SetId("FingerMove").timeScale = 1/Time.timeScale;
        yield return BetterWaitForSeconds.WaitRealtime(duration - 0.1f);
        if (useParticle) _tapFeedbackController.PlayParticle(handT.position);
    }

    IEnumerator Untap(float duration)
    {
        handT.DORotate(Vector3.zero, duration).SetId("FingerMove").timeScale = 1/Time.timeScale;
        yield return BetterWaitForSeconds.WaitRealtime(duration);
    }

    IEnumerator MovePosition(Vector2 targetPos, float handSpeed)
    {
        float distance = (targetPos - handT.anchoredPosition).magnitude;
        float duration = distance / handSpeed;
        handT.DOAnchorPos(targetPos, duration).SetId("FingerMove").timeScale = 1/Time.timeScale;
        yield return BetterWaitForSeconds.WaitRealtime(duration);
    }

    [Serializable]
    public class PathPoint
    {
        public bool shouldFingerPress;
        public GameObject target;
        public float handSpeed = 300;
        public Vector2 targetPos { get; set; }
    }
}