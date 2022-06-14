using UnityEngine;
using VPNest.GameEvents.Scripts.Events.Base;

namespace VPNest.GameEvents.Scripts.Events
{
    [CreateAssetMenu(fileName = "New String Event", menuName = "Game Events/String Event", order = 3)]
    public class StringEvent : BaseGameEvent<string> { }
}