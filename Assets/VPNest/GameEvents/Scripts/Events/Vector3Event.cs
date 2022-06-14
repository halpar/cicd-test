using UnityEngine;
using VPNest.GameEvents.Scripts.Events.Base;

namespace VPNest.GameEvents.Scripts.Events
{
    [CreateAssetMenu(fileName = "New Vector3 Event", menuName = "Game Events/Vector3 Event", order = 4)]
    public class Vector3Event : BaseGameEvent<Vector3> { }
}