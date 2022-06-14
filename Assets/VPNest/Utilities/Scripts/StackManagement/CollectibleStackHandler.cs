using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleStackHandler : MonoBehaviour
{
    [SerializeField] private List<StackObject> stackList;
    [SerializeField] private bool isMexicanWaveOn;
    [SerializeField] private Transform initTransform;
    [SerializeField] private float smoothSpeed = 10;
    [SerializeField] private GameObject stackObject;
    [SerializeField] private CollectWay CollectWay;

    private float mexicanWaveDelayRate = 15;
    private Vector3 wayVector = Vector3.zero;
    private Coroutine asyncCoroutine;
    private float distance = 1.2f;
    public UnityAction OnStackListUpdated;
    
    public Vector3 LastCollectablePos
    {
        get => stackList[stackList.Count - 1].transform.position;
    }

    private void Awake()
    {
        AssignWayVector();
    }

    private void FixedUpdate()
    {
        SwingCollectables();
    }

    public void RemoveAtFromGivenIndexOfList(int index)
    {
        if (asyncCoroutine != null)
            StopCoroutine(asyncCoroutine);
        
        if (stackList.Count - 1 == index)
        {
            var collectable = stackList[index];
            stackList.RemoveAt(index);
            stackList[index].transform.DOKill();
            collectable.Pop();
            
            SetCollectablesIndex();
            return;
        }

        for (int i = stackList.Count - 1; i >= index; i--)
        {
            stackList.RemoveAt(i);
            SetCollectablesIndex();
        }
    }
    
    public void RemoveAtGivenIndexOfList(int index)
    {
        var car = stackList[index];
        car.transform.DOKill();
        stackList.RemoveAt(index);
        car.Pop();

        SetCollectablesIndex();
    }

    private void SwingCollectables()
    {
        for (var i = 0; i < stackList.Count; i++)
        {
            var currentStackItem = stackList[i].transform;

            Transform previousStackItem;
            Vector3 newPosition;
            if (i > 0)
            {
                previousStackItem = stackList[i - 1].transform;
                newPosition = previousStackItem.position + wayVector * distance;
            }
            else
            {
                previousStackItem = initTransform;
                newPosition = previousStackItem.position;
            }

            var currentStackItemPosition = currentStackItem.transform.position;
            var duration = smoothSpeed * Time.fixedDeltaTime;
            var position = Vector3.Lerp(currentStackItemPosition, newPosition, duration);
            var rotation = Quaternion.Slerp(currentStackItem.transform.rotation,
                previousStackItem.transform.rotation, duration);

            currentStackItem.transform.position = position;
            currentStackItem.transform.rotation = rotation;
        }
    }
    
    public void AddCollectableAsync(int amount)
    {
        if (asyncCoroutine != null)
        {
            StopCoroutine(asyncCoroutine);
            asyncCoroutine = null;
        }

        asyncCoroutine = StartCoroutine(AddCollectableCor(amount));
    }

    private IEnumerator AddCollectableCor(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos;

            if (stackList.Count > 0)
            {
                spawnPos = stackList[stackList.Count - 1].transform.position;
            }
            else
            {
                spawnPos = initTransform.position;
            }

            var newItem = Instantiate(stackObject, spawnPos, Quaternion.identity).GetComponent<StackObject>();
                
            stackList.Add(newItem);
            SetCollectablesIndex();
            yield return BetterWaitForSeconds.WaitRealtime(0.08f);
        }

        asyncCoroutine = null;
        if(isMexicanWaveOn) DoMexicanWave();
    }
    
    private void SetCollectablesIndex()
    {
        for (int i = 0; i < stackList.Count; i++)
        {
            stackList[i].stackIndex = i;
        }

        OnStackListUpdated?.Invoke();
    }

    private void AssignWayVector()
    {
        switch (CollectWay)
        {
            case CollectWay.up:
                wayVector = Vector3.up;
                break;
            case CollectWay.forward:
                wayVector = Vector3.forward;
                break;
            case CollectWay.back:
                wayVector = Vector3.back;
                break;
            case CollectWay.left:
                wayVector = Vector3.left;
                break;
            case CollectWay.right:
                wayVector = Vector3.right;
                break;
            
        }
    }
    
    private void DoMexicanWave()
    {
        for (int i = 0; i < stackList.Count; i++)
            stackList[stackList.Count - i - 1].CollectAnimation(i / mexicanWaveDelayRate);
    }
}
