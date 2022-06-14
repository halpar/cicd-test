using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VP.Nest.Utilities
{
    public class Timer
    {
        private static GameObject _initGameObject; // Global game object used for initializing class, is destroyed on scene change
        private static bool _isInited = false;
        public Action OnTimerEnds;
        
        private static void InitIfNeeded()
        {
            if (_isInited == false)
            {
                _isInited = true;
                TimerManager.Instance._timers = new List<Timer>();
            }
        }
        

        /// <summary>
        /// Triggers an action in every interval for given iteration value
        /// </summary>
        /// <param name="action">Action to be run</param>S
        /// <param name="interval">The interval between every action</param>
        /// <param name="iteration">Value indicating how many times the desired action will be performed.</param>
        /// <param name="triggerImmediately">Whether the action will start immediately or not</param>
        public static Timer Create(Action action, float interval, int iteration = -1, bool triggerImmediately = false)
        {
            return Create(action, interval, "", false, false, iteration, triggerImmediately);
        }

        /// <summary>
        /// Triggers an action in every interval for given iteration value
        /// </summary>
        /// <param name="action">Action to be run</param>
        /// <param name="interval">The interval between every action</param>
        /// <param name="functionID">The ID key of the function</param>
        /// <param name="iteration">Value indicating how many times the desired action will be performed.</param>
        /// <param name="triggerImmediately">Whether the action will start immediately or not</param>
        /// <returns></returns>
        public static Timer Create(Action action, float interval, string functionID, int iteration = -1, bool triggerImmediately = false)
        {
            return Create(action, interval, functionID, false, false, iteration, triggerImmediately);
        }

        /// <summary>
        /// Triggers an action in every interval for given iteration value
        /// </summary>
        /// <param name="action">Action to be run</param>
        /// <param name="interval">The interval between every action</param>
        /// <param name="useUnscaledTime">Whether the speed of the action changes according to delta time value or not</param>
        /// <param name="functionID">The ID key of the function</param>
        /// <param name="iteration">Value indicating how many times the desired action will be performed.</param>
        /// <param name="triggerImmediately">Whether the action will start immediately or not</param>
        /// <returns></returns>
        public static Timer Create(Action action, float interval, bool useUnscaledTime, string functionID, int iteration = -1,
            bool triggerImmediately = false)
        {
            return Create(action, interval, functionID, false, useUnscaledTime, iteration, triggerImmediately);
        }

        /// <summary>
        /// Triggers an action in every interval for given iteration value 
        /// </summary>
        /// <param name="action">Action to be run</param>
        /// <param name="interval">The interval between every action</param>
        /// <param name="functionID">The ID key of the function</param>
        /// <param name="stopAllWithSameID">Value specifying whether other timers with the same id with this timer should be destroyed.</param>
        /// <param name="iteration">Value indicating how many times the desired action will be performed.</param>
        /// <param name="triggerImmediately">Whether the action will start immediately or not</param>
        /// <returns></returns>
        public static Timer Create(Action action, float interval, string functionID, bool stopAllWithSameID, int iteration = -1,
            bool triggerImmediately = false)
        {
            return Create(action, interval, functionID, stopAllWithSameID, false, iteration, triggerImmediately);
        }

        /// <summary>
        /// Triggers an action in every interval for given iteration value
        /// </summary>
        /// <param name="action">Action to be run</param>
        /// <param name="interval">The interval between every action</param>
        /// <param name="iteration">Value indicating how many times the desired action will be performed.</param>
        /// <param name="functionID">The ID key of the function</param>
        /// <param name="useUnscaledDeltaTime">Whether the speed of the action changes according to delta time value or not</param>
        /// <param name="triggerImmediately">Whether the action will start immediately or not</param>
        /// <param name="stopAllWithSameID">Value specifying whether other timers with the same id with this timer should be destroyed.</param>
        /// <returns></returns>
        public static Timer Create(Action action, float interval, string functionID, bool stopAllWithSameID, bool useUnscaledDeltaTime,
            int iteration = -1, bool triggerImmediately = false)
        {
            InitIfNeeded();

            if (stopAllWithSameID)
            {
                StopAllTimers(functionID);
            }

            if (triggerImmediately)
            {
                action();
                iteration--;
            }

            Timer timer = new Timer(action, interval, iteration, functionID, useUnscaledDeltaTime);
            TimerManager.Instance._timers.Add(timer);

            return timer;
        }

        private static void RemoveTimer(Timer funcTimer)
        {
            InitIfNeeded();
            TimerManager.Instance._timers.Remove(funcTimer);
        }


        /// <summary>
        /// Destroys a timer with a certain ID (If you have timers with the same ID it destroys the first one)
        /// </summary>
        /// <param name="_name">ID of the timer to be destroyed</param>
        public static void StopTimer(string _name)
        {
            InitIfNeeded();
            for (int i = 0; i < TimerManager.Instance._timers.Count; i++)
            {
                if (TimerManager.Instance._timers[i].functionID == _name)
                {
                    TimerManager.Instance._timers[i].DestroySelf();
                    return;
                }
            }
        }

        /// <summary>
        /// Stops all of the timers
        /// </summary>
        /// <param name="_name"></param>
        public static void StopAllTimers(string _name)
        {
            InitIfNeeded();
            for (int i = 0; i < TimerManager.Instance._timers.Count; i++)
            {
                if (TimerManager.Instance._timers[i].functionID == _name)
                {
                    TimerManager.Instance._timers[i].DestroySelf();
                    i--;
                }
            }
        }
        
        public static Timer GetTimer(string _name)
        {
            InitIfNeeded();
            for (int i = 0; i < TimerManager.Instance._timers.Count; i++)
            {
                if (TimerManager.Instance._timers[i].functionID == _name)
                {
                    return TimerManager.Instance._timers[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Returns true if the timer is active
        /// </summary>
        /// <param name="name">ID of the timer</param>
        /// <returns></returns>
        public static bool IsFuncActive(string name)
        {
            InitIfNeeded();
            for (int i = 0; i < TimerManager.Instance._timers.Count; i++)
            {
                if (TimerManager.Instance._timers[i].functionID == name)
                {
                    return true;
                }
            }

            return false;
        }

        private GameObject gameObject;
        private float interval;
        private float baseInterval;
        private bool useUnscaledDeltaTime;
        private int iteration;
        private string functionID;
        public Action action;


        private Timer(Action action, float interval, int iteration, string functionID, bool useUnscaledDeltaTime)
        {
            this.gameObject = gameObject;
            this.action = action;
            this.interval = interval;
            this.functionID = functionID;
            this.useUnscaledDeltaTime = useUnscaledDeltaTime;
            this.iteration = iteration;
            baseInterval = interval;
        }

        /// <summary>
        /// Changes the current interval for a timer
        /// </summary>
        /// <param name="interval">Interval of the timer</param>
        public void SkipIntervalTo(float interval)
        {
            this.interval = interval;
        }


        public void SetBaseInterval(float baseInterval)
        {
            this.baseInterval = baseInterval;
        }

        public float GetBaseInterval()
        {
            return baseInterval;
        }

        public void UpdateTimer()
        {
            if (useUnscaledDeltaTime)
            {
                interval -= Time.unscaledDeltaTime;
            }
            else
            {
                interval -= Time.deltaTime;
            }

            if (interval <= 0)
            {
                if (iteration.Equals(0))
                {
                    //Destroy
                    OnTimerEnds?.Invoke();
                    DestroySelf();
                }
                else
                {
                    //Repeat
                    action();
                    iteration--;
                    interval += baseInterval;
                }
            }
        }

        public void DestroySelf()
        {
            RemoveTimer(this);
        }
    }
}