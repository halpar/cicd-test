using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class StackObject : MonoBehaviour
{
    [SerializeField] private Transform modelHolder;

    public int stackIndex { get; set; }


    public void CollectAnimation(float delay)
    {
        modelHolder.DOScale(1.5f, .15f).SetEase(Ease.InQuart).SetDelay(delay)
            .OnComplete(() => modelHolder.DOScale(1f, .15f).SetEase(Ease.OutQuart));
    }

    public void Pop()
    {
        Destroy(gameObject);
    }
}