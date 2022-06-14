using UnityEngine;
using VPNest.GameEvents.Scripts.Events.Base;

namespace VPNest.GameEvents.Scripts.Events
{
    [CreateAssetMenu(fileName = "New Transform Event", menuName = "Game Events/Transform Event", order = 5)]
    public class TransformEvent : BaseGameEvent<Transform> { }
}