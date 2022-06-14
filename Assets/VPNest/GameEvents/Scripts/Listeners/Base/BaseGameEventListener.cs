using UnityEngine;
using UnityEngine.Events;
using VPNest.GameEvents.Scripts.Events.Base;
using VPNest.GameEvents.Scripts.Listeners.Interface;

namespace VPNest.GameEvents.Scripts.Listeners.Base
{
    /// <typeparam name="T"> Generic Type </typeparam>
    /// <typeparam name="E"> Event </typeparam>
    /// <typeparam name="UER"> Unity Event Response </typeparam>
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T> where E :
    BaseGameEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField] private E gameEvent;

        private E GameEvent
        {
            get => gameEvent;
            set => gameEvent = value;
        }

        public UER OnUnityEventResponse => unityEventResponse;

        [SerializeField] private UER unityEventResponse;

        private void OnEnable()
        {
            if(gameEvent == null) return;

            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if(gameEvent == null) return;

            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            unityEventResponse?.Invoke(item);
        }
    }
    
}