using UnityEngine;
using UnityEngine.Events;

namespace SelocanusToolkit
{
    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }
    public class CollisionNotifier : NotifierBase
    {
        public CollisionEvent OnEnterEvent;
        public CollisionEvent OnStayEvent;
        public CollisionEvent OnExitEvent;

        private void OnCollisionEnter(Collision collision)
        {
            OnEnterEvent?.Invoke(collision);

        }

        private void OnCollisionStay(Collision collision)
        {
            OnStayEvent?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            OnExitEvent?.Invoke(collision);

        }
    }
}

