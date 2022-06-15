using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Helper
{
    public static IEnumerator InvokeAction(Action action, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
        yield return null;
    }

    public static void Pause()
    {
        Debug.Break();
    }

    #region Logs

    public static void Log()
    {
        Debug.Log("-");
    }

    public static void Log(Object context)
    {
        Debug.Log("-", context);
    }

    public static void Log<T1>(T1 value)
    {
        Debug.Log($"{value}");
    }

    public static void Log<T1>(T1 value, Object context)
    {
        Debug.Log($"{value}", context);
    }

    public static void Log<T1, T2>(T1 value1, T2 value2)
    {
        Debug.Log($"{value1}, {value2}");
    }

    public static void Log<T1, T2>(T1 value1, T2 value2, Object context)
    {
        Debug.Log($"{value1}, {value2}", context);
    }

    public static void Log<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
        Debug.Log($"{value1}, {value2}, {value3}");
    }

    public static void Log<T1, T2, T3>(T1 value1, T2 value2, T3 value3, Object context)
    {
        Debug.Log($"{value1}, {value2}, {value3}", context);
    }

    public static void Log<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
    {
        Debug.Log($"{value1}, {value2}, {value3}, {value4}");
    }

    public static void Log<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4, Object context)
    {
        Debug.Log($"{value1}, {value2}, {value3}, {value4}", context);
    }

    public static void LogWarning()
    {
        Debug.LogWarning("-");
    }

    public static void LogWarning(Object context)
    {
        Debug.LogWarning("-", context);
    }

    public static void LogWarning<T1>(T1 value)
    {
        Debug.LogWarning($"{value}");
    }

    public static void LogWarning<T1>(T1 value, Object context)
    {
        Debug.LogWarning($"{value}", context);
    }

    public static void LogWarning<T1, T2>(T1 value1, T2 value2)
    {
        Debug.LogWarning($"{value1}, {value2}");
    }

    public static void LogWarning<T1, T2>(T1 value1, T2 value2, Object context)
    {
        Debug.LogWarning($"{value1}, {value2}", context);
    }

    public static void LogWarning<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
        Debug.LogWarning($"{value1}, {value2}, {value3}");
    }

    public static void LogWarning<T1, T2, T3>(T1 value1, T2 value2, T3 value3, Object context)
    {
        Debug.LogWarning($"{value1}, {value2}, {value3}", context);
    }

    public static void LogWarning<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
    {
        Debug.LogWarning($"{value1}, {value2}, {value3}, {value4}");
    }

    public static void LogWarning<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4, Object context)
    {
        Debug.LogWarning($"{value1}, {value2}, {value3}, {value4}", context);
    }

    public static void LogError()
    {
        Debug.LogError("-");
    }

    public static void LogError(Object context)
    {
        Debug.LogError("-", context);
    }

    public static void LogError<T1>(T1 value)
    {
        Debug.LogError($"{value}");
    }

    public static void LogError<T1>(T1 value, Object context)
    {
        Debug.LogError($"{value}", context);
    }

    public static void LogError<T1, T2>(T1 value1, T2 value2)
    {
        Debug.LogError($"{value1}, {value2}");
    }

    public static void LogError<T1, T2>(T1 value1, T2 value2, Object context)
    {
        Debug.LogError($"{value1}, {value2}", context);
    }

    public static void LogError<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
    {
        Debug.LogError($"{value1}, {value2}, {value3}");
    }

    public static void LogError<T1, T2, T3>(T1 value1, T2 value2, T3 value3, Object context)
    {
        Debug.LogError($"{value1}, {value2}, {value3}", context);
    }

    public static void LogError<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
    {
        Debug.LogError($"{value1}, {value2}, {value3}, {value4}");
    }

    public static void LogError<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4, Object context)
    {
        Debug.LogError($"{value1}, {value2}, {value3}, {value4}", context);
    }

    #endregion
}