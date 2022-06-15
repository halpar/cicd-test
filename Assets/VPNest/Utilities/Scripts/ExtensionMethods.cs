using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public static class ExtensionMethods
{
    #region Vector

    public static Vector3 Flattened(this Vector3 vector)
    {
        return new Vector3(vector.x, 0f, vector.z);
    }

    public static float FlatDistance(this Vector3 origin, Vector3 destination)
    {
        return Vector3.Distance(origin.Flattened(), destination.Flattened());
    }

    public static Vector3 Abs(this Vector3 vector)
    {
        vector.x = Mathf.Abs(vector.x);
        vector.y = Mathf.Abs(vector.y);
        vector.z = Mathf.Abs(vector.z);
        return vector;
    }

    #endregion

    #region Array

    public static T RandomItem<T>(this T[] array)
    {
        if (array.Length == 0)
        {
            throw new System.IndexOutOfRangeException("Cannot select a random item from an empty array");
        }

        var rnd = new System.Random(Random.Range(0, 1000));
        var index = rnd.Next(0, array.Length);
        return array[index];
    }

    public static void Shuffle<T>(this T[] array)
    {
        var rng = new System.Random(Random.Range(0, 1000));
        var n = array.Length;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            var value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }

    #endregion

    #region List

    public static void Shuffle<T>(this IList<T> list)
    {
        var rng = new System.Random(Random.Range(0, 1000));
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        }

        var rnd = new System.Random(Random.Range(0, 1000));
        var index = rnd.Next(0, list.Count);
        return list[index];
    }

    #endregion

    #region Transform

    public static Vector3 ChangeX(this Transform trans, float x)
    {
        var position = trans.position;
        position.x = x;
        trans.position = position;
        return position;
    }

    public static Vector3 ChangeY(this Transform trans, float y)
    {
        var position = trans.position;
        position.y = y;
        trans.position = position;
        return position;
    }

    public static Vector3 ChangeZ(this Transform trans, float z)
    {
        var position = trans.position;
        position.z = z;
        trans.position = position;
        return position;
    }

    #endregion

    #region EventTrigger

    public static void AddListener(this EventTrigger trigger, EventTriggerType triggerType,
        UnityAction<BaseEventData> action)
    {
        if (trigger.triggers.Any(i => i.eventID == triggerType))
        {
            foreach (var entry in trigger.triggers.Where(entry => entry.eventID == triggerType))
            {
                entry.callback.AddListener(action);
                break;
            }
        }
        else
        {
            var entry = new EventTrigger.Entry
            {
                eventID = triggerType
            };
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }
    }

    public static void RemoveListener(this EventTrigger trigger, EventTriggerType triggerType,
        UnityAction<BaseEventData> action)
    {
        foreach (var entry in trigger.triggers.Where(entry => entry.eventID == triggerType))
        {
            entry.callback.RemoveListener(action);
            break;
        }
    }

    public static void RemoveAllListeners(this EventTrigger trigger, EventTriggerType triggerType)
    {
        foreach (var entry in trigger.triggers.Where(entry => entry.eventID == triggerType))
        {
            entry.callback.RemoveAllListeners();
            break;
        }
    }

    #endregion
}