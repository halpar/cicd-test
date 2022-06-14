using UnityEngine;
using VPNest.GameEvents.Scripts.Events.Base;

namespace VPNest.GameEvents.Scripts.Events
{
    [CreateAssetMenu(fileName = "New Int Event", menuName = "Game Events/Int Event", order = 1)]
    public class IntEvent : BaseGameEvent<int> { }
}