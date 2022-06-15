using UnityEngine;
using UnityEngine.EventSystems;
using VP.Nest.SceneManagement;

namespace VP.Nest.CreativeBuild
{
    public class CreativeToggle : MonoBehaviour
    {
        private CanvasGroup[] canvasGroups;

        private bool isDown = false;

        float elapsedTime = 0;


        void Start()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) =>
            {
                Debug.Log("Down");
                isDown = true;
            });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) =>
            {
                Debug.Log("Up");
                isDown = false;
            });
            trigger.triggers.Add(entry);
        }

        private void Update()
        {
            if (isDown)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= 3)
                {
                    GameManager.Instance.LoadLevelSelectScene();
                    isDown = false;
                    elapsedTime = 0;
                }
            }
            else
            {
                elapsedTime = 0;
            }
        }
    }
}