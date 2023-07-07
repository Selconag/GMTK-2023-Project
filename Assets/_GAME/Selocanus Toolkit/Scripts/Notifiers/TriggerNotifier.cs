using UnityEngine;
using UnityEngine.Events;

namespace SelocanusToolkit
{
    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider> { }
    public class TriggerNotifier : NotifierBase
    {
        public TriggerEvent OnEnterEvent;
        public TriggerEvent OnStayEvent;
        public TriggerEvent OnExitEvent;

        private bool _triggeredBefore;

        private void OnTriggerEnter(Collider other)
        {
            OnEnterEvent?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnStayEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {

            OnExitEvent?.Invoke(other);
        }
    }
}

