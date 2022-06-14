using UnityEngine;
using VPNest.GameEvents.Scripts.Events;
using VPNest.GameEvents.Scripts.Listeners.Base;
using VPNest.GameEvents.Scripts.UnityEvents;

namespace VPNest.GameEvents.Scripts.Listeners
{
    public class TransformListener : BaseGameEventListener<Transform, TransformEvent, UnityTransformEvent>{}
}