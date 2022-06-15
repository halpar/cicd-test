using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class UsefulFunctions
{
    public static void DrawArrowGizmo(Vector3 pos, Quaternion lookRotation, Vector2 size)
    {
        Vector3 direction = lookRotation * Vector3.forward * size.y;
        Vector3 left = lookRotation * Quaternion.Euler(0f, 160f, 0f) * Vector3.forward;
        Vector3 right = lookRotation * Quaternion.Euler(0f, 200f, 0f) * Vector3.forward;
        Gizmos.DrawRay(pos, direction);
        Gizmos.DrawRay(pos + direction, right * size.x);
        Gizmos.DrawRay(pos + direction, left * size.x);
    }
    
    public static Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    public static class BetterWaitForSeconds
    {
        private class WaitForSeconds : CustomYieldInstruction
        {
            private float waitUntil;

            public override bool keepWaiting
            {
                get
                {
                    if (Time.time < waitUntil)
                        return true;
                    Pool(this);
                    return false;
                }
            }

            public void Initialize(float seconds)
            {
                waitUntil = Time.time + seconds;
            }
        }

        private class WaitForSecondsRealtime : CustomYieldInstruction
        {
            private float waitUntil;

            public override bool keepWaiting
            {
                get
                {
                    if (Time.realtimeSinceStartup < waitUntil)
                        return true;
                    Pool(this);
                    return false;
                }
            }

            public void Initialize(float seconds)
            {
                waitUntil = Time.realtimeSinceStartup + seconds;
            }
        }

        private const int POOL_INITIAL_SIZE = 4;
        private static readonly Stack<WaitForSeconds> waitForSecondsPool;
        private static readonly Stack<WaitForSecondsRealtime> waitForSecondsRealtimePool;

        static BetterWaitForSeconds()
        {
            waitForSecondsPool = new Stack<WaitForSeconds>(POOL_INITIAL_SIZE);
            waitForSecondsRealtimePool = new Stack<WaitForSecondsRealtime>(POOL_INITIAL_SIZE);
            for (int i = 0; i < POOL_INITIAL_SIZE; i++)
            {
                waitForSecondsPool.Push(new WaitForSeconds());
                waitForSecondsRealtimePool.Push(new WaitForSecondsRealtime());
            }
        }

        public static CustomYieldInstruction Wait(float seconds)
        {
            WaitForSeconds instance;
            if (waitForSecondsPool.Count > 0)
                instance = waitForSecondsPool.Pop();
            else
                instance = new WaitForSeconds();
            instance.Initialize(seconds);
            return instance;
        }

        public static CustomYieldInstruction WaitRealtime(float seconds)
        {
            WaitForSecondsRealtime instance;
            if (waitForSecondsRealtimePool.Count > 0)
                instance = waitForSecondsRealtimePool.Pop();
            else
                instance = new WaitForSecondsRealtime();
            instance.Initialize(seconds);
            return instance;
        }

        private static void Pool(WaitForSeconds instance)
        {
            waitForSecondsPool.Push(instance);
        }

        private static void Pool(WaitForSecondsRealtime instance)
        {
            waitForSecondsRealtimePool.Push(instance);
        }
    }

    public static void ObliqueLaunch(Rigidbody rb, Vector3 targetPos, float maxHeight)
    {
        float g = Physics.gravity.y;
        float displacementY = targetPos.y - rb.transform.position.y;
        Vector3 displacementXZ =
            new Vector3(targetPos.x - rb.transform.position.x, 0, targetPos.z - rb.transform.position.z);
        float time = Mathf.Sqrt(-2 * maxHeight / g) + Mathf.Sqrt(2 * (displacementY - maxHeight) / g);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * g * maxHeight);
        Vector3 velocityXZ = displacementXZ / time;
        rb.velocity = velocityY + velocityXZ;
    }


    public static void DrawPathObliqueLaunchGizmos(Transform transform, Vector3 targetPos, float maxHeight)
    {
        float g = Physics.gravity.y;
        float displacementY = targetPos.y - transform.position.y;
        Vector3 displacementXZ =
            new Vector3(targetPos.x - transform.position.x, 0, targetPos.z - transform.position.z);
        float time = Mathf.Sqrt(-2 * maxHeight / g) + Mathf.Sqrt(2 * (displacementY - maxHeight) / g);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * g * maxHeight);
        Vector3 velocityXZ = displacementXZ / time;

        Vector3 initialVelocity = velocityY + velocityXZ;

        Vector3 previousDrawPoint = transform.position;
        int resolution = 30;

        Gizmos.color = Color.green;
        for (int i = 1; i <= resolution; i++)
        {
            float simTime = i / (float)resolution * time;
            Vector3 displacement = (initialVelocity * simTime) + simTime * simTime * Physics.gravity / 2f;
            Vector3 drawPoint = transform.position + displacement;
            Gizmos.DrawLine(previousDrawPoint, drawPoint);
            previousDrawPoint = drawPoint;
        }
    }

    private static float Map(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }
}