using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SortedEnumerator
{
    private SortedDictionary<int, IEnumerator> _coroutineDelegatesWithPriority;

    public void AddListener(IEnumerator action, int order)
    {
        if (_coroutineDelegatesWithPriority == null)
            _coroutineDelegatesWithPriority = new SortedDictionary<int, IEnumerator>();

        while (_coroutineDelegatesWithPriority.ContainsKey(order))
        {
            order++;
        }

        _coroutineDelegatesWithPriority.Add(order, action);
    }

    public IEnumerator[] GetInvocationList()
    {
        List<IEnumerator> funcs = new List<IEnumerator>();

        foreach (var VARIABLE in _coroutineDelegatesWithPriority.Values)
        {
            funcs.Add(VARIABLE);
        }

        return funcs.ToArray();
    }

    public bool IsNull
    {
        get => _coroutineDelegatesWithPriority == null;
    }

    public void SetNull()
    {
        _coroutineDelegatesWithPriority = null;
    }
}