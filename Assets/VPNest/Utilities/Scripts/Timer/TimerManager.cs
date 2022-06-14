using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VP.Nest.Utilities;

public class TimerManager : MonoBehaviour
{
    private static TimerManager instance = null;
    public List<Timer> _timers; // Holds a reference to all active timers

    public static TimerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("TimerManager").AddComponent<TimerManager>();
            }

            return instance;
        }
    }

    private void OnEnable()
    {
        instance = this;
    }
    
    private void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            _timers[i].UpdateTimer();
        }
    }
}