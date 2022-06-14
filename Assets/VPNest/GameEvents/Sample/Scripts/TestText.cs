using UnityEngine;
using UnityEngine.UI;

namespace VPNest.GameEvents.Sample.Scripts
{
    public class TestText : MonoBehaviour
    {
        private Text _text;

        private int _currentCount;
        
        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        public void OnVoidEvent()
        {
            _currentCount++;
            _text.text = _currentCount.ToString();
        }

        public void OnIntEvent(int value)
        {
            _text.text = value.ToString();
        }

        public void OnTransformEvent(Transform t)
        {
            _text.text = t.name;
        }
    }
}
