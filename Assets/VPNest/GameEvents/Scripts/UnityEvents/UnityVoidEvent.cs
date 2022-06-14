using System;
using UnityEngine.Events;
using Void = VPNest.GameEvents.Scripts.DataType.Void;

namespace VPNest.GameEvents.Scripts.UnityEvents
{
    [Serializable] public class UnityVoidEvent : UnityEvent<Void> { }
}