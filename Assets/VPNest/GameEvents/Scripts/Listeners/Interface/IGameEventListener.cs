namespace VPNest.GameEvents.Scripts.Listeners.Interface
{
    public interface IGameEventListener<in T>
    {
        public void OnEventRaised(T item);
    }
}