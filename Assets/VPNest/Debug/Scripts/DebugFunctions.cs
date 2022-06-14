using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VP.Nest.UI;
using VP.Nest.UI.InGame;
using VP.Nest.Utilities;

namespace VP.Nest.DebugSystems
{
    public static class DebugFunctions
    {
        // Is Mouse over a UI Element? Used for ignoring World clicks through UI
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }

        //Draws the vectors in gizmos.
        public static void DebugDrawLines(Vector3 position, float size, List<Vector3> points, Color? color = null,
            float duration = float.MaxValue)
        {
            color = color != null ? color.Value : Color.black;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Debug.DrawLine(position + points[i] * size, position + points[i + 1] * size, color.Value, duration);
            }
        }

        // Create a Sprite in the UI with an debug action
        public static void CreateDebugUIButton(UnityAction action, int priority = 0, string btnText = "Debug Button")
        {
            GameObject gameObject = new GameObject();
            gameObject.name = "DebugButton";
            gameObject.transform.SetParent(UIManager.Instance.InGameUI.transform);

            UnityEngine.UI.Image img = gameObject.AddComponent<Image>();
            img.color = new Color(1, 1, 1, 0.647f);
            Button btn = gameObject.AddComponent<Button>();
            btn.onClick.AddListener(action);

            GameObject childObject = new GameObject();
            childObject.transform.SetParent(gameObject.transform);
            TextMeshProUGUI text = childObject.AddComponent<TextMeshProUGUI>();
            text.SetText(btnText);
            text.enableAutoSizing = true;
            text.alignment = TextAlignmentOptions.Center;
            text.color = new Color(0.3409f, 0.3962f, 0.3226f);

            RectTransform rectTransform = btn.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(60, -120 + (-90 * priority));
            rectTransform.sizeDelta = new Vector2(225f, 80f);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);

            gameObject.transform.localScale = Vector3.one;
        }

        public static void DebugDrawCircle(Vector3 center, float radius, Color color, int divisions,
            float duration = float.MaxValue)
        {
            for (int i = 0; i <= divisions; i++)
            {
                Vector3 vec1 = center +
                               VectorExtensions.ApplyRotationToVector(new Vector3(0, 1) * radius,
                                   (360f / divisions) * i);
                Vector3 vec2 = center +
                               VectorExtensions.ApplyRotationToVector(new Vector3(0, 1) * radius,
                                   (360f / divisions) * (i + 1));
                Debug.DrawLine(vec1, vec2, color, duration);
            }
        }

        // Create a Sprite in the UI where mouse is
        public static GameObject CreateUISpriteOnMouse(string name, Vector3 localScale, Sprite sprite = null,
            int sortingOrder = 0,
            Color? color = null)
        {
            Vector3 position = UsefulFunctions.GetWorldPositionFromUI_Perspective(Input.mousePosition);
            color = color.Value != null ? color.Value : Color.white;
            return CreateWorldSprite(name, position, localScale, null, sprite, sortingOrder, color.Value);
        }

        // Create a Sprite in the World
        public static GameObject CreateWorldSprite(string name, Vector3 localScale, Vector3 localPosition,
            Transform parent = null, Sprite sprite = null, int sortingOrder = 0, Color? color = null)
        {
            color = color != null ? color.Value : Color.white;
            GameObject gameObject = new GameObject(name, typeof(SpriteRenderer));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localScale = localScale;
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.color = color.Value;
            return gameObject;
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

        public static void DrawArrowGizmos(Vector3 from, Vector3 to, float arrowPointSize)
        {
            Vector3 direction = to - from;
            Vector3 left = -direction.normalized + Vector3.right / 2f;
            Vector3 right = -direction.normalized + Vector3.left / 2f;
            Gizmos.DrawRay(from, direction);
            Gizmos.DrawRay(to, right * arrowPointSize);
            Gizmos.DrawRay(to, left * arrowPointSize);
        }
    }
}