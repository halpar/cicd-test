using UnityEngine;
using VPNest.GameEvents.Scripts.Events;
using VPNest.GameEvents.Scripts.Listeners.Base;
using VPNest.GameEvents.Scripts.UnityEvents;

namespace VPNest.GameEvents.Scripts.Listeners
{
    public class Vector3Listener : BaseGameEventListener<Vector3, Vector3Event, UnityVector3Event>{}
}