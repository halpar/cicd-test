using UnityEngine;
using VPNest.GameEvents.Scripts.Events;

namespace VPNest.GameEvents.Sample.Scripts
{
    public class SampleEventRaiser : MonoBehaviour
    {
        [Header("Void Event")]
        [SerializeField] private VoidEvent onPressedSpace;
        
        [Header("Int Event")]
        [SerializeField] private IntEvent onPressedE;
        [SerializeField] private int intEventValue;
        
        [Header("Transform Event")]
        [SerializeField] private TransformEvent onPressedT;
        [SerializeField] private Transform transformValue;

        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                onPressedSpace.Raise();
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                onPressedE.Raise(intEventValue);
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                onPressedT.Raise(transformValue);
            }
        }
    }
}
