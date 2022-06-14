using UnityEngine;
using VPNest.GameEvents.Scripts.Events.Base;

namespace VPNest.GameEvents.Scripts.Events
{
    [CreateAssetMenu(fileName = "New Float Event", menuName = "Game Events/Float Event", order = 2)]
    public class FloatEvent : BaseGameEvent<float> { }
}